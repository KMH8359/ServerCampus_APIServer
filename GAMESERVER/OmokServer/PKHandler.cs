using System;


namespace PvPGameServer;

public class PKHandler
{
    public static Func<string, byte[], bool> NetSendFunc;
    public static Action<MemoryPackBinaryRequestInfo> DistributeInnerPacket;
    public static Action<MemoryPackBinaryRequestInfo> DistributeDBRequest;

    protected UserManager _userMgr = null;


    public void Init(UserManager userMgr)
    {
        this._userMgr = userMgr;
    }           
            
    
}
