using System.ComponentModel.DataAnnotations;
using HIVESERVER.Repository;
using HIVESERVER.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ZLogger;

namespace HIVESERVER.Controllers;

[ApiController]
[Route("[controller]")]
public class Login : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IAccountDb _accountDb;
    private readonly IMemoryDb _memoryDb;

    public Login(ILogger<Login> logger, IAccountDb accountDb, IMemoryDb memoryDb)
    {
        _logger = logger;
        _accountDb = accountDb;
        _memoryDb = memoryDb;
    }

    [HttpPost]
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var response = new LoginResponse();
        // ID, PW 검증
        (ErrorCode errorCode, long accountId) = await _accountDb.LoginAsync(request.Id, request.Password);
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            _logger.ZLogError($"[Login] ErrorCode:{ErrorCode.LoginFailPwNotMatch} id:{request.Id}, AccountId:{accountId}");
            return response;
        }


        string authToken = Security.CreateAuthToken();
        errorCode = await _memoryDb.RegisterUserAsync(request.Id, authToken, accountId);   // 인증 정보를 Redis에 저장
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            _logger.ZLogError($"[Login] ErrorCode:{ErrorCode.LoginFailAddRedis} id:{request.Id}, AuthToken:{authToken}, AccountId:{accountId}");
            return response;
        }

        _logger.ZLogInformation($"[Login] id:{request.Id}, AuthToken:{authToken}, AccountId:{accountId}");

        response.AuthToken = authToken;
        return response;
    }

    [HttpGet]
    public async Task<ErrorCode> VerifyLoginTokenAsync(string Id, string AuthToken) 
    {
        (bool succeed, UserAuthData userAuthData) = await _memoryDb.GetUserAsync(Id);   // redis에 로그인 정보가 존재하는지 확인
        if (succeed == false)
        {
           return ErrorCode.RedisFailException;
        }
        else if (userAuthData == null || userAuthData.AuthToken != AuthToken)
        {
            return ErrorCode.AuthTokenNotFound;
        }
        return ErrorCode.None;      
    }
}


public class LoginRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "ID CANNOT BE EMPTY")]
    [StringLength(10, ErrorMessage = "ID IS TOO LONG")]
    public string? Id { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "PASSWORD CANNOT BE EMPTY")]
    [StringLength(30, ErrorMessage = "PASSWORD IS TOO LONG")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}

public class LoginResponse
{
    public ErrorCode Result { get; set; }
    public string? AuthToken { get; set; }
}

class DBUserInfo
{
    public long AccountId { get; set; }
    public string? Email { get; set; }
    public string? HashedPassword { get; set; }
    public string? SaltValue { get; set; }
}

class DBUserGameData
{
    public string? AccountId { get; set; }
}