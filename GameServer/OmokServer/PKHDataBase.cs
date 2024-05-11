using MemoryPack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using StackExchange.Redis;
using CloudStructures;
using CloudStructures.Structures;
using MySqlConnector;
using SqlKata.Execution;
using System.Threading.Tasks;

namespace PvPGameServer;

public class PKHDataBase : PKHandler, IDisposable
{
    private readonly RedisConnection _redisConnection;
    private readonly MySqlConnection _mysqlConnection;
    private readonly string redisConnectionString = "127.0.0.1:6379";
    private readonly string mySQLConnectionString = "Server=127.0.0.1;user=root;Password=123qwe;Database=game_db;Pooling=true;Min Pool Size=0;Max Pool Size=40;AllowUserVariables=True;";
    private readonly SqlKata.Compilers.MySqlCompiler _compiler;
    private readonly QueryFactory _queryFactory;

    public PKHDataBase()
    {
        RedisConfig config = new("default", redisConnectionString);
        _redisConnection = new RedisConnection(config);
        _mysqlConnection = new MySqlConnection(mySQLConnectionString);
        _mysqlConnection.Open();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new QueryFactory(_mysqlConnection, _compiler);
    }

    public void Dispose()
    {
        _mysqlConnection.Close();
    }

    public void RegistPacketHandler(Dictionary<int, Action<MemoryPackBinaryRequestInfo>> dbRequestHandlerMap)
    {
        dbRequestHandlerMap.Add((int)PACKETID.REQ_LOGIN, HandleLoginRequest);
        dbRequestHandlerMap.Add((int)PACKETID.REQ_DB_SAVE_GAMERESULT, HandleSaveGameResultRequest);

    }

    public void HandleLoginRequest(MemoryPackBinaryRequestInfo requestData)
    {
        try
        {
            var reqData = MemoryPackSerializer.Deserialize<PKTReqLogin>(requestData.Data);
            var userID = reqData.UserID;
            var authToken = reqData.AuthToken;

            var errorCode = VerifyLogin(userID, authToken);
            errorCode.Wait();

            
            ResponseVerify(errorCode.Result, userID, requestData.SessionID);
        }
        catch (Exception ex)
        {
            Logger.Error(ex.ToString());
        }
    }

    private async Task<ErrorCode> VerifyLogin(string userID, string authToken)
    {
        ErrorCode result = ErrorCode.NONE;
        try
        {
            RedisString<string> redis = new(_redisConnection, userID, null);
            RedisResult<string> user = await redis.GetAsync();

            if (!user.HasValue)
            {
                return ErrorCode.DB_LOGIN_EMPTY_USER;
            }
            else if (user.Value != authToken)
            {
                return ErrorCode.LOGIN_INVALID_AUTHTOKEN;
            }
        }
        catch (Exception ex)
        {
            Logger.Error("Error while verifying login: " + ex.ToString());
            return ErrorCode.DB_LOGIN_EXCEPTION;
        }

        return result;

    }

    void ResponseVerify(ErrorCode errorCode, string userID, string sessionID)
    {
        var packet = InnerPakcetMaker.MakeResVerifyLoginRequest(sessionID, userID, errorCode);

        DistributeInnerPacket(packet);
    }

    public void HandleSaveGameResultRequest(MemoryPackBinaryRequestInfo requestData)
    {
        try
        {
            var reqData = MemoryPackSerializer.Deserialize<PKTReqDBSaveResult>(requestData.Data);
            var WinuserID = reqData.WinUserID;
            var LoseUserID = reqData.LoseUserID;

            _queryFactory.Query("usergamedata").Where("UserId", WinuserID).Increment("Win", 1);

            _queryFactory.Query("usergamedata").Where("UserId", LoseUserID).Increment("Lose", 1);

        }
        catch (Exception ex)
        {
            Logger.Error(ex.ToString());
        }
    }



}

public class UserAuthData
{
    public string UserId { get; set; } = "";
    public string AuthToken { get; set; } = "";
    public long AccountId { get; set; } = 0;
    public string State { get; set; } = "";
}