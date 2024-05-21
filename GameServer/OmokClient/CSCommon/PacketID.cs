using System;
using System.Collections.Generic;
using System.Text;

namespace CSCommon
{


    // 1001 ~ 2000
    public enum PACKETID : int
    {
         BEGIN = 1001,

         REQ_LOGIN = 1002,
         RES_LOGIN = 1003,

         HEART_BEAT = 1004,
         NFT_MUST_CLOSE = 1005,
         NTF_HEART_BEAT = 1006,

         REQ_ROOM_ENTER = 1015,
         RES_ROOM_ENTER = 1016,
         NTF_ROOM_USER_LIST = 1017,
         NTF_ROOM_NEW_USER = 1018,

         REQ_ROOM_LEAVE = 1021,
         RES_ROOM_LEAVE = 1022,
         NTF_ROOM_LEAVE_USER = 1023,

         REQ_ROOM_CHAT = 1026,
         NTF_ROOM_CHAT = 1027,
         RES_ROOM_CHAT = 1028,

         REQ_READY_OMOK = 1031,
         RES_READY_OMOK = 1032,
         NTF_READY_OMOK = 1033,

         NTF_START_OMOK = 1034,

         REQ_PUT_MOK = 1035,
         RES_PUT_MOK = 1036,
         NTF_PUT_MOK = 1037,

         NTF_TIME_OVER = 1038,
         NTF_IN_TIME_OVER = 1039,
         NTF_IN_TOO_LONG_GAME = 1040,

         NTF_END_MOK = 1041,


         END = 1100,
    }
}
