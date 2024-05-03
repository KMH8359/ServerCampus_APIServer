using MemoryPack;
using System;
using System.Collections.Generic;



namespace PvPGameServer;

public class PKHCommon : PKHandler
{
    public void RegistPacketHandler(Dictionary<int, Action<MemoryPackBinaryRequestInfo>> packetHandlerMap)
    {
        packetHandlerMap.Add((int)PACKETID.NTF_IN_CONNECT_CLIENT, NotifyInConnectClient);
        packetHandlerMap.Add((int)PACKETID.NTF_IN_DISCONNECT_CLIENT, NotifyInDisConnectClient);

        packetHandlerMap.Add((int)PACKETID.NTF_HEART_BEAT, BroadcastHeartbeat);
        packetHandlerMap.Add((int)PACKETID.REQ_LOGIN, RequestLogin);
        packetHandlerMap.Add((int)PACKETID.RES_DB_LOGIN, ResponseVerifyLogin);

    }

    public void NotifyInConnectClient(MemoryPackBinaryRequestInfo requestData)
    {
    }

    public void NotifyInDisConnectClient(MemoryPackBinaryRequestInfo requestData)
    {
        var sessionID = requestData.SessionID;
        var user = _userMgr.GetUser(sessionID);
        
        if (user != null)
        {
            var roomNum = user.RoomNumber;

            if (roomNum != Room.InvalidRoomNumber)
            {
                var internalPacket = InnerPakcetMaker.MakeNTFInnerRoomLeavePacket(sessionID, roomNum, user.ID());                
                DistributeInnerPacket(internalPacket);
            }

            _userMgr.RemoveUser(sessionID);
        }
    }

    public void BroadcastHeartbeat(MemoryPackBinaryRequestInfo packetData)
    {
        var sessionID = packetData.SessionID;
        var Data = MemoryPackSerializer.Deserialize<PKTInternalNtfHeartbeat>(packetData.Data);

        int groupIndex = Data.GroupIndex;
        var notifyPacket = new PKTHeartBeat();

        var sendPacket = MemoryPackSerializer.Serialize(notifyPacket);
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.HEART_BEAT);
        foreach (var session in GetSessionGroupFunc(groupIndex))
        {
            if ((DateTime.UtcNow - session._lastResponseTime).TotalMilliseconds > sessionTimeoutLimit)
            {
                Logger.Debug($"세션 번호 {session.SessionID} 장시간 응답 없음으로 연결 종료");
                session.Close();
                continue;
            }
            NetSendFunc(session.SessionID, sendPacket);
        }
    }

    public void RequestLogin(MemoryPackBinaryRequestInfo packetData)
    {
        var sessionID = packetData.SessionID;
        Logger.Debug("로그인 요청 받음");

        try
        {
            if(_userMgr.GetUser(sessionID) != null)
            {
                ResponseLoginToClient(ErrorCode.LOGIN_ALREADY_WORKING, packetData.SessionID);
                return;
            }
            
            DistributeDBRequest(packetData);

            Logger.Debug($"로그인 인증 요청");

        }
        catch(Exception ex)
        {
            Logger.Error(ex.ToString());
        }
    }

    public void ResponseVerifyLogin(MemoryPackBinaryRequestInfo packetData)
    {
        var sessionID = packetData.SessionID;

        try
        {
            var reqData = MemoryPackSerializer.Deserialize<PKTResDBLogin>(packetData.Data);

            var verifyResult = (ErrorCode)reqData.Result;

            if (verifyResult != ErrorCode.NONE)
            {
                ResponseLoginToClient(verifyResult, packetData.SessionID);
                return;
            }

            var errorCode = _userMgr.AddUser(reqData.UserID, sessionID);
            if (errorCode != ErrorCode.NONE)
            {
                ResponseLoginToClient(errorCode, packetData.SessionID);

                if (errorCode == ErrorCode.LOGIN_FULL_USER_COUNT)
                {
                    NotifyMustCloseToClient(ErrorCode.LOGIN_FULL_USER_COUNT, packetData.SessionID);
                }

                return;
            }

            ResponseLoginToClient(errorCode, packetData.SessionID);

            Logger.Debug("로그인 인증 완료");

        }
        catch (Exception ex)
        {
            Logger.Error(ex.ToString());
        }
    }


    public void ResponseLoginToClient(ErrorCode errorCode, string sessionID)
    {
        var resLogin = new PKTResLogin()
        {
            Result = (short)errorCode
        };

        var sendData = MemoryPackSerializer.Serialize(resLogin);
        MemoryPackPacketHeadInfo.Write(sendData, PACKETID.RES_LOGIN);

        NetSendFunc(sessionID, sendData);
    }

    public void NotifyMustCloseToClient(ErrorCode errorCode, string sessionID)
    {
        var resLogin = new PKTNtfMustClose()
        {
            Result = (short)errorCode
        };

        var sendData = MemoryPackSerializer.Serialize(resLogin);
        MemoryPackPacketHeadInfo.Write(sendData, PACKETID.NTF_MUST_CLOSE);

        NetSendFunc(sessionID, sendData);
    }


    
                  
}
