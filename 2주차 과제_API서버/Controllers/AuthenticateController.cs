using CloudStructures.Structures;
using Microsoft.AspNetCore.Mvc;
using SqlKata.Execution;


[ApiController]
[Route("[controller]")]
public class Authentication : ControllerBase
{
    [HttpPost]
    public async Task<AuthResponse> Post(AuthRequest request)
    {
        var response = new AuthResponse();
        response.Result = ErrorCode.None;

        using (var db = await DBManager.GetGameDBQuery())
        {
            var userInfo = await db.Query("account").Where("Email", request.Email).FirstOrDefaultAsync<DBUserInfo>();

            if (userInfo == null || string.IsNullOrEmpty(userInfo.HashedPassword))
            {
                response.Result = ErrorCode.Login_Fail_NotUser;
                return response;
            }
                        
            var hashingPassword = Security.MakeHashingPassWord(userInfo.SaltValue, request.Password);

            Console.WriteLine($"[Request Login] Email:{request.Email}, request.Password:{request.Password},  saltValue:{userInfo.SaltValue}, hashingPassword:{hashingPassword}");
            if (userInfo.HashedPassword != hashingPassword)
            {
                response.Result = ErrorCode.Login_Fail_PW;
                return response;
            }

            db.Dispose();
        }

        return response;
    }
}


public class AuthRequest
{
    public string? Email { get; set; }
    public string? Authtoken { get; set; }
}

public class AuthResponse
{
    public ErrorCode Result { get; set; }
    public string? Authtoken { get; set; }
}

class DBUserInfo
{
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public string SaltValue { get; set; }
}