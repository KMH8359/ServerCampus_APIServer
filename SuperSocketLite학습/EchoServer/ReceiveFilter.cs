using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SuperSocket.Common;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketEngine.Protocol;

namespace EchoServer
{
    public class EFBinaryRequestInfo : BinaryRequestInfo
    {
        // 패킷 헤더용 변수
        public Int16 TotalSize { get; private set; }
        public Int16 PacketID { get; private set; }
        public SByte Value1 { get; private set; }

        public const int HEADERE_SIZE = 5;


        public EFBinaryRequestInfo(Int16 totalSize, Int16 packetID, SByte value1, byte[] body)
            : base(null, body)
        {
            this.TotalSize = totalSize;
            this.PacketID = packetID;
            this.Value1 = value1;
        }
    }

    public class ReceiveFilter : FixedHeaderReceiveFilter<EFBinaryRequestInfo>
    {
        public ReceiveFilter() : base(EFBinaryRequestInfo.HEADERE_SIZE)
        {
        }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)           // 패킷을 수신하면 자동으로 호출되는 함수 - Body 길이 리턴
        {
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(header, offset, 2);

            var packetTotalSize = BitConverter.ToInt16(header, offset);     // 패킷의 전체 길이 계산
            return packetTotalSize - EFBinaryRequestInfo.HEADERE_SIZE;  // 거기에서 헤더 길이만큼 빼면 바디 길이가 나옴
        }

        protected override EFBinaryRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)   // 패킷을 수신하면 자동으로 호출되는 함수
        {
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(header.Array, 0, EFBinaryRequestInfo.HEADERE_SIZE);

            return new EFBinaryRequestInfo(BitConverter.ToInt16(header.Array, 0),   // 전체 크기
                                           BitConverter.ToInt16(header.Array, 0 + 2),       // ID
                                           (SByte)header.Array[4],                              // Value1
                                           bodyBuffer.CloneRange(offset, length));          // Body 시작점
        }
    }
}
