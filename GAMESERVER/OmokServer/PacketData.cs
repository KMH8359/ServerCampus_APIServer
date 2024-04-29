using MemoryPack;
using System;
using System.Collections.Generic;


namespace PvPGameServer;

public struct MemoryPackPacketHeadInfo
{
    const int PacketHeaderMemoryPackStartPos = 1;
    public const int HeadSize = 6;

    public UInt16 TotalSize;
    public UInt16 Id;
    public byte Type;

    public static UInt16 GetTotalSize(byte[] data, int startPos)
    {
        return FastBinaryRead.UInt16(data, startPos + PacketHeaderMemoryPackStartPos);
    }

    public static void WritePacketId(byte[] data, UInt16 packetId)
    {
        FastBinaryWrite.UInt16(data, PacketHeaderMemoryPackStartPos + 2, packetId);
    }

    public void Read(byte[] headerData)
    {
        var pos = PacketHeaderMemoryPackStartPos;

        TotalSize = FastBinaryRead.UInt16(headerData, pos);
        pos += 2;

        Id = FastBinaryRead.UInt16(headerData, pos);
        pos += 2;

        Type = headerData[pos];
        pos += 1;
    }
        
    public static void Write(byte[] packetData, PACKETID packetId, byte type = 0)
    {
        var pos = PacketHeaderMemoryPackStartPos;

        FastBinaryWrite.UInt16(packetData, pos, (UInt16)packetData.Length);
        pos += 2;

        FastBinaryWrite.UInt16(packetData, pos, (UInt16)packetId);
        pos += 2;

        packetData[pos] = type;
    }

    public void DebugConsolOutHeaderInfo()
    {
        Console.WriteLine("DebugConsolOutHeaderInfo");
        Console.WriteLine("TotalSize : " + TotalSize);
        Console.WriteLine("Id : " + Id);
        Console.WriteLine("Type : " + Type);
    }
}



[MemoryPackable]
public partial class MemoryPackPacketHead
{
    public UInt16 TotalSize { get; set; } = 0;
    public UInt16 Id { get; set; } = 0;
    public byte Type { get; set; } = 0;
}

[MemoryPackable]
public partial class PKTResponse : MemoryPackPacketHead
{
    public short Result { get; set; }
}



// 로그인 요청
[MemoryPackable]
public partial class PKTReqLogin : MemoryPackPacketHead
{
    public string UserID { get; set; }
    public string AuthToken { get; set; }
}

[MemoryPackable]
public partial class PKTResLogin : PKTResponse
{
}

[MemoryPackable]
public partial class PKTHeartBeat : MemoryPackPacketHead
{
}

[MemoryPackable]
public partial class PKTNtfMustClose : PKTResponse
{
}



[MemoryPackable]
public partial class PKTReqRoomEnter : MemoryPackPacketHead
{
    public int RoomNumber { get; set; }
}

[MemoryPackable]
public partial class PKTResRoomEnter : PKTResponse
{
}

[MemoryPackable]
public partial class PKTNtfRoomUserList : MemoryPackPacketHead
{
    public List<string> UserIDList { get; set; } = new List<string>();
}

[MemoryPackable]
public partial class PKTNtfRoomNewUser : MemoryPackPacketHead
{
    public string UserID { get; set; }
}


[MemoryPackable]
public partial class PKTReqRoomLeave : MemoryPackPacketHead
{
}

[MemoryPackable]
public partial class PKTResRoomLeave : PKTResponse
{
}

[MemoryPackable]
public partial class PKTNtfRoomLeaveUser : MemoryPackPacketHead
{
    public string UserID { get; set; }
}


[MemoryPackable]
public partial class PKTReqRoomChat : MemoryPackPacketHead
{
    public string ChatMessage { get; set; }
}


[MemoryPackable]
public partial class PKTNtfRoomChat : MemoryPackPacketHead
{
    public string UserID { get; set; }

    public string ChatMessage { get; set; }
}

[MemoryPackable]
public partial class PKTInternalReqRoomEnter : MemoryPackPacketHead
{
    public int RoomNumber;

    public string UserID;
}



// 오목 플레이 준비 완료 요청
[MemoryPackable]
public partial class PKTReqReadyOmok : MemoryPackPacketHead
{
}

[MemoryPackable]
public partial class PKTResReadyOmok : PKTResponse
{
}

[MemoryPackable]
public partial class PKTNtfReadyOmok : MemoryPackPacketHead
{
    public string UserID;
    public bool IsReady;
}

[MemoryPackable]
public partial class PKTNtfStartOmok : MemoryPackPacketHead
{
    public string BlackMokUserID;
}

// 돌 두기
[MemoryPackable]
public partial class PKTReqPutMok : MemoryPackPacketHead
{
    public int PosX;
    public int PosY;
}

[MemoryPackable]
public partial class PKTResPutMok : PKTResponse
{
}

[MemoryPackable]
public partial class PKTNtfPutMok : MemoryPackPacketHead
{
    public int PosX;
    public int PosY;
}

[MemoryPackable]
public partial class PKTNtfTimeOver : MemoryPackPacketHead
{
   
}

[MemoryPackable]
public partial class PKTNtfEndOmok : MemoryPackPacketHead
{
    public string WinUserID;
}