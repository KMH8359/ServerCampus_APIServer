using MySqlConnector;
using SqlKata.Execution;
using Microsoft.AspNetCore.Mvc; 

// [] 기호는 함수의 특성을 나타내는 애트리뷰트 기호
[ApiController]                 // 유효성 검사 및 400 응답, 속성 라우팅, 바인딩된 소스 매개변수 추론 등의 기능을 수행하는 애트리뷰트. 하단의 route를 요구함
[Route("[controller]")]         // 쿼리가 호출해야 하는 함수를 라우팅하는 애트리뷰트
public class CreateAccount : Controller
{
    [HttpPost]
    public async Task<CreateAccountResponse> Signup(CreateAccountRequest request)
    {
        var response = new CreateAccountResponse {Result = ErrorCode.None};
        
        var saltValue = Security.SaltString();
        var hashingPassword = Security.MakeHashingPassWord(saltValue, request.Password);
        
        using (var db = await DBManager.GetGameDBQuery())
        {
            try
            {
                var count = await db.Query("account").InsertAsync(new
                {
                    Email = request.Email,
                    SaltValue = saltValue,
                    HashedPassword = hashingPassword
                });

                if (count != 1)
                {
                    response.Result = ErrorCode.Create_Account_Fail_Duplicate;
                }

                Console.WriteLine($"[Request CreateAccount] Email:{request.Email}, saltValue:{saltValue}, hashingPassword:{hashingPassword}");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.Result = ErrorCode.Create_Account_Fail_Exception;
                return response;
            }
            finally
            {
                db.Dispose();
            }
        }

        return response;
    }

    
}


public class CreateAccountRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class CreateAccountResponse
{
    public ErrorCode Result { get; set; }
}