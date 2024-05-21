using MemoryPack;
using System;
using System.Collections.Generic;


namespace CSCommon
{
    [MemoryPackable]
    public partial class PKTReqDBSaveResult : MemoryPackPacketHead
    {
        public string WinUserID;
        public string LoseUserID;
    }

    [MemoryPackable]
    public partial class PKTResDBLogin : PKTResponse
    {
        public string UserID { get; set; }
    }
}
 