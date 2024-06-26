﻿using System;
using System.Collections.Generic;
using System.Linq;
using SuperSocket.SocketBase.Logging;
using System.Threading.Tasks.Dataflow;
using System.Threading;
using CloudStructures;
using CloudStructures.Structures;
using System.Runtime.Intrinsics.X86;
using System.Net;
using System.Net.Sockets;
using CSCommon;

namespace PvPGameServer;

class PacketProcessor
{
    bool _isThreadRunning = false;
    System.Threading.Thread _processThread = null;
    System.Threading.Thread _dbThread = null;
    System.Threading.Thread _matchingThread = null;

    public Func<string, byte[], bool> NetSendFunc;
    public Func<int, IEnumerable<NetworkSession>> GetSessionGroupFunc;
    
    // BufferBlock is Thread Safe
    BufferBlock<MemoryPackBinaryRequestInfo> _msgBuffer = new BufferBlock<MemoryPackBinaryRequestInfo>();
    BufferBlock<MemoryPackBinaryRequestInfo> _dbmsgBuffer = new BufferBlock<MemoryPackBinaryRequestInfo>();

    public UserManager _userMgr = new UserManager();
    List<Room> _roomList = new List<Room>();

    Dictionary<int, Action<MemoryPackBinaryRequestInfo>> _packetHandlerMap = new Dictionary<int, Action<MemoryPackBinaryRequestInfo>>();
    Dictionary<int, Action<MemoryPackBinaryRequestInfo>> _dbRequestHandlerMap = new Dictionary<int, Action<MemoryPackBinaryRequestInfo>>();

    PKHCommon _commonPacketHandler = new PKHCommon();
    PKHRoom _roomPacketHandler = new PKHRoom();
    PKHDataBase _databasePacketHandler = new PKHDataBase();

    private ILog _logger;

    RoomManager _roomMgr;

    RedisConnection _redisConnection;
    RedisList<string> _redisList_clientInfo;  // 매칭 서버에서 대전 서버로 보낼 정보들을 저장하는 RedisList
    RedisList<CompleteMatchingData> _redisList_gameRoom;  // 대전 서버로부터 플레이어들이 진입할 방에 대한 정보들을 받아와 저장하는 RedisList
    RedisList<string> _redisList_clientInfoToRemove;  // 매칭서버에서 제거할(게임을 다 플레이하고 원상태로 돌아온) 플레이어들의 정보를 저장하는 Redis

    string myAddress;
    int myPort;

    public void CreateAndStart(RoomManager roomManager, ServerOption serverOpt, ILog logger)
    {
        var maxUserCount = serverOpt.RoomMaxCount * serverOpt.RoomMaxUserCount;
        _userMgr.Init(maxUserCount);

        _roomMgr = roomManager;
        _roomList = roomManager.GetRoomsList();
        var minRoomNum = _roomList[0].Number;
        var maxRoomNum = _roomList[0].Number + _roomList.Count() - 1;

        _logger = logger;

        RedisConfig config = new("default", serverOpt.UserRedisAddress);
        _redisConnection = new RedisConnection(config);
        _redisList_clientInfo = new RedisList<string>(_redisConnection, "ClientInfo", TimeSpan.FromMinutes(30));
        _redisList_gameRoom = new RedisList<CompleteMatchingData>(_redisConnection, "MatchingData", TimeSpan.FromMinutes(60));
        _redisList_clientInfoToRemove = new RedisList<string>(_redisConnection, "ClientInfo_Remove", TimeSpan.FromMinutes(30));

        RegistPacketHandler();
        _commonPacketHandler.sessionTimeoutLimit = serverOpt.HeartbeatTimeOut;

        _isThreadRunning = true;
        _processThread = new System.Threading.Thread(this.Process); 
        _processThread.Start();
        _dbThread = new System.Threading.Thread(this.DB_Process);
        _dbThread.Start();
        _matchingThread = new System.Threading.Thread(this.Matching_Process);
        _matchingThread.Start();


        myAddress = GetLocalIP(); // 127.0.0.1
        myPort = serverOpt.Port;
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

    public static string GetLocalIP()
    {
        IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());
        foreach (IPAddress ip in addresses)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork) 
            {
                return ip.ToString();
            }
        }
        throw new Exception("IPv4 address not found.");
    }

    void Process()
    {
        while (_isThreadRunning)
        {
            try
            {
                var packet = _msgBuffer.Receive();

                var header = new MemoryPackPacketHeaderInfo();
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

                var header = new MemoryPackPacketHeaderInfo();
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

    void Matching_Process()
    {
        while (_isThreadRunning)
        {
            try
            {
                var length = _redisList_clientInfo.LengthAsync().Result;

                if (length < 2)
                {
                    Thread.Sleep(1);
                    continue;
                }

                var resultTask = _redisList_clientInfo.LeftPopAsync();
                resultTask.Wait(); 
                var player1_id = resultTask.Result.Value;

                resultTask = _redisList_clientInfo.LeftPopAsync();
                resultTask.Wait();
                var player2_id = resultTask.Result.Value;


                var room = _roomMgr.GetValidRoom();
                room.IsReserved = true;

                CompleteMatchingData data = new CompleteMatchingData(myAddress, room.Number, myPort, player1_id);
                var task = _redisList_gameRoom.RightPushAsync(data);
                task.Wait();

                data.myId = player2_id;
                task = _redisList_gameRoom.RightPushAsync(data);
                task.Wait();

                Console.WriteLine("_redisList_gameRoom에 데이터 저장");
                
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

    public void registClientInfoToRemove(string UserID)
    {
        _redisList_clientInfoToRemove.RightPushAsync(UserID);
    }

}

public class CompleteMatchingData
{
    public string ServerAddress { get; set; }
    public int RoomNumber { get; set; }
    public int PortNumber { get; set; }
    public string myId { get; set; }

    public CompleteMatchingData( string address, int roomnum, int portnum, string myid) 
    {
        ServerAddress = address;
        RoomNumber = roomnum;
        PortNumber = portnum;
        myId = myid;
    }
}