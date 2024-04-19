using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HIVESERVER.Repository;
//using HIVESERVER.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using SqlKata.Execution;
using ZLogger;

namespace HIVESERVER.Controllers;

// [] 기호는 함수의 특성을 나타내는 애트리뷰트 기호
[ApiController]                 // 유효성 검사 및 400 응답, 속성 라우팅, 바인딩된 소스 매개변수 추론 등의 기능을 수행하는 애트리뷰트. 하단의 route를 요구함
[Route("[controller]")]         // 쿼리가 호출해야 하는 함수를 라우팅하는 애트리뷰트
public class CreateAccount : ControllerBase // Controller 클래스는 ControllerBase 클래스를 상속하는 자식 클래스. MVC의 View를 지원하는 기능을 갖고 있음
{
    private readonly ILogger Logger;

    public CreateAccount(ILogger<CreateAccount> logger)
    {
        Logger = logger;
    }
    
    [HttpPost]
    public async Task<CreateAccountResponse> SignupAsync(CreateAccountRequest request)   // 비동기 함수는 async 키워드를 사용하고 Task<>나 Task값을 리턴함
    {
        var response = new CreateAccountResponse {Result = ErrorCode.None};
        
        var saltValue = Security.SaltString();  // 암호화 솔트값 생성. 솔트값은 비밀번호가 생성되거나 변경될 때마다 재생성된다
        var hashingPassword = Security.MakeHashingPassWord(saltValue, request.Password); // 솔트값과 패스워드를 사용하여 암호화
        
        using (var db = await DBManager.GetGameDBQuery())   // db 변수의 명시적인 자원 해제를 위한 대괄호 사용
        {
            try
            {
                var count = await db.Query("account").InsertAsync(new   // DB INSERT - 성공시 1, 실패시 0 리턴
                {
                    Email = request.Email,
                    SaltValue = saltValue,
                    HashedPassword = hashingPassword
                });

                if (count != 1)
                {
                    response.Result = ErrorCode.Create_Account_Fail_Duplicate;
                }

                Logger.ZLogInformation($"[Request CreateAccount] Email:{request.Email}, saltValue:{saltValue}, hashingPassword:{hashingPassword}");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.Result = ErrorCode.Create_Account_Fail_Exception;
                return response;
            }
            finally
            {
                db.Dispose(); // 사용 완료한 DB의 연결 해제
            }
        }

        return response;
    }

    
}


public class CreateAccountRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class CreateAccountResponse
{
    public ErrorCode Result { get; set; }
}