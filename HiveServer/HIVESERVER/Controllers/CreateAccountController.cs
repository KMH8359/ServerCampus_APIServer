using System.ComponentModel.DataAnnotations;
using HIVESERVER.Services;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace HIVESERVER.Controllers;

// [] 기호는 함수의 특성을 나타내는 애트리뷰트 기호
[ApiController]                 // 유효성 검사 및 400 응답, 속성 라우팅, 바인딩된 소스 매개변수 추론 등의 기능을 수행하는 애트리뷰트. 하단의 route를 요구함
[Route("[controller]")]         // 쿼리가 호출해야 하는 함수를 라우팅하는 애트리뷰트
public class CreateAccount : ControllerBase // Controller 클래스는 ControllerBase 클래스를 상속하는 자식 클래스. MVC의 View를 지원하는 기능을 갖고 있음
{
    private readonly ILogger _logger;
    private readonly IAccountDb _accountDb; 

    public CreateAccount(ILogger<CreateAccount> logger, IAccountDb accountDb)
    {
        _logger = logger;
        _accountDb = accountDb;
    }
    
    [HttpPost]
    public async Task<ErrorCode> SignupAsync(CreateAccountRequest request)   // 비동기 함수는 async 키워드를 사용하고 Task<>나 Task값을 리턴함
    {
        var response = ErrorCode.None;
        
        var errorCode = await _accountDb.CreateAccountAsync(request.Id, request.Password);
        if (errorCode != ErrorCode.None)
        {
            response = errorCode;
            _logger.ZLogError($"[CreateAccount] ErrorCode: {errorCode},Email: {request.Id}, Password: {request.Password}");
            return response;
        }

        _logger.ZLogInformation($"[CreateAccount] Email: {request.Id}, Password: {request.Password}");

        return response;
    }
    
}


public class CreateAccountRequest
{
    [Required] // CANNOT BE NULL
    [MinLength(1, ErrorMessage = "EMAIL CANNOT BE EMPTY")]  // MINLENGTH
    [StringLength(50, ErrorMessage = "EMAIL IS TOO LONG")]  // MAXLENGTH
    //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]   // ID@Domain.TOPDomain ex) KMH8359@naver.com
    public string Id { get; set; }  

    [Required]
    [MinLength(1, ErrorMessage = "PASSWORD CANNOT BE EMPTY")]
    [StringLength(30, ErrorMessage = "PASSWORD IS TOO LONG")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}

public class CreateAccountResponse
{
    public ErrorCode Result { get; set; }
}