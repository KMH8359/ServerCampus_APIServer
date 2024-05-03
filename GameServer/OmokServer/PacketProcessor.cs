using System;
using System.Collections.Generic;
using System.Linq;
using SuperSocket.SocketBase.Logging;
using System.Threading.Tasks.Dataflow;


namespace PvPGameServer;

class PacketProcessor
{
    bool _isThreadRunning = false;
    System.Threading.Thread _processThread = null;
    System.Threading.Thread _dbThread = null;

    public Func<string, byte[], bool> NetSendFunc;
    public Func<int, IEnumerable<NetworkSession>> GetSessionGroupFunc;
    
    // BufferBlock is Thread Safe
    BufferBlock<MemoryPackBinaryRequestInfo> _msgBuffer = new BufferBlock<MemoryPackBinaryRequestInfo>();
    BufferBlock<MemoryPackBinaryRequestInfo> _dbmsgBuffer = new BufferBlock<MemoryPackBinaryRequestInfo>();

    UserManager _userMgr = new UserManager();
    List<Room> _roomList = new List<Room>();

    Dictionary<int, Action<MemoryPackBinaryRequestInfo>> _packetHandlerMap = new Dictionary<int, Action<MemoryPackBinaryRequestInfo>>();
    Dictionary<int, Action<MemoryPackBinaryRequestInfo>> _dbRequestHandlerMap = new Dictionary<int, Action<MemoryPackBinaryRequestInfo>>();

    PKHCommon _commonPacketHandler = new PKHCommon();
    PKHRoom _roomPacketHandler = new PKHRoom();
    PKHDataBase _databasePacketHandler = new PKHDataBase();

    private ILog _logger;

    public void CreateAndStart(List<Room> roomList, ServerOption serverOpt, ILog logger)
    {
        var maxUserCount = serverOpt.RoomMaxCount * serverOpt.RoomMaxUserCount;
        _userMgr.Init(maxUserCount);

        _roomList = roomList;
        var minRoomNum = _roomList[0].Number;
        var maxRoomNum = _roomList[0].Number + _roomList.Count() - 1;

        _logger = logger;

        RegistPacketHandler();
        _commonPacketHandler.sessionTimeoutLimit = serverOpt.HeartbeatTimeOut;

        _isThreadRunning = true;
        _processThread = new System.Threading.Thread(this.Process); 
        _processThread.Start();
        _dbThread = new System.Threading.Thread(this.DB_Process);
        _dbThread.Start();

    }
    
    public void Destroy()
    {
        _logger.Info("PacketProcessor::Destory - begin");

        _isThreadRunning = false;
        _msgBuffer.Complete();
        _dbmsgBuffer.Complete();    
        _processThread.Join();
        _dbThread.Join();
        _logger.Info("PacketProcessor::Destory - end");
    }
          
    public void InsertPacket(MemoryPackBinaryRequestInfo data)
    {
        _msgBuffer.Post(data);
    }

    public void InsertDBRequest(MemoryPackBinaryRequestInfo data)
    {
        _dbmsgBuffer.Post(data);
    }

    void RegistPacketHandler()
    {
        PKHandler.NetSendFunc = NetSendFunc;
        PKHandler.GetSessionGroupFunc = GetSessionGroupFunc;    
        PKHandler.DistributeInnerPacket = InsertPacket;
        PKHandler.DistributeDBRequest = InsertDBRequest;
        PKHandler.Logger = _logger;

        _commonPacketHandler.Init(_userMgr);
        _commonPacketHandler.RegistPacketHandler(_packetHandlerMap);    

        _roomPacketHandler.Init(_userMgr);
        _roomPacketHandler.SetRoomList(_roomList);
        _roomPacketHandler.RegistPacketHandler(_packetHandlerMap);

        _databasePacketHandler.Init(_userMgr);
        _databasePacketHandler.RegistPacketHandler(_dbRequestHandlerMap);
    }

    void Process()
    {
        while (_isThreadRunning)
        {
            try
            {
                var packet = _msgBuffer.Receive();

                var header = new MemoryPackPacketHeadInfo();
                header.Read(packet.Data);

                if (_packetHandlerMap.ContainsKey(header.Id))
                {
                    _packetHandlerMap[header.Id](packet);
                }
            }
            catch (Exception ex)
            {
                if (_isThreadRunning)
                {
                    _logger.Error(ex.ToString());
                }
            }
        }
    }

    void DB_Process()
    {
        while (_isThreadRunning)
        {
            try
            {
                var packet = _dbmsgBuffer.Receive();

                var header = new MemoryPackPacketHeadInfo();
                header.Read(packet.Data);

                if (_dbRequestHandlerMap.ContainsKey(header.Id))
                {
                    _dbRequestHandlerMap[header.Id](packet);
                }
            }
            catch (Exception ex)
            {
                if (_isThreadRunning)
                {
                    _logger.Error(ex.ToString());
                }
            }
        }
    }


}
