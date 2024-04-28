using System;
using System.Collections.Generic;
using System.Linq;


namespace PvPGameServer;

public class UserManager
{
    int _maxUserCount;
    UInt64 _userSequenceNumber = 0;

    Dictionary<string, User> _userMap = new Dictionary<string, User>();


    public void Init(int maxUserCount)
    {
        _maxUserCount = maxUserCount;
    }

    public ErrorCode AddUser(string userID, string sessionID)
    {
        if(IsFullUserCount())
        {
            return ErrorCode.LOGIN_FULL_USER_COUNT;
        }


        foreach (var userEntry in _userMap)
        {
            if (userEntry.Value.ID() == userID)
            {
                return ErrorCode.ADD_USER_DUPLICATION;
            }
        }


        ++_userSequenceNumber;
        
        var user = new User();
        user.Set(_userSequenceNumber, sessionID, userID);
        _userMap.Add(sessionID, user);

        return ErrorCode.NONE;
    }

    public ErrorCode RemoveUser(string sessionID)
    {
        if(_userMap.Remove(sessionID) == false)
        {
            return ErrorCode.REMOVE_USER_SEARCH_FAILURE_USER_ID;
        }

        return ErrorCode.NONE;
    }

    public User GetUser(string sessionID)
    {
        User user = null;
        _userMap.TryGetValue(sessionID, out user);
        return user;
    }

    bool IsFullUserCount()
    {
        return _maxUserCount <= _userMap.Count();
     }
            
}

public class User
{
    UInt64 SequenceNumber = 0;
    string SessionID;
   
    public int RoomNumber { get; private set; } = -1;
    string UserID;
            
    public void Set(UInt64 sequence, string sessionID, string userID)
    {
        SequenceNumber = sequence;
        SessionID = sessionID;
        UserID = userID;
    }                   
    
    public bool IsSessionIDMatch(string netSessionID)
    {
        return SessionID == netSessionID;
    }

    public string ID()
    {
        return UserID;
    }

    public void StoreRoomNumber(int roomNumber)
    {
        RoomNumber = roomNumber;
    }

    public void DiscardRoomNumber()
    {
        RoomNumber = -1;
    }

    public bool IsStateLogin() { return SequenceNumber != 0; }

    public bool IsStateRoom() { return RoomNumber != -1; }
}

