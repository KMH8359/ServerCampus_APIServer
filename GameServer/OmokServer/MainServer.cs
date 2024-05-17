using System;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SuperSocket.SocketBase.Logging;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketBase.Config;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using MemoryPack;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Ocsp;


namespace PvPGameServer;

public class MainServer : AppServer<NetworkSession, MemoryPackBinaryRequestInfo>, IHostedService
{
    public static ILog MainLogger;
            
    PacketProcessor _packetProcessor = new PacketProcessor();
    RoomManager _roomMgr = new RoomManager();

    ServerOption _serverOpt;
    IServerConfig _networkConfig;

    private readonly IHostApplicationLifetime _appLifetime;
    private readonly ILogger<MainServer> _appLogger;

    private System.Timers.Timer HeartbeatTimer;
    private int heartbeatGroupIndex = 0;


    public MainServer(IHostApplicationLifetime appLifetime, IOptions<ServerOption> serverConfig, ILogger<MainServer> logger)
        : base(new DefaultReceiveFilterFactory<ReceiveFilter, MemoryPackBinaryRequestInfo>())
    {
        _serverOpt = serverConfig.Value;
        _appLogger = logger;
        _appLifetime = appLifetime;

        NewSessionConnected += new SessionHandler<NetworkSession>(OnConnected);
        SessionClosed += new SessionHandler<NetworkSession, CloseReason>(OnClosed);
        NewRequestReceived += new RequestHandler<NetworkSession, MemoryPackBinaryRequestInfo>(OnPacketReceived);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _appLifetime.ApplicationStarted.Register(AppOnStarted);
        _appLifetime.ApplicationStopped.Register(AppOnStopped);
                    
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void AppOnStarted()
    {
        _appLogger.LogInformation("OnStarted");

        InitConfig(_serverOpt);
        
        CreateServer(_serverOpt);

        var IsResult = base.Start();

        if (IsResult)
        {
            _appLogger.LogInformation("서버 네트워크 시작");
        }
        else
        {
            _appLogger.LogError("서버 네트워크 시작 실패");
            return;
        }
    }

    private void AppOnStopped()
    {
        MainLogger.Info("OnStopped - begin");

        base.Stop();

        _packetProcessor.Destroy();

        MainLogger.Info("OnStopped - end");
    }
            
    public void InitConfig(ServerOption option)
    {
        _networkConfig = new ServerConfig
        {
            Port = option.Port,
            Ip = "Any",
            MaxConnectionNumber = option.MaxConnectionNumber,
            MaxRequestLength = option.MaxRequestLength,
            ReceiveBufferSize = option.ReceiveBufferSize,
            SendBufferSize = option.SendBufferSize,
            Mode = SocketMode.Tcp,
            Name = option.Name
        };
    }

    public void CreateServer(ServerOption serverOpt)
    {
        try
        {
            bool bResult = Setup(new RootConfig(), _networkConfig, logFactory: new NLogLogFactory());

            if (bResult == false)
            {
                MainLogger.Error("[ERROR] 서버 네트워크 설정 실패 ㅠㅠ");
                return;
            }
            else
            {
                MainLogger = base.Logger;
            }

            CreateComponent(serverOpt);
            SetHeartbeatTimer();
            MainLogger.Info("서버 생성 성공");
        }
        catch(Exception ex)
        {
            MainLogger.Error($"[ERROR] 서버 생성 실패: {ex.ToString()}");
        }
    }

    public bool IsRunning(ServerState eCurState)
    {
        if (eCurState == ServerState.Running)
        {
            return true;
        }

        return false;
    }

    public void StopServer()
    {
        Stop();

        _packetProcessor.Destroy();
    }

    public ErrorCode CreateComponent(ServerOption serverOpt)
    {
        Room.NetSendFunc = this.SendData;
        _roomMgr.CreateRooms(serverOpt);

        _packetProcessor = new PacketProcessor();
        _packetProcessor.NetSendFunc = this.SendData;
        _packetProcessor.GetSessionGroupFunc = this.GetSessionsByGroupIndex;

        _packetProcessor.CreateAndStart(_roomMgr, serverOpt, MainLogger);

        _roomMgr.SetPacketProcessor(_packetProcessor);

        MainLogger.Info("CreateComponent - Success");
        return ErrorCode.NONE;
    }

    public bool SendData(string sessionID, byte[] sendData)
    {
        var session = GetSessionByID(sessionID);

        try
        {
            if (session == null)
            {
                return false;
            }

            session.Send(sendData, 0, sendData.Length);
        }
        catch (Exception ex)
        {
            MainLogger.Error($"{ex.ToString()},  {ex.StackTrace}");

            session.SendEndWhenSendingTimeOut();
            session.Close();
        }
        return true;
    }

    public void Distribute(MemoryPackBinaryRequestInfo requestPacket)
    {
        _packetProcessor.InsertPacket(requestPacket);
    }

    void OnConnected(NetworkSession session)
    {
        MainLogger.Info($"세션 번호 {session.SessionID} 접속");
        session._lastResponseTime = DateTime.UtcNow;
    }

    void OnClosed(NetworkSession session, CloseReason reason)
    {
        MainLogger.Info($"세션 번호 {session.SessionID} 접속해제: {reason.ToString()}");

        var user = _packetProcessor._userMgr.GetUser(session.SessionID);
        _packetProcessor.registClientInfoToRemove(user.ID());
        var packet = InnerPakcetMaker.MakeNTFInConnectOrDisConnectClientPacket(false, session.SessionID);
        Distribute(packet);
    }

    void OnPacketReceived(NetworkSession session, MemoryPackBinaryRequestInfo reqInfo)
    {
        // MainLogger.Debug($"세션 번호 {session.SessionID} 받은 데이터 크기: {reqInfo.Body.Length}, ThreadId: {Thread.CurrentThread.ManagedThreadId}");

        reqInfo.SessionID = session.SessionID;
        session._lastResponseTime = DateTime.UtcNow;
        Distribute(reqInfo);         
    }

    void SetHeartbeatTimer()
    {
        int count = _serverOpt.HeartbeatGroupCount;
        HeartbeatTimer = new System.Timers.Timer(_serverOpt.HeartbeatInterval / _serverOpt.HeartbeatGroupCount);
        HeartbeatTimer.Elapsed += (sender, e) => SendHeartBeatMessage(sender, e, heartbeatGroupIndex);
        HeartbeatTimer.AutoReset = true;
        HeartbeatTimer.Start();
    }

    void SendHeartBeatMessage(object sender, ElapsedEventArgs e, int groupIndex)
    {
        var packet = InnerPakcetMaker.MakeHeartbeatRequest(groupIndex);
        Distribute(packet);

        heartbeatGroupIndex = (heartbeatGroupIndex + 1) % _serverOpt.HeartbeatGroupCount;
    }

    public IEnumerable<NetworkSession> GetSessionsByGroupIndex(int groupIndex)
    {
        var sessions = GetAllSessions();

        var startIndex = groupIndex * _serverOpt.HeartbeatGroupSize;
        var endIndex = startIndex + _serverOpt.HeartbeatGroupSize;

        return sessions.Skip(startIndex).Take(endIndex - startIndex);
    }
}


public class NetworkSession : AppSession<NetworkSession, MemoryPackBinaryRequestInfo>
{
    public DateTime _lastResponseTime { get; set; }
}
