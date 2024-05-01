using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks.Dataflow;


namespace PvPGameServer;

class PacketProcessor
{
    bool _isThreadRunning = false;
    System.Threading.Thread _processThread = null;
    System.Threading.Thread _dbThread = null;

    public Func<string, byte[], bool> NetSendFunc;
    
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
            

    public void CreateAndStart(List<Room> roomList, ServerOption serverOpt)
    {
        var maxUserCount = serverOpt.RoomMaxCount * serverOpt.RoomMaxUserCount;
        _userMgr.Init(maxUserCount);

        _roomList = roomList;
        var minRoomNum = _roomList[0].Number;
        var maxRoomNum = _roomList[0].Number + _roomList.Count() - 1;
        
        RegistPacketHandler();

        _isThreadRunning = true;
        _processThread = new System.Threading.Thread(this.Process); 
        _processThread.Start();
        _dbThread = new System.Threading.Thread(this.DB_Process);
        _dbThread.Start();

    }
    
    public void Destroy()
    {
        MainServer.MainLogger.Info("PacketProcessor::Destory - begin");

        _isThreadRunning = false;
        _msgBuffer.Complete();
        _dbmsgBuffer.Complete();    
        _processThread.Join();
        _dbThread.Join();
        MainServer.MainLogger.Info("PacketProcessor::Destory - end");
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
        PKHandler.DistributeInnerPacket = InsertPacket;
        PKHandler.DistributeDBRequest = InsertDBRequest;

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
                    MainServer.MainLogger.Error(ex.ToString());
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
                    MainServer.MainLogger.Error(ex.ToString());
                }
            }
        }
    }


}
