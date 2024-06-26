﻿using Microsoft.Extensions.Options;
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

    System.Threading.Thread _eraseWorker = null;

    RedisConnection _redisConnection;

    RedisList<string> _redisList_clientInfo;  // 매칭 서버에서 대전 서버로 보낼 정보들을 저장하는 RedisList
    RedisList<CompleteMatchingData> _redisList_gameRoom;  // 대전 서버로부터 플레이어들이 진입할 방에 대한 정보들을 받아와 저장하는 RedisList
    RedisList<string> _redisList_clientInfoToRemove;  // 매칭서버에서 제거할(게임을 다 플레이하고 원상태로 돌아온) 플레이어들의 정보를 저장하는 Redis

    public MatchWoker(IOptions<MatchingConfig> matchingConfig)
    {
        Console.WriteLine("MatchWoker 생성자 호출");

        RedisConfig config = new("default", matchingConfig.Value.UserRedisAddress);
        _redisConnection = new RedisConnection(config);

        _redisList_clientInfo = new RedisList<string>(_redisConnection, "ClientInfo", TimeSpan.FromMinutes(30));
        _redisList_gameRoom = new RedisList<CompleteMatchingData>(_redisConnection, "MatchingData", TimeSpan.FromMinutes(60));
        _redisList_clientInfoToRemove = new RedisList<string>(_redisConnection, "ClientInfo_Remove", TimeSpan.FromMinutes(30));

        _reqWorker = new Thread(this.RunMatching);
        _reqWorker.Start();

        _completeWorker = new Thread(this.RunMatchingComplete);
        _completeWorker.Start();

        _eraseWorker = new Thread(this.RunErase);
        _eraseWorker.Start();
    }
    
    public ErrorCode AddUser(string userID)
    {
        try
        {
            if (_reqQueue.Contains<string> (userID) || _completeDic.ContainsKey(userID))
            {
                return ErrorCode.MatchingInProgress;
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

                _reqQueue.TryDequeue(out var user1);
                _reqQueue.TryDequeue(out var user2);
                
                Console.WriteLine("_redisList_clientInfo에 매칭된 플레이어 정보 저장");
                var task = _redisList_clientInfo.RightPushAsync(user1);
                task.Wait();
                task = _redisList_clientInfo.RightPushAsync(user2);
                task.Wait();

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

                Console.WriteLine($"정보 저장 - address : {user1.ServerAddress}, roomNumber : {user1.RoomNumber}");
            }
            catch (Exception ex)
            {

            }
        }        
    }

    void RunErase()
    {
        while (true)
        {
            try
            {
                if (_redisList_clientInfoToRemove.LengthAsync().Result == 0)
                {
                    Thread.Sleep(1);
                    continue;
                }

                var task = _redisList_clientInfoToRemove.LeftPopAsync();
                task.Wait();
                var UserID = task.Result.Value;

                EraseUserID(UserID);

            }
            catch (Exception ex)
            {

            }
        }
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
    public int PortNumber { get; set; }
    public string myId { get; set; }
    
}


public class MatchingConfig
{
    public string UserRedisAddress { get; set; }
    public string PubKey { get; set; }
    public string SubKey { get; set; }
}