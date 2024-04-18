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
    static HttpClient _httpClient = new HttpClient();   // API 서버와 통신하기 위한 HTTP 클라이언트 객체
    static readonly string ApiServerURL = "http://localhost:5256/Authentication"; // 프로그램 수명 동안 계속 유지하기 위한 static 키워드 사용

    [HttpPost]
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var response = new LoginResponse();
        response.Result = ErrorCode.None;
        var authResponse = new AuthResponse();
        try
        {
            var httpResponse =
                await _httpClient.PostAsJsonAsync(ApiServerURL,
                    request); // API 서버로 패킷을 보내기 위해 request 객체를 json으로 캐스팅하여 전달

            authResponse =
                await httpResponse.Content
                    .ReadFromJsonAsync<
                        AuthResponse>(); // API 서버가 반환한 httpResponse 객체 내부의 필드 Content를 AuthResponse으로 읽어오는 함수

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            response.Result = ErrorCode.Login_Fail_Exception;
            return response;
        }
        
        if (authResponse.Result != ErrorCode.None)
        {
            response.Result = authResponse.Result;
            return response;
        }
        
        
        // 처음 로그인한 계정은 UserGameData에 본인의 데이터를 생성해야 한다.
        using (var db = await DBManager.GetGameDBQuery())
        {
            var userGameData = await db.Query("UserGameData").Where("AccountId", authResponse.AccountId).FirstOrDefaultAsync<DBUserGameData>(); // 조건을 만족하는 컬럼이 있는지만 알면 되고 굳이 가져올 필요는 없는데 FirstOrDefaultAsync 말고 다른 함수는 없을까
            if (userGameData == null)
            {
                var count = await db.Query("UserGameData").InsertAsync(new
                {
                    AccountId = authResponse.AccountId,
                    Level = 1,
                    EXP = 0,
                    Win = 0,
                    Lose = 0
                });

                if (count != 1)
                {
                    response.Result = ErrorCode.INSERT_GAMEDATA_FAIL;
                    return response;
                }
            }
            
        }

        string tokenValue = Security.CreateAuthToken();
        var idDefaultExpiry = TimeSpan.FromDays(1);
        var redisId = new RedisString<string>(DBManager.RedisConn, request.Email, idDefaultExpiry);
        await redisId.SetAsync(tokenValue);

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

public class AuthResponse
{
    public ErrorCode Result { get; set; }
    public long AccountId { get; set; }
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