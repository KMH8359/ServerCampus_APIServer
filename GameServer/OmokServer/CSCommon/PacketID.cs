
using System;

namespace CSCommon
{
    // 1 ~ 10000
    public enum PACKETID : int
    {
        REQ_RES_TEST_ECHO = 101,


        // 클라이언트
        BEGIN = 1001,

        REQ_LOGIN = 1002,
        RES_LOGIN = 1003,
        HEART_BEAT = 1004,
        NTF_MUST_CLOSE = 1005,
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

        REQ_ROOM_DEV_ALL_ROOM_START_GAME = 1091,
        RES_ROOM_DEV_ALL_ROOM_START_GAME = 1092,

        REQ_ROOM_DEV_ALL_ROOM_END_GAME = 1093,
        RES_ROOM_DEV_ALL_ROOM_END_GAME = 1094,

        END = 1100,


        // 시스템, 서버 - 서버
        SS_START = 8001,

        NTF_IN_CONNECT_CLIENT = 8011,
        NTF_IN_DISCONNECT_CLIENT = 8012,

        REQ_SS_SERVERINFO = 8021,
        RES_SS_SERVERINFO = 8023,

        REQ_IN_ROOM_ENTER = 8031,
        RES_IN_ROOM_ENTER = 8032,

        NTF_IN_ROOM_LEAVE = 8036,


        // DB 8101 ~ 9000
        REQ_DB_LOGIN = 8101,
        RES_DB_LOGIN = 8102,
        REQ_DB_SAVE_GAMERESULT = 8103,
    }



}