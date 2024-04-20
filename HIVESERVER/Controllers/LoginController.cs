using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HIVESERVER.Repository;
using HIVESERVER.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        (ErrorCode errorCode, long accountId) = await _accountDb.VerifyAccountAsync(request.Email, request.Password);
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }


        string authToken = Security.CreateAuthToken();
        errorCode = await _memoryDb.RegisterUserAsync(request.Email, authToken, accountId);   // 인증 정보를 Redis에 저장
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }

        _logger.ZLogInformation($"[Login] email:{request.Email}, AuthToken:{authToken}, AccountId:{accountId}");

        response.AuthToken = authToken;
        return response;
    }

    [HttpGet]
    public async Task<ErrorCode> VerifyLoginTokenAsync(string Email, string AuthToken) 
    {
        (bool succeed, UserAuthData userAuthData) = await _memoryDb.GetUserAsync(Email);   // redis에 로그인 정보가 존재하는지 확인
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
    [MinLength(1, ErrorMessage = "EMAIL CANNOT BE EMPTY")]
    [StringLength(50, ErrorMessage = "EMAIL IS TOO LONG")]
    [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
    public string? Email { get; set; }

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