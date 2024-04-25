using System.Data;
using GAMESERVER.Services;
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

    public async Task<ErrorCode> CreateUserGameDataAsync(String id)
    {
        try
        {
            _logger.ZLogDebug( $"[CreateUserGameData] UserId: {id}");

            int count = await _queryFactory.Query("usergamedata").InsertAsync(new
            {
                UserId = id,
                LEVEL = 1,
                EXP = 0,
                WIN = 0,
                LOSE = 0
            });

            if (count != 1) 
            {
                _logger.ZLogError(
                    $"[CreateUserGameData] ErrorCode: {ErrorCode.CreateCharacterFailInsert}, UserId: {id}");
                return ErrorCode.CreateCharacterFailInsert;
            }
            
            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[CreateUserGameData] ErrorCode: {ErrorCode.CreateCharacterFailException}, UserId: {id}");
            return ErrorCode.CreateCharacterFailException;
        }
    }
    public async Task<bool> GetUserAsync(string id)
    {
        UserAccountInfo userInfo = await _queryFactory.Query("usergamedata")
                                .Where("UserId", id)
                                .FirstOrDefaultAsync<UserAccountInfo>();
        if (userInfo == null)
        {
            return false;
        }
        return true;
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
    public long AccountId { get; set; }
    public string? Email { get; set; }
    public string? HashedPassword { get; set; }
    public string? SaltValue { get; set; }
}