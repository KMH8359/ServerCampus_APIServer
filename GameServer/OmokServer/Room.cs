using MemoryPack;
using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;


namespace PvPGameServer;

public class Room
{
    public const int InvalidRoomNumber = -1;


    public int Index { get; private set; }
    public int Number { get; private set; }

    public int _maxUserCount { get; private set; } = 0;

    List<RoomUser> _userList = new List<RoomUser>();

    public static Func<string, byte[], bool> NetSendFunc;


    public int CurTurnPlayerIndex = 0;

    public string _BlackMokUserID { get; private set; } = null;
    public string _WhiteMokUserID { get; private set; } = null;

    public OmokGame _omokGame { get; private set; } = null;

    public int TimeOutCount = 0;

    public DateTime RecentPutMokTime;

    public void Init(int index, int number, int maxUserCount)
    {
        Index = index;
        Number = number;
        _maxUserCount = maxUserCount;
        _omokGame = new OmokGame();
        RecentPutMokTime = DateTime.MinValue;
    }

    public bool AddUser(string userID, string netSessionID)
    {
        if(GetUser(userID) != null)
        {
            return false;
        }

        var roomUser = new RoomUser();
        roomUser.Set(userID, netSessionID);
        _userList.Add(roomUser);

        return true;
    }

    public bool RemoveUser(RoomUser user)
    {
        return _userList.Remove(user);
    }

    public RoomUser GetUser(string userID)
    {
        return _userList.Find(x => x.UserID == userID);
    }

    public RoomUser GetUserByNetSessionId(string netSessionID)
    {
        return _userList.Find(x => x.NetSessionID == netSessionID);
    }

    public int CurrentUserCount()
    {
        return _userList.Count();
    }


    public bool CheckAllUsersReady()
    {
        if (CurrentUserCount() < _maxUserCount) return false;

        foreach (var user in _userList)
        {
            if (user.IsReady == false)
            {   
                return false;
            }
        }
        return true;
    }

    public RoomUser GetOtherRoomUser(string userID)
    {
        foreach (var user in _userList)
        {
            if (user.UserID != userID)
            {
                return user;
            }
        }
        return null;
    }

    public (string, string) DetermineMokAssignment()
    {
        Random rnd = new Random();
        CurTurnPlayerIndex = rnd.Next(_maxUserCount);

        return (_userList[CurTurnPlayerIndex].UserID, _userList[(CurTurnPlayerIndex + 1) % _maxUserCount].UserID);
    }

    public void NotifyGameStart()
    {
        _omokGame.BoardClear();

        (_BlackMokUserID, _WhiteMokUserID) = DetermineMokAssignment();
        TimeOutCount = 0;
        var notifyPacket = new PKTNtfStartOmok()
        {
            BlackMokUserID = _BlackMokUserID
        };

        var sendPacket = MemoryPackSerializer.Serialize(notifyPacket);
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.NTF_START_OMOK);

        Broadcast("", sendPacket);

        RecentPutMokTime = DateTime.UtcNow;
    }

    public void NotifyGameReady(RoomUser user)
    {
        var notifyPacket = new PKTNtfReadyOmok()
        {
            UserID = user.UserID,
            IsReady = user.IsReady
        };

        var sendPacket = MemoryPackSerializer.Serialize(notifyPacket);
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.NTF_READY_OMOK);

        Broadcast("", sendPacket);
    }

    public ErrorCode ProcessPutMokRequest(int x, int y, string UserID)
    {
        if (_omokGame.CheckOmokBoardPosition(x, y) != 0) { return ErrorCode.OMOK_ALREADY_EXIST; }
        if (_userList[CurTurnPlayerIndex].UserID != UserID) { return ErrorCode.OMOK_TURN_NOT_MATCH; }
        if (_omokGame.삼삼확인(x, y)) { return ErrorCode.OMOK_RENJURULE; }

        if (UserID == _BlackMokUserID)
        {
            _omokGame.PutMok(x, y, OmokGame.돌종류.흑돌);
        }
        else if (UserID == _WhiteMokUserID)
        {
            _omokGame.PutMok(x, y, OmokGame.돌종류.백돌);
        }

        CurTurnPlayerIndex = (CurTurnPlayerIndex + 1) % _maxUserCount;

        RecentPutMokTime = DateTime.UtcNow;
        return ErrorCode.NONE;
    }
    public void NotifyGameEnd(string CurTurnUserID)
    {
        var notifyPacket = new PKTNtfEndOmok()
        {
            WinUserID = CurTurnUserID
        };

        var sendPacket = MemoryPackSerializer.Serialize(notifyPacket);
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.NTF_END_MOK);

        
        Broadcast("", sendPacket);
        RecentPutMokTime = DateTime.MinValue;
        foreach (var user in _userList)
        {
            user.IsReady = false;
        }
    }


    public void NotifyPacketUserList(string userNetSessionID)
    {
        var packet = new PKTNtfRoomUserList();
        foreach (var user in _userList)
        {
            packet.UserIDList.Add(user.UserID);
        }

        var sendPacket = MemoryPackSerializer.Serialize(packet);
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.NTF_ROOM_USER_LIST);
        
        NetSendFunc(userNetSessionID, sendPacket);
    }

    public void NofifyPacketNewUser(string newUserNetSessionID, string newUserID)
    {
        var packet = new PKTNtfRoomNewUser();
        packet.UserID = newUserID;
        
        var sendPacket = MemoryPackSerializer.Serialize(packet);
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.NTF_ROOM_NEW_USER);
        
        Broadcast(newUserNetSessionID, sendPacket);
    }

    public void NotifyPacketLeaveUser(string userID)
    {
        if(CurrentUserCount() == 0)
        {
            return;
        }

        var packet = new PKTNtfRoomLeaveUser();
        packet.UserID = userID;
        
        var sendPacket = MemoryPackSerializer.Serialize(packet);
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.NTF_ROOM_LEAVE_USER);
      
        Broadcast("", sendPacket);
    }

    public void Broadcast(string excludeNetSessionID, byte[] sendPacket)
    {
        foreach(var user in _userList)
        {
            if(user.NetSessionID == excludeNetSessionID)
            {
                continue;
            }

            NetSendFunc(user.NetSessionID, sendPacket);
        }
    }


}


public class RoomUser
{
    public string UserID { get; private set; }
    public string NetSessionID { get; private set; }

    public bool IsReady { get; set; }

    public void Set(string userID, string netSessionID)
    {
        UserID = userID;
        NetSessionID = netSessionID;
    }
}

