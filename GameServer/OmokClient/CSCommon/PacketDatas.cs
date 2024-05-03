using MemoryPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSCommon
{
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
    public partial class PKTResRoomChat : PKTResponse
    {
    }

    [MemoryPackable]
    public partial class PKTNtfRoomChat : MemoryPackPacketHead
    {
        public string UserID;

        public string ChatMessage;
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


    // 오목 시작 통보(서버에서 클라이언트들에게)
    [MemoryPackable]
    public partial class PKTNtfStartOmok : MemoryPackPacketHead
    {
        public string FirstUserID; // 선턴 유저 ID
    }


    // 돌 두기
    [MemoryPackable]
    public partial class PKTReqPutMok : MemoryPackPacketHead
    {
        public int PosX;
        public int PosY;        
    }

    [MemoryPackable]
    public partial class PKTResPutMok    : PKTResponse
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

    // 오목 게임 종료 통보
    [MemoryPackable]
    public partial class PKTNtfEndOmok : MemoryPackPacketHead
    {
        public string WinUserID;
    }

   
}
