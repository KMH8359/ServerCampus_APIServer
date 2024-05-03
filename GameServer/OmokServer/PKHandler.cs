using SuperSocket.SocketBase.Logging;
using System;
using System.Collections.Generic;


namespace PvPGameServer;

public class PKHandler
{
    public static Func<string, byte[], bool> NetSendFunc;
    public static Action<MemoryPackBinaryRequestInfo> DistributeInnerPacket;
    public static Action<MemoryPackBinaryRequestInfo> DistributeDBRequest;
    public static Func<int, IEnumerable<NetworkSession>> GetSessionGroupFunc;
    public static ILog Logger;
    public int sessionTimeoutLimit = 0;
    protected UserManager _userMgr = null;


    public void Init(UserManager userMgr)
    {
        this._userMgr = userMgr;
    }           
            
    
}
