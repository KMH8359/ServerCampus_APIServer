using System.Data;
using HIVESERVER.Services;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using ZLogger;

namespace HIVESERVER.Repository;

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
            string id = email.Split('@')[0];
            string saltValue = Security.SaltString();
            string hashingPassword = Security.MakeHashingPassWord(saltValue, pw);
            _logger.ZLogDebug( $"[CreateAccount] Email: {email}, UserId: {id}, SaltValue : {saltValue}, HashingPassword:{hashingPassword}"); // 디버깅 목적 로깅

            int count = await _queryFactory.Query("account").InsertAsync(new
            {
                Email = email,
                UserId = id,
                SaltValue = saltValue,
                HashedPassword = hashingPassword
            });

            if (count != 1) 
            {
                _logger.ZLogError(
                    $"[CreateAccount] ErrorCode: {ErrorCode.CreateAccountFailInsert}, Email: {email}, Password: {pw}");
                return ErrorCode.CreateAccountFailInsert;
            }
            
            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[CreateAccount] ErrorCode: {ErrorCode.CreateAccountFailException}, Email: {email}, Password: {pw}");
            return ErrorCode.CreateAccountFailException;
        }
    }

    public async Task<Tuple<ErrorCode, long>> LoginAsync(string id, string pw)
    {
        try
        {
            UserAccountInfo userInfo = await _queryFactory.Query("account")
                                    .Where("UserId", id)
                                    .FirstOrDefaultAsync<UserAccountInfo>();
            _logger.ZLogDebug( $"[Login] Id: {id}, Password: {pw}"); 
            if (userInfo.AccountId == 0)
            {
                return new Tuple<ErrorCode, long>(ErrorCode.LoginFailUserNotExist, 0);
            }

            string hashingPassword = Security.MakeHashingPassWord(userInfo.SaltValue, pw);
            if (userInfo.HashedPassword != hashingPassword)
            {
                _logger.ZLogError($"[Login] ErrorCode: {ErrorCode.LoginFailPwNotMatch}, Id: {id}, Password: {pw}");
                return new Tuple<ErrorCode, long>(ErrorCode.LoginFailPwNotMatch, 0);
            }

            return new Tuple<ErrorCode, long>(ErrorCode.None, userInfo.AccountId);
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"[Login] ErrorCode: {ErrorCode.LoginFailException}, Id: {id}, Password: {pw}");
            return new Tuple<ErrorCode, long>(ErrorCode.LoginFailException, 0);
        }
    }

    private void Open()
    {
        _dbConn = new MySqlConnection(_dbConfig.Value.MySqlHive);

        _dbConn.Open();
    }

    private void Close() 
    {
        _dbConn.Close();
    }
}

public class DbConfig
{
    public string? MySqlHive { get; set; }
    public string? Redis { get; set; }
}


public class UserAccountInfo
{
    public long AccountId { get; set; }
    public string? Email { get; set; }
    public string? HashedPassword { get; set; }
    public string? SaltValue { get; set; }
}