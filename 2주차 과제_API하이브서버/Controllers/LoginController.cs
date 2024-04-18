using System.Text;
using CloudStructures.Structures;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using SqlKata.Execution;
using Newtonsoft.Json;

[ApiController]
[Route("[controller]")]
public class Login : ControllerBase
{
    [HttpPost]
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var response = new LoginResponse();
        response.Result = ErrorCode.None;
        string tokenValue = null;

        using (var db = await DBManager.GetGameDBQuery())
        {
            var userGameData = await db.Query("account").Where("Email", request.Email).FirstOrDefaultAsync<DBUserInfo>();

            if (userGameData == null) {
                response.Result = ErrorCode.Login_Fail_NotUser;
                return response;
            }

            tokenValue = Security.CreateAuthToken();
            var idDefaultExpiry = TimeSpan.FromDays(1);
            var redisId = new RedisString<string>(DBManager.RedisConn, request.Email, idDefaultExpiry);
            var setAuthTokenSucceed = await redisId.SetAsync(tokenValue);
            
            if (setAuthTokenSucceed == false) {
                response.Result = ErrorCode.Login_Fail_Exception;
                return response;
            }
        }

        response.Logintoken = tokenValue;
        return response;
    }
}


public class LoginRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class LoginResponse
{
    public ErrorCode Result { get; set; }
    public string Logintoken { get; set; }
}

class DBUserInfo
{
    public long AccountId { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public string SaltValue { get; set; }
    public string authToken {get; set;}
}

class DBUserGameData
{
    public string AccountId { get; set; }
}