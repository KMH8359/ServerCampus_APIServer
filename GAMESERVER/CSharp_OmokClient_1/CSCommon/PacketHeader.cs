using MemoryPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSCommon
{
    public struct MemoryPackPacketHeaderInfo
    {
        const int PacketHeaderMemoryPackStartPos = 3;
        public const int HeadSize = 8;

        public UInt16 TotalSize;
        public UInt16 ID;
        public byte Type;

        public static UInt16 GetTotalSize(byte[] data, int startPos)
        {
            return FastBinaryRead.UInt16(data, startPos + PacketHeaderMemoryPackStartPos);
        }
                
        public static void Write(byte[] data, UInt16 totalSize, UInt16 packetID)
        {
            // FastBinaryWrite.UInt16(data, PacketHeaderMemoryPackStartPos, totalSize);
            FastBinaryWrite.UInt16(data, PacketHeaderMemoryPackStartPos + 2, packetID);
        }


        public void Read(byte[] headerData)
        {
            var pos = PacketHeaderMemoryPackStartPos;

            TotalSize = FastBinaryRead.UInt16(headerData, pos);
            pos += 2;

            ID = FastBinaryRead.UInt16(headerData, pos);
            pos += 2;

            Type = headerData[pos];
            pos += 1;
        }

        public void Write(byte[] packetData)
        {
            var pos = PacketHeaderMemoryPackStartPos;

            FastBinaryWrite.UInt16(packetData, pos, TotalSize);
            pos += 2;

            FastBinaryWrite.UInt16(packetData, pos, ID);
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

            FastBinaryWrite.UInt16(packetData, pos, ID);
            pos += 2;

            packetData[pos] = Type;
            pos += 1;

            return packetData;
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
