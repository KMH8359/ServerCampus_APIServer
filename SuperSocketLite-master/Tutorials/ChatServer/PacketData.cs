using MessagePack; //https://github.com/neuecc/MessagePack-CSharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSBaseLib
{
    public class PacketDef
    {
        public const Int16 PACKET_HEADER_SIZE = 5;
        public const int MAX_USER_ID_BYTE_LENGTH = 16;
        public const int MAX_USER_PW_BYTE_LENGTH = 16;

        public const int INVALID_ROOM_NUMBER = -1;
    }

    public class PacketToBytes
    {
        public static byte[] Make(PACKETID packetID, byte[] bodyData)       // body 데이터에 패킷 ID와 전체 크기 등의 데이터를 추가하여 반환
        {
            byte type = 0;
            var pktID = (Int16)packetID;
            Int16 bodyDataSize = 0;
            if (bodyData != null)
            {
                bodyDataSize = (Int16)bodyData.Length;
            }
            var packetSize = (Int16)(bodyDataSize + PacketDef.PACKET_HEADER_SIZE);
                        
            var dataSource = new byte[packetSize];
            Buffer.BlockCopy(BitConverter.GetBytes(packetSize), 0, dataSource, 0, 2);   // 전체 크기 복사 - short니 2바이트만큼
            Buffer.BlockCopy(BitConverter.GetBytes(pktID), 0, dataSource, 2, 2);        //  패킷 ID 복사 - short니 2바이트만큼
            dataSource[4] = type;   // 의미없는 데이터
            
            if (bodyData != null)
            {
                Buffer.BlockCopy(bodyData, 0, dataSource, 5, bodyDataSize); // 패킷 내용물 복사
            }

            return dataSource;
        }

        public static Tuple<int, byte[]> ClientReceiveData(int recvLength, byte[] recvData)     // 패킷을 ID와 Body 데이터로 분해해서 반환
        {
            var packetSize = BitConverter.ToInt16(recvData, 0);
            var packetID = BitConverter.ToInt16(recvData, 2);
            var bodySize = packetSize - PacketDef.PACKET_HEADER_SIZE;

            var packetBody = new byte[bodySize];
            Buffer.BlockCopy(recvData, PacketDef.PACKET_HEADER_SIZE, packetBody,  0, bodySize);

            return new Tuple<int, byte[]>(packetID, packetBody);
        }
    }

    // 로그인 요청
    [MessagePackObject]
    public class PKTReqLogin
    {
        [Key(0)]
        public string UserID;
        [Key(1)]
        public string AuthToken;
    }

    [MessagePackObject]
    public class PKTResLogin
    {
        [Key(0)]
        public short Result;
    }


    [MessagePackObject]
    public class PKNtfMustClose
    {
        [Key(0)]
        public short Result;
    }



    [MessagePackObject]
    public class PKTReqRoomEnter
    {
        [Key(0)]
        public int RoomNumber;
    }

    [MessagePackObject]
    public class PKTResRoomEnter
    {
        [Key(0)]
        public short Result;
    }

    [MessagePackObject]
    public class PKTNtfRoomUserList
    {
        [Key(0)]
        public List<string> UserIDList = new List<string>();
    }

    [MessagePackObject]
    public class PKTNtfRoomNewUser
    {
        [Key(0)]
        public string UserID;
    }


    [MessagePackObject]
    public class PKTReqRoomLeave
    {
    }

    [MessagePackObject]
    public class PKTResRoomLeave
    {
        [Key(0)]
        public short Result;
    }

    [MessagePackObject]
    public class PKTNtfRoomLeaveUser
    {
        [Key(0)]
        public string UserID;
    }


    [MessagePackObject]
    public class PKTReqRoomChat
    {
        [Key(0)]
        public string ChatMessage;
    }

    
    [MessagePackObject]
    public class PKTNtfRoomChat
    {
        [Key(0)]
        public string UserID;

        [Key(1)]
        public string ChatMessage;
    }
}
