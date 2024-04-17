using MySqlConnector;
using SqlKata.Execution;
using Microsoft.AspNetCore.Mvc; 

// [] 기호는 함수의 특성을 나타내는 애트리뷰트 기호
[ApiController]                 // 유효성 검사 및 400 응답, 속성 라우팅, 바인딩된 소스 매개변수 추론 등의 기능을 수행하는 애트리뷰트. 하단의 route를 요구함
[Route("[controller]")]         // 쿼리가 호출해야 하는 함수를 라우팅하는 애트리뷰트
public class CreateAccount : Controller
{
    [HttpPost]
    public async Task<CreateAccountResponse> Signup(CreateAccountRequest request)   // 비동기 함수 async Task<>
    {
        var response = new CreateAccountResponse {Result = ErrorCode.None};
        
        var saltValue = Security.SaltString();  // 암호화 솔트값 생성. 솔트값은 비밀번호가 생성되거나 변경될 때마다 재생성된다
        var hashingPassword = Security.MakeHashingPassWord(saltValue, request.Password); // 솔트값과 패스워드를 사용하여 암호화
        
        using (var db = await DBManager.GetGameDBQuery())   // db 변수의 명시적인 자원 해제를 위한 대괄호 사용
        {
            try
            {
                var count = await db.Query("account").InsertAsync(new   // 성공시 1, 실패시 0 리턴
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