using CloudStructures.Structures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SqlKata.Execution;
using ZLogger;

namespace HIVESERVER.Controllers;

[ApiController]
[Route("[controller]")]
public class Authentication : ControllerBase
{
    private readonly ILogger Logger;

    public Authentication(ILogger<Authentication> logger)
    {
        Logger = logger;
    }

    [HttpPost]
    public async Task<AuthResponse> AuthenticateAsync(AuthRequest request)
    {
        var response = new AuthResponse();
        response.Result = ErrorCode.None;

        var redisId = new RedisString<string>(DBManager.RedisConn, request.Email, null);
        var loginToken = await redisId.GetAsync();
        
        if (loginToken.GetValueOrDefault() != request.LoginToken) 
        {
            response.Result = ErrorCode.Authenticate_Fail_NotToken;
        }

        Logger.ZLogInformation($"[Request Authentication] Email:{request.Email}, Token:{request.LoginToken}");
        return response;
    }
}


public class AuthRequest
{
    public string? Email { get; set; }
    public string? LoginToken { get; set; }
}

public class AuthResponse
{
    public ErrorCode Result { get; set; }

}

