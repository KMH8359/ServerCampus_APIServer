using System;
using System.Data;
using System.Threading.Tasks;
using GAMESERVER.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using ZLogger;

namespace GAMESERVER.Repository;

public class AccountDb : IAccountDb
{
    private readonly IOptions<DbConfig> _dbConfig; // appsettings.json으로부터 db 연결 설정 정보를 받아 저장함
    private readonly ILogger<AccountDb> _logger;
    private IDbConnection _dbConn;  // 실제 DB와의 연결을 관리
    private readonly SqlKata.Compilers.MySqlCompiler _compiler;
    private readonly QueryFactory _queryFactory;        

    public AccountDb(ILogger<AccountDb> logger, IOptions<DbConfig> dbConfig)
    {
        _dbConfig = dbConfig;
        _logger = logger;

        Open();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new QueryFactory(_dbConn, _compiler);
    }

    public void Dispose() // 리소스 해제 함수
    {
        Close();
    }

    public async Task<ErrorCode> CreateAccountAsync(string email, string pw)
    {
        try
        {
            string saltValue = Security.SaltString();
            string hashingPassword = Security.MakeHashingPassWord(saltValue, pw);
            _logger.ZLogDebug( $"[CreateAccount] Email: {email}, SaltValue : {saltValue}, HashingPassword:{hashingPassword}"); // 디버깅 목적 로깅

            int count = await _queryFactory.Query("account").InsertAsync(new
            {
                Email = email,
                SaltValue = saltValue,
                HashedPassword = hashingPassword
            });

            return count != 1 ? ErrorCode.CreateAccountFailInsert : ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[CreateAccount] ErrorCode: {ErrorCode.CreateAccountFailException}, Email: {email}, Password: {pw}");
            return ErrorCode.CreateAccountFailException;
        }
    }

    public async Task<Tuple<ErrorCode, long>> VerifyAuthTokenAsync(string email, string pw)
    {
        try
        {
            UserAccountInfo userInfo = await _queryFactory.Query("user")
                                    .Where("Email", email)
                                    .FirstOrDefaultAsync<UserAccountInfo>();

            if (userInfo.UserId == 0)
            {
                return new Tuple<ErrorCode, long>(ErrorCode.LoginFailUserNotExist, 0);
            }

            string hashingPassword = Security.MakeHashingPassWord(userInfo.SaltValue, pw);
            if (userInfo.HashedPassword != hashingPassword)
            {
                _logger.ZLogError($"[AccountDb.HiveServerLogin] ErrorCode: {ErrorCode.LoginFailPwNotMatch}, Email: {email}, Password: {pw}");
                return new Tuple<ErrorCode, long>(ErrorCode.LoginFailPwNotMatch, 0);
            }

            return new Tuple<ErrorCode, long>(ErrorCode.None, userInfo.UserId);
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"[AccountDb.HiveServerLogin] ErrorCode: {ErrorCode.LoginFailException}, Email: {email}, Password: {pw}");
            return new Tuple<ErrorCode, long>(ErrorCode.LoginFailException, 0);
        }
    }

    private void Open()
    {
        _dbConn = new MySqlConnection(_dbConfig.Value.MySqlGame);

        _dbConn.Open();
    }

    private void Close() 
    {
        _dbConn.Close();
    }
}

public class DbConfig
{
    public string? MySqlGame { get; set; }
    public string? Redis { get; set; }
}


public class UserAccountInfo
{
    public long UserId { get; set; }
    public string? Email { get; set; }
    public string? HashedPassword { get; set; }
    public string? SaltValue { get; set; }
}