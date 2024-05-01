using MemoryPack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using MySql.Data.MySqlClient;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace PvPGameServer;

public class PKHDataBase : PKHandler
{
    private readonly ConnectionMultiplexer _redis;
    private readonly string redisConnectionString = "localhost:6379";

    public PKHDataBase()
    {
        _redis = ConnectionMultiplexer.Connect(redisConnectionString);
    }

    public void RegistPacketHandler(Dictionary<int, Action<MemoryPackBinaryRequestInfo>> dbRequestHandlerMap)
    {
        dbRequestHandlerMap.Add((int)PACKETID.REQ_LOGIN, HandleLoginRequest);

    }

    public void HandleLoginRequest(MemoryPackBinaryRequestInfo requestData)
    {
        try
        {
            var reqData = MemoryPackSerializer.Deserialize<PKTReqLogin>(requestData.Data);
            var userID = reqData.UserID;
            var authToken = reqData.AuthToken;

            var errorCode = VerifyLogin(userID, authToken);

            ResponseVerify(errorCode, userID, requestData.SessionID);
        }
        catch (Exception ex)
        {
            MainServer.MainLogger.Error(ex.ToString());
        }
    }

    private ErrorCode VerifyLogin(string userID, string authToken)
    {
        try
        {
            IDatabase db = _redis.GetDatabase();

            RedisValue storedAuthToken = db.StringGet(userID);

            if (storedAuthToken.HasValue && storedAuthToken.ToString() == authToken)
            {
                return ErrorCode.NONE;
            }
            else
            {
                return ErrorCode.DB_LOGIN_INVALID_PASSWORD;
            }
        }
        catch (Exception ex)
        {
            MainServer.MainLogger.Error("Error while verifying login: " + ex.ToString());
            return ErrorCode.DB_LOGIN_EXCEPTION;
        }
    }

    void ResponseVerify(ErrorCode errorCode, string userID, string sessionID)
    {
        var packet = InnerPakcetMaker.MakeResVerifyLoginRequest(sessionID, userID, errorCode);

        DistributeInnerPacket(packet);
    }



}