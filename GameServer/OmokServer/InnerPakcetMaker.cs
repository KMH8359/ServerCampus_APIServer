using MemoryPack;
using System;


namespace PvPGameServer;

public class InnerPakcetMaker   // 서버 혼자 쓰는 패킷 메이커
{
    public static MemoryPackBinaryRequestInfo MakeNTFInnerRoomLeavePacket(string sessionID, int roomNumber, string userID)
    {            
        var packet = new PKTInternalNtfRoomLeave()
        {
            RoomNumber = roomNumber,
            UserID = userID,
        };

        var sendData = MemoryPackSerializer.Serialize(packet);
        MemoryPackPacketHeadInfo.Write(sendData, PACKETID.NTF_IN_ROOM_LEAVE);
        
        var roomLeavePacket = new MemoryPackBinaryRequestInfo(null);
        roomLeavePacket.Data = sendData;
        roomLeavePacket.SessionID = sessionID;
        return roomLeavePacket;
    }

    public static MemoryPackBinaryRequestInfo MakeNTFInConnectOrDisConnectClientPacket(bool isConnect, string sessionID)
    {
        var packet = new MemoryPackBinaryRequestInfo(null);
        packet.Data = new byte[MemoryPackPacketHeadInfo.HeadSize];
        
        if (isConnect)
        {
            MemoryPackPacketHeadInfo.WritePacketId(packet.Data, (UInt16)PACKETID.NTF_IN_CONNECT_CLIENT);
        }
        else
        {
            MemoryPackPacketHeadInfo.WritePacketId(packet.Data, (UInt16)PACKETID.NTF_IN_DISCONNECT_CLIENT);
        }

        packet.SessionID = sessionID;
        return packet;
    }

    public static MemoryPackBinaryRequestInfo MakeResVerifyLoginRequest(string sessionID, string userID, ErrorCode errorCode)
    {
        var packet = new PKTResDBLogin()
        {
            UserID = userID,
            Result = (short)errorCode
        };

        var sendData = MemoryPackSerializer.Serialize(packet);
        MemoryPackPacketHeadInfo.Write(sendData, PACKETID.RES_DB_LOGIN);

        var verifyPacket = new MemoryPackBinaryRequestInfo(null);
        verifyPacket.Data = sendData;
        verifyPacket.SessionID = sessionID;
        return verifyPacket;
    }

    public static MemoryPackBinaryRequestInfo MakeReqSaveGameResult(string sessionID, string WinUserID, string LoseuserID)
    {
        var packet = new PKTReqDBSaveResult()
        {
            WinUserID = WinUserID,
            LoseUserID = LoseuserID
        };

        var sendData = MemoryPackSerializer.Serialize(packet);
        MemoryPackPacketHeadInfo.Write(sendData, PACKETID.REQ_DB_SAVE_GAMERESULT);

        var verifyPacket = new MemoryPackBinaryRequestInfo(null);
        verifyPacket.Data = sendData;
        verifyPacket.SessionID = sessionID;
        return verifyPacket;
    }

    public static MemoryPackBinaryRequestInfo MakeNTFInTimeOutPacket(int roomNumber)
    {
        var packet = new PKTInternalNtfRoomTimeOut()
        {
            RoomNumber = roomNumber
        };

        var sendData = MemoryPackSerializer.Serialize(packet);
        MemoryPackPacketHeadInfo.Write(sendData, PACKETID.NTF_IN_TIME_OVER);

        var notifyPacket = new MemoryPackBinaryRequestInfo(null);
        notifyPacket.Data = sendData;
        return notifyPacket;
    }

    public static MemoryPackBinaryRequestInfo MakeNTFInTooLongGameRoomPacket(int roomNumber)
    {
        var packet = new PKTInternalNtfTooLongGameRoom()
        {
            RoomNumber = roomNumber
        };

        var sendData = MemoryPackSerializer.Serialize(packet);
        MemoryPackPacketHeadInfo.Write(sendData, PACKETID.NTF_IN_TOO_LONG_GAME);

        var notifyPacket = new MemoryPackBinaryRequestInfo(null);
        notifyPacket.Data = sendData;
        return notifyPacket;
    }

    public static MemoryPackBinaryRequestInfo MakeHeartbeatRequest(int groupIndex)
    {
        var packet = new PKTInternalNtfHeartbeat()
        {
            GroupIndex = groupIndex
        };

        var sendData = MemoryPackSerializer.Serialize(packet);
        MemoryPackPacketHeadInfo.Write(sendData, PACKETID.NTF_HEART_BEAT);

        var notifyPacket = new MemoryPackBinaryRequestInfo(null);
        notifyPacket.Data = sendData;
        return notifyPacket;
    }

}
   

[MemoryPackable]
public partial class PKTInternalNtfRoomLeave : MemoryPackPacketHead
{
    public int RoomNumber { get; set; }
    public string UserID { get; set; }
}

[MemoryPackable]
public partial class PKTInternalNtfRoomTimeOut : MemoryPackPacketHead
{
    public int RoomNumber { get; set; }
}

[MemoryPackable]
public partial class PKTInternalNtfTooLongGameRoom : MemoryPackPacketHead
{
    public int RoomNumber { get; set; }
}

[MemoryPackable]
public partial class PKTInternalNtfHeartbeat : MemoryPackPacketHead
{
    public int GroupIndex { get; set; }
}