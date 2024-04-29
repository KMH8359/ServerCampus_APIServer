using System;
using System.Collections.Generic;
using System.Text;

namespace CSCommon
{
    // 1001 ~ 2000
    public class PacketID
    {
        public const UInt16 BEGIN = 1001;

        public const UInt16 REQ_LOGIN = 1002;
        public const UInt16 RES_LOGIN = 1003;

        public const UInt16 HEART_BEAT = 1004;
        public const UInt16 NFT_MUST_CLOSE = 1005;

        public const UInt16 REQ_ROOM_ENTER = 1015;
        public const UInt16 RES_ROOM_ENTER = 1016;
        public const UInt16 NTF_ROOM_USER_LIST = 1017;
        public const UInt16 NTF_ROOM_NEW_USER = 1018;

        public const UInt16 REQ_ROOM_LEAVE = 1021;
        public const UInt16 RES_ROOM_LEAVE = 1022;
        public const UInt16 NTF_ROOM_LEAVE_USER = 1023;

        public const UInt16 REQ_ROOM_CHAT = 1026;
        public const UInt16 NTF_ROOM_CHAT = 1027;
        public const UInt16 RES_ROOM_CHAT = 1028;

        public const UInt16 REQ_READY_OMOK = 1031;
        public const UInt16 RES_READY_OMOK = 1032;
        public const UInt16 NTF_READY_OMOK = 1033;

        public const UInt16 NTF_START_OMOK = 1034;

        public const UInt16 REQ_PUT_MOK = 1035;
        public const UInt16 RES_PUT_MOK = 1036;
        public const UInt16 NTF_PUT_MOK = 1037;

        public const UInt16 NTF_TIME_OVER = 1038;

        public const UInt16 NTF_END_MOK = 1041;


        public const UInt16 END = 1100;
    }
}
