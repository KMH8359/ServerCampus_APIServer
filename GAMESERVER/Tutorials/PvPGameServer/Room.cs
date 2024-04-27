﻿using MemoryPack;
using System;
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

    public enum 돌종류 { 없음, 흑돌, 백돌 };

    const int 바둑판크기 = 19;


    int[,] 바둑판 = new int[바둑판크기, 바둑판크기];
    public bool 게임종료 { get; private set; } = true;
    public int CurTurnPlayerIndex { get; private set; } = 0;

    public string _BlackMokUserID { get; private set; } = null;
    public string _WhiteMokUserID { get; private set; } = null;



    public void Init(int index, int number, int maxUserCount)
    {
        Index = index;
        Number = number;
        _maxUserCount = maxUserCount;
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

    public (string, string) DetermineMokAssignment()
    {
        Random rnd = new Random();
        CurTurnPlayerIndex = rnd.Next(_maxUserCount);

        return (_userList[CurTurnPlayerIndex].UserID, _userList[(CurTurnPlayerIndex + 1) % 2].UserID);
    }

    public void EndGame()
    {
        게임종료 = true;
    }

    public void NotifyGameStart()
    {
        Array.Clear(바둑판, 0, 바둑판크기 * 바둑판크기);

        게임종료 = false;
        (_BlackMokUserID, _WhiteMokUserID) = DetermineMokAssignment();
        var notifyPacket = new PKTNtfStartOmok()
        {
            BlackMokUserID = _BlackMokUserID
        };

        var sendPacket = MemoryPackSerializer.Serialize(notifyPacket);
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.NTF_START_OMOK);

        Broadcast("", sendPacket);

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
        if (바둑판[x, y] != 0) { return ErrorCode.OMOK_ALREADY_EXIST; }
        if (_userList[CurTurnPlayerIndex].UserID != UserID) { return ErrorCode.OMOK_TURN_NOT_MATCH; }

        if (UserID == _BlackMokUserID)
        {   
            바둑판[x, y] = (int)돌종류.흑돌;
        }
        else if (UserID == _WhiteMokUserID)
        {
            바둑판[x, y] = (int)돌종류.백돌;
        }
        CurTurnPlayerIndex = (CurTurnPlayerIndex + 1) % 2; 
        return ErrorCode.NONE;
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
