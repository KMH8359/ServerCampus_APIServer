using CloudStructures.Structures;
using Microsoft.AspNetCore.Mvc;
using SqlKata.Execution;


[ApiController]
[Route("[controller]")]
public class Authentication : ControllerBase
{
    [HttpPost]
    public async Task<AuthResponse> AuthenticateAsync(AuthRequest request)
    {
        var response = new AuthResponse();
        response.Result = ErrorCode.None;

        using (var db = await DBManager.GetGameDBQuery())
        {
            var userInfo = await db.Query("account").Where("Email", request.Email).FirstOrDefaultAsync<DBUserInfo>();   // DB에서 Where절을 만족하는 컬럼을 찾는 SELECT 쿼리 호출. 첫번째 요소를 반환하며 실패시 null을 리턴함

            if (userInfo == null || string.IsNullOrEmpty(userInfo.HashedPassword))
            {
                response.Result = ErrorCode.Login_Fail_NotUser;
                return response;
            }
                        
            var hashingPassword = Security.MakeHashingPassWord(userInfo.SaltValue, request.Password);       // 유저의 솔트값과 입력받은 비밀번호로 만든 해싱패스워드가 일치하면 로그인 성공

            Console.WriteLine($"[Request Authenticate] Email:{request.Email}, request.Password:{request.Password},  saltValue:{userInfo.SaltValue}, hashingPassword:{hashingPassword}");
            if (userInfo.HashedPassword != hashingPassword)         
            {
                response.Result = ErrorCode.Login_Fail_PW;
                return response;
            }

            response.AccountId = userInfo.AccountId;        // 인증 후 게임서버의 MySQL에서 유저의 게임 정보를 탐색하기 위해 AccountId 필드를 패킷에 포함 
            db.Dispose();
        }

        return response;
    }
}


public class AuthRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class AuthResponse
{
    public ErrorCode Result { get; set; }
    public long AccountId { get; set; }
}

class DBUserInfo
{
    public long AccountId {get; set;}
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public string SaltValue { get; set; }
}