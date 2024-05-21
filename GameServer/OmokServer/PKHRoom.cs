using MemoryPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using CSCommon;

namespace PvPGameServer;

public class PKHRoom : PKHandler
{
    List<Room> _roomList = null;
    int _startRoomNumber;

    public void SetRoomList(List<Room> roomList)
    {
        _roomList = roomList;
        _startRoomNumber = roomList[0].Number;
    }

    public void RegistPacketHandler(Dictionary<int, Action<MemoryPackBinaryRequestInfo>> packetHandlerMap)
    {
        packetHandlerMap.Add((int)PACKETID.REQ_ROOM_ENTER, RequestRoomEnter);
        packetHandlerMap.Add((int)PACKETID.REQ_ROOM_LEAVE, RequestLeave);
        packetHandlerMap.Add((int)PACKETID.NTF_IN_ROOM_LEAVE, NotifyLeaveInternal);
        packetHandlerMap.Add((int)PACKETID.REQ_ROOM_CHAT, RequestChat);
        packetHandlerMap.Add((int)PACKETID.REQ_READY_OMOK, RequestGameReady);
        packetHandlerMap.Add((int)PACKETID.REQ_PUT_MOK, RequestPutMok);
        packetHandlerMap.Add((int)PACKETID.NTF_IN_TIME_OVER, OnTurnTimeOut);
        packetHandlerMap.Add((int)PACKETID.NTF_IN_TOO_LONG_GAME, HandleTooLongGameRoom);
    }


    Room GetRoom(int roomNumber)
    {
        var index = roomNumber - _startRoomNumber;

        if (index < 0 || index >= _roomList.Count())
        {
            return null;
        }

        return _roomList[index];
    }

    (bool, Room, RoomUser) CheckRoomAndRoomUser(string userNetSessionID)
    {
        var user = _userMgr.GetUser(userNetSessionID);
        if (user == null)
        {
            return (false, null, null);
        }

        var roomNumber = user.RoomNumber;
        var room = GetRoom(roomNumber);
        
        if (room == null)
        {
            return (false, null, null);
        }

        var roomUser = room.GetUserByNetSessionId(userNetSessionID);

        if (roomUser == null)
        {
            return (false, room, null);
        }

        return (true, room, roomUser);
    }



    void RequestRoomEnter(MemoryPackBinaryRequestInfo packetData)
    {
        var sessionID = packetData.SessionID;
        Logger.Debug("RequestRoomEnter");

        try
        {
            var user = _userMgr.GetUser(sessionID);

            if (user == null || user.IsSessionIDMatch(sessionID) == false)
            {
                ResponseEnterRoomToClient(ErrorCode.ROOM_ENTER_INVALID_USER, sessionID);
                return;
            }

            if (user.IsStateRoom())
            {
                ResponseEnterRoomToClient(ErrorCode.ROOM_ENTER_INVALID_STATE, sessionID);
                return;
            }

            var reqData = MemoryPackSerializer.Deserialize<PKTReqRoomEnter>(packetData.Data);

            var room = GetRoom(reqData.RoomNumber);

            if (room == null)
            {
                ResponseEnterRoomToClient(ErrorCode.ROOM_ENTER_INVALID_ROOM_NUMBER, sessionID);
                return;
            }

            if (room.AddUser(user.ID(), sessionID) == false)
            {
                ResponseEnterRoomToClient(ErrorCode.ROOM_ENTER_FAIL_ADD_USER, sessionID);
                return;
            }


            user.StoreRoomNumber(reqData.RoomNumber);

            room.NotifyPacketUserList(sessionID);
            room.NofifyPacketNewUser(sessionID, user.ID());

            ResponseEnterRoomToClient(ErrorCode.NONE, sessionID);

            Logger.Debug("RequestEnterInternal - Success");
        }
        catch (Exception ex)
        {
            Logger.Error(ex.ToString());
        }
    }

    void ResponseEnterRoomToClient(ErrorCode errorCode, string sessionID)
    {
        var resRoomEnter = new PKTResRoomEnter()
        {
            Result = (short)errorCode
        };

        var sendPacket = MemoryPackSerializer.Serialize(resRoomEnter);
        MemoryPackPacketHeaderInfo.Write(sendPacket, PACKETID.RES_ROOM_ENTER);

        NetSendFunc(sessionID, sendPacket);
    }

    void RequestLeave(MemoryPackBinaryRequestInfo packetData)
    {
        var sessionID = packetData.SessionID;
        Logger.Debug("방나가기 요청 받음");

        try
        {
            var user = _userMgr.GetUser(sessionID);
            if (user == null)
            {
                return;
            }

            if (LeaveRoomUser(sessionID, user.RoomNumber) == false)
            {
                return;
            }

            user.DiscardRoomNumber();

            ResponseLeaveRoomToClient(sessionID);

            Logger.Debug("Room RequestLeave - Success");
        }
        catch (Exception ex)
        {
            Logger.Error(ex.ToString());
        }
    }

    bool LeaveRoomUser(string sessionID, int roomNumber)
    {
        Logger.Debug($"LeaveRoomUser. SessionID:{sessionID}");

        var room = GetRoom(roomNumber);
        if (room == null)
        {
            return false;
        }

        var roomUser = room.GetUserByNetSessionId(sessionID);
        if (roomUser == null)
        {
            return false;
        }

        var userID = roomUser.UserID;
        room.RemoveUser(roomUser);

        room.NotifyPacketLeaveUser(userID);

        if (room.GameStartTime != DateTime.MinValue)    // 게임 중에 나감
        {
            RoomUser aloneUser = room.GetOtherRoomUser(userID);
            if (aloneUser != null)
            {
                room.NotifyGameEnd(aloneUser.UserID);
            }
        }
        return true;
    }

    void ResponseLeaveRoomToClient(string sessionID)
    {
        var resRoomLeave = new PKTResRoomLeave()
        {
            Result = (short)ErrorCode.NONE
        };

        var sendPacket = MemoryPackSerializer.Serialize(resRoomLeave);
        MemoryPackPacketHeaderInfo.Write(sendPacket, PACKETID.RES_ROOM_LEAVE);

        NetSendFunc(sessionID, sendPacket);
    }

    void NotifyLeaveInternal(MemoryPackBinaryRequestInfo packetData)
    {
        var sessionID = packetData.SessionID;
        Logger.Debug($"NotifyLeaveInternal. SessionID: {sessionID}");

        var reqData = MemoryPackSerializer.Deserialize<PKTInternalNtfRoomLeave>(packetData.Data);
        LeaveRoomUser(sessionID, reqData.RoomNumber);
    }

    void RequestChat(MemoryPackBinaryRequestInfo packetData)
    {
        var sessionID = packetData.SessionID;
        Logger.Debug("Room RequestChat");

        try
        {

            (bool check, Room room, RoomUser user) = CheckRoomAndRoomUser(sessionID);
            
            if (check == false)
            {
                return;
            }

            var reqData = MemoryPackSerializer.Deserialize<PKTReqRoomChat>(packetData.Data);

            var notifyPacket = new PKTNtfRoomChat()
            {
                UserID = user.UserID,
                ChatMessage = reqData.ChatMessage
            };

            var sendPacket = MemoryPackSerializer.Serialize(notifyPacket);
            MemoryPackPacketHeaderInfo.Write(sendPacket, PACKETID.NTF_ROOM_CHAT);

            room.Broadcast("", sendPacket);

            Logger.Debug("Room RequestChat - Success");
        }
        catch (Exception ex)
        {
            Logger.Error(ex.ToString());
        }
    }

    void RequestGameReady(MemoryPackBinaryRequestInfo packetData)
    {
        var sessionID = packetData.SessionID;
        Logger.Debug("Request GameReady");

        try
        {
            (bool check, Room room, RoomUser user) = CheckRoomAndRoomUser(sessionID);

            if (check == false)
            {
                return;
            }

            user.IsReady = !user.IsReady;
            if (room.CheckAllUsersReady())
            {
                room.NotifyGameStart();

                Logger.Debug("Room RequestReady and GameStart");
            }
            else
            {
                room.NotifyGameReady(user);
                Logger.Debug("Room RequestReady");
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex.ToString());
        }
    }

    void RequestPutMok(MemoryPackBinaryRequestInfo packetData)
    {
        var sessionID = packetData.SessionID;
        Logger.Debug("Room RequestPutMok");

        try
        {
            (bool check, Room room, RoomUser user) = CheckRoomAndRoomUser(sessionID);

            if (check == false)
            {
                return;
            }

            var reqData = MemoryPackSerializer.Deserialize<PKTReqPutMok>(packetData.Data);
            if (room.ProcessPutMokRequest(reqData.PosX, reqData.PosY, user.UserID) != ErrorCode.NONE) { 
                return; 
            }

            
            var notifyPacket = new PKTNtfPutMok()
            {
                PosX = reqData.PosX,
                PosY = reqData.PosY
            };

            var sendPacket = MemoryPackSerializer.Serialize(notifyPacket);
            MemoryPackPacketHeaderInfo.Write(sendPacket, PACKETID.NTF_PUT_MOK);

            room.Broadcast("", sendPacket);

            Logger.Debug("Room RequestPutMok - Success");

            if (room._omokGame.CheckWinCondition(reqData.PosX, reqData.PosY))
            {
                room.NotifyGameEnd(user.UserID);
                var LoseUser = room.GetOtherRoomUser(user.UserID);
                if (LoseUser == null) { return; }

                var packet = InnerPakcetMaker.MakeReqSaveGameResult(sessionID, user.UserID, LoseUser.UserID);

                DistributeDBRequest(packet);
                Logger.Debug("Game Over");
            }

        }
        catch (Exception ex)
        {
            Logger.Error(ex.ToString());
        }
    }

    void OnTurnTimeOut(MemoryPackBinaryRequestInfo packetData)
    {
        var reqData = MemoryPackSerializer.Deserialize<PKTInternalNtfRoomTimeOut>(packetData.Data);

        int roomNumber = reqData.RoomNumber;
        var room = GetRoom(roomNumber);
        var notifyPacket = new PKTNtfTimeOver();

        var sendPacket = MemoryPackSerializer.Serialize(notifyPacket);
        MemoryPackPacketHeaderInfo.Write(sendPacket, PACKETID.NTF_TIME_OVER);

        room.Broadcast("", sendPacket);

        room.CurTurnPlayerIndex = (room.CurTurnPlayerIndex + 1) % room._maxUserCount;
        room.RecentPutMokTime = DateTime.UtcNow;

        Logger.Debug("Room TurnTimeOver");
        room.TimeOutCount++;
        if (room.TimeOutCount >= 6)
        {
            room.NotifyGameEnd(null);
        }
    }

    void HandleTooLongGameRoom(MemoryPackBinaryRequestInfo packetData)
    {
        var reqData = MemoryPackSerializer.Deserialize<PKTInternalNtfTooLongGameRoom>(packetData.Data);

        int roomNumber = reqData.RoomNumber;
        var room = GetRoom(roomNumber);
        room.NotifyGameEnd(null);
    }


}
