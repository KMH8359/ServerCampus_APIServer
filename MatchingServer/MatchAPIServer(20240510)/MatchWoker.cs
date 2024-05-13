using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using CloudStructures;
using CloudStructures.Structures;
using StackExchange.Redis;
using System.Linq;


namespace APIServer;

public interface IMatchWoker : IDisposable
{
    public ErrorCode AddUser(string userID);

    public (bool, CompleteMatchingData) GetCompleteMatching(string userID);
}

public class MatchWoker : IMatchWoker
{
    List<string> _pvpServerAddressList = new();

    System.Threading.Thread _reqWorker = null;
    ConcurrentQueue<string> _reqQueue = new();

    System.Threading.Thread _completeWorker = null;
    ConcurrentDictionary<string, CompleteMatchingData> _completeDic = new();

    RedisConnection _redisConnection;

    RedisList<string> _redisList_clientInfo;  // 매칭 서버에서 대전 서버로 보낼 정보들을 저장하는 RedisList
    RedisList<CompleteMatchingData> _redisList_gameRoom;  // 대전 서버로부터 플레이어들이 진입할 방에 대한 정보들을 받아와 저장하는 RedisList


    public MatchWoker(IOptions<MatchingConfig> matchingConfig)
    {
        Console.WriteLine("MatchWoker 생성자 호출");

        RedisConfig config = new("default", matchingConfig.Value.UserRedisAddress);
        _redisConnection = new RedisConnection(config);

        _redisList_clientInfo = new RedisList<string>(_redisConnection, "ClientInfo", TimeSpan.FromMinutes(30));
        _redisList_gameRoom = new RedisList<CompleteMatchingData>(_redisConnection, "MatchingData", TimeSpan.FromMinutes(60));

        _reqWorker = new Thread(this.RunMatching);
        _reqWorker.Start();

        _completeWorker = new Thread(this.RunMatchingComplete);
        _completeWorker.Start();
    }
    
    public ErrorCode AddUser(string userID)
    {
        try
        {
            if (_reqQueue.Contains<string> (userID) || _completeDic.ContainsKey(userID))
            {
                return ErrorCode.MatchingReqFail;
            }
            _reqQueue.Enqueue(userID);
            return ErrorCode.None;
        }
        catch (Exception ex)
        {
            return ErrorCode.MatchingReqFail;
        }
    }

    public (bool, CompleteMatchingData) GetCompleteMatching(string userID)
    {
        if (_completeDic.TryGetValue(userID, out var result))
        {
            return (true, result);
        }
        return (false, null);
    }

    public void EraseUserID(string userID)
    {
        _completeDic.TryRemove(userID, out var result);
    }

    void RunMatching()
    {
        while (true)
        {
            try
            {
                if (_reqQueue.Count < 2)
                {
                    Thread.Sleep(1);
                    continue;
                }

                if (_reqQueue.TryDequeue(out var user1) && _reqQueue.TryDequeue(out var user2))
                {
                    var task = _redisList_clientInfo.RightPushAsync(user1);
                    task.Wait();
                    task = _redisList_clientInfo.RightPushAsync(user2);
                    task.Wait();
                }

            }
            catch (Exception ex)
            {

            }
        }
    }

    void RunMatchingComplete()
    {
        while (true)
        {
            try
            {
                if (_redisList_gameRoom.LengthAsync().Result < 2)
                {
                    Thread.Sleep(1);
                    continue;
                }

                //TODO: 매칭 결과를 _completeDic에 넣는다
                // 2명이 하므로 각각 유저를 대상으로 총 2개를 _completeDic에 넣어야 한다

                var user1_task = _redisList_gameRoom.LeftPopAsync();
                user1_task.Wait();
                var user1 = user1_task.Result.Value;

                var user2_task = _redisList_gameRoom.LeftPopAsync();
                user2_task.Wait();
                var user2 = user2_task.Result.Value;

                _completeDic.TryAdd(user1.myId, user1);
                _completeDic.TryAdd(user2.myId, user2);
            }
            catch (Exception ex)
            {

            }
        }        
    }


    string GetGameServerAddress()
    {
        return "127.0.0.1";
    }

    int GetGameRoomNumber()
    {
        return 0;
    }




    public void Dispose()
    {
        Console.WriteLine("MatchWoker 소멸자 호출");
    }
}


public class CompleteMatchingData
{    
    public string ServerAddress { get; set; }
    public int RoomNumber { get; set; }
    public string myId { get; set; }
    public string enemyId { get; set; }
}


public class MatchingConfig
{
    public string UserRedisAddress { get; set; }
    public string MatchingDataRedisAddress { get; set; }
    public string PubKey { get; set; }
    public string SubKey { get; set; }
}