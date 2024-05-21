using MemoryPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSCommon
{
    public struct MemoryPackPacketHeaderInfo
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
                
        public static void WritePacketId(byte[] data, UInt16 packetID)
        {
            FastBinaryWrite.UInt16(data, PacketHeaderMemoryPackStartPos + 2, packetID);
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

        public void Write(byte[] packetData)
        {
            var pos = PacketHeaderMemoryPackStartPos;

            FastBinaryWrite.UInt16(packetData, pos, TotalSize);
            pos += 2;

            FastBinaryWrite.UInt16(packetData, pos, Id);
            pos += 2;

            packetData[pos] = Type;
            pos += 1;
        }

        public byte[] Write()
        {
            var packetData = new byte[HeadSize];
            var pos = PacketHeaderMemoryPackStartPos;

            FastBinaryWrite.UInt16(packetData, pos, TotalSize);
            pos += 2;

            FastBinaryWrite.UInt16(packetData, pos, Id);
            pos += 2;

            packetData[pos] = Type;
            pos += 1;

            return packetData;
        }

        public static void Write(byte[] packetData, PACKETID packetId, byte type = 0)
        {
            var pos = PacketHeaderMemoryPackStartPos;

            FastBinaryWrite.UInt16(packetData, pos, (UInt16)packetData.Length);
            pos += 2;

            FastBinaryWrite.UInt16(packetData, pos, (UInt16)packetId);
            pos += 2;

            packetData[pos] = type;
            pos += 1;
        }
    }


    [MemoryPackable]
    public partial class MemoryPackPacketHead
    {
        public UInt16 TotalSize { get; set; } = 0;
        public UInt16 Id { get; set; } = 0;
        public byte Type { get; set; } = 0;
    }
}
