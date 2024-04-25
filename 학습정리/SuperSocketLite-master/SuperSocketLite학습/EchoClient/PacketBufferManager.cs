using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_test_client
{
    class PacketBufferManager
    {
        int BufferSize = 0;
        int ReadPos = 0;
        int WritePos = 0;

        int HeaderSize = 0;
        int MaxPacketSize = 0;
        byte[] PacketData;
        byte[] PacketDataTemp;

        public bool Init(int size, int headerSize, int maxPacketSize)   // size - 버퍼의 크기, headerSize - 헤더의 크기, maxPacketSize - 패킷의 최대 크기
        {
            // 패킷의 손실을 줄이기 위해 버퍼의 크기는 패킷의 최대치보다 두배 이상 크도록 설정한다. (읽고 있는 패킷과 새로 들어오는 패킷을 하나의 버퍼로 처리하기 위해서)
            if (size < (maxPacketSize * 2) || size < 1 || headerSize < 1 || maxPacketSize < 1)
            {
                return false;
            }

            BufferSize = size;
            PacketData = new byte[size];
            PacketDataTemp = new byte[size];
            HeaderSize = headerSize;
            MaxPacketSize = maxPacketSize;

            return true;
        }

        public bool Write(byte[] data, int pos, int size)
        {
            if (data == null || (data.Length < (pos + size)))
            {
                return false;
            }

            var remainBufferSize = BufferSize - WritePos;

            if (remainBufferSize < size)
            {
                return false;
            }

            Buffer.BlockCopy(data, pos, PacketData, WritePos, size);
            WritePos += size;

            if (NextFree() == false)
            {
                BufferRelocate();
            }
            return true;
        }

        public ArraySegment<byte> Read()
        {
            var enableReadSize = WritePos - ReadPos;    // 읽기 가능한 범위

            if (enableReadSize < HeaderSize) // 범위가 패킷의 헤더 크기보다 작으면 - ex) 패킷이 아직 완전히 도착하지 않았을 때
            {
                return new ArraySegment<byte>();
            }

            var packetDataSize = BitConverter.ToInt16(PacketData, ReadPos);
            if (enableReadSize < packetDataSize)
            {
                return new ArraySegment<byte>();
            }

            var completePacketData = new ArraySegment<byte>(PacketData, ReadPos, packetDataSize);
            ReadPos += packetDataSize;
            return completePacketData;
        }

        bool NextFree()
        {
            var enableWriteSize = BufferSize - WritePos;    // 버퍼 최대크기 - 현재까지 쓴 위치 = 이제 쓸 수 있는 남은 버퍼의 크기

            if (enableWriteSize < MaxPacketSize)    // 그 크기가 패킷 하나보다 작으면 못써먹음
            {
                return false;
            }

            return true;
        }

        void BufferRelocate()
        {
            var enableReadSize = WritePos - ReadPos;

            Buffer.BlockCopy(PacketData, ReadPos, PacketDataTemp, 0, enableReadSize);
            Buffer.BlockCopy(PacketDataTemp, 0, PacketData, 0, enableReadSize);

            ReadPos = 0;
            WritePos = enableReadSize;
        }
    }
}
