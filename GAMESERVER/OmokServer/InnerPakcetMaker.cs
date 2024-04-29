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

}
   

[MemoryPackable]
public partial class PKTInternalNtfRoomLeave : MemoryPackPacketHead
{
    public int RoomNumber { get; set; }
    public string UserID { get; set; }
}
