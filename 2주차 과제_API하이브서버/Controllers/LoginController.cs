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
        string? tokenValue = null;

        using (var db = await DBManager.GetGameDBQuery())
        {
            var userAccountInfo = await db.Query("account").Where("Email", request.Email).FirstOrDefaultAsync<DBUserInfo>();

            if (userAccountInfo == null || string.IsNullOrEmpty(userAccountInfo.HashedPassword)) {
                response.Result = ErrorCode.Login_Fail_NotUser;
                return response;
            }

            var hashingPassword = Security.MakeHashingPassWord(userAccountInfo.SaltValue, request.Password);

            Console.WriteLine($"[Request Login] Email:{request.Email}, request.Password:{request.Password},  saltValue:{userAccountInfo.SaltValue}, hashingPassword:{hashingPassword}");

            if (userAccountInfo.HashedPassword != hashingPassword)
            {
                response.Result = ErrorCode.Login_Fail_PW;
                return response;
            }
            
            tokenValue = Security.CreateAuthToken();
            var idDefaultExpiry = TimeSpan.FromDays(1);
            var redisId = new RedisString<string>(DBManager.RedisConn, request.Email, idDefaultExpiry);
            var setAuthTokenSucceed = await redisId.SetAsync(tokenValue);
            
            if (setAuthTokenSucceed == false) {
                response.Result = ErrorCode.Login_Fail_Token;
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
}

class DBUserGameData
{
    public string AccountId { get; set; }
}