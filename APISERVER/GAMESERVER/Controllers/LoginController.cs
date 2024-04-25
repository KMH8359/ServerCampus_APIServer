using System.ComponentModel.DataAnnotations;
using GAMESERVER.Repository;
using GAMESERVER.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ZLogger;

namespace GAMESERVER.Controllers;

[ApiController]
[Route("[controller]")]
public class Login : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IAccountDb _accountDb;
    private readonly IMemoryDb _memoryDb;
    private readonly HttpClient _httpClient;   // API 서버와 통신하기 위한 HTTP 클라이언트 객체
    static readonly string ApiServerURL = "http://localhost:5256/Login"; // 프로그램 수명 동안 계속 유지하기 위한 static 키워드 사용

    public Login(ILogger<Login> logger, IAccountDb accountDb, IMemoryDb memoryDb, HttpClient httpClient)
    {
        _logger = logger;
        _accountDb = accountDb;
        _memoryDb = memoryDb;
        _httpClient = httpClient;
    }

    [HttpPost]
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var response = new LoginResponse();
        response.Result = ErrorCode.None;
        var loginResponse = new LoginResponse();


        (bool succeed, UserAuthData userAuthData) = await _memoryDb.GetUserAsync(request.Id);   // redis에 로그인 정보가 존재하는지 확인
        if (succeed && userAuthData.AuthToken == request.AuthToken)
        {
            response.AuthToken = userAuthData.AuthToken;
            _logger.ZLogInformation($"[Login] id:{request.Id}, AuthToken:{request.AuthToken}");
            return response;
        }

        // 하이브 서버에 인증 토큰 검증 요청     
        ErrorCode errorCode = await RequestVerifyAuthTokenAsync(request);
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }
        
        bool hasGameData = await _accountDb.GetUserAsync(request.Id);

        if (hasGameData == false)
        {
            errorCode = await _accountDb.CreateUserGameDataAsync(request.Id);
            if (errorCode != ErrorCode.None)
            {
                response.Result = errorCode;
                return response;
            }
        }
        errorCode = await _memoryDb.RegisterUserAsync(request.Id, request.AuthToken);   // 로그인 정보를 Redis에 저장
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            _logger.ZLogError($"[Login] ErrorCode:{errorCode} id:{request.Id}, AuthToken:{request.AuthToken}");
            return response;
        }

        _logger.ZLogInformation($"[Login] id:{request.Id}, AuthToken:{request.AuthToken}");

        response.AuthToken = request.AuthToken;
        return response;
    }

    public async Task<ErrorCode> RequestVerifyAuthTokenAsync(LoginRequest request)
    {
        ErrorCode result = ErrorCode.None;
        try
        {
            string queryString = ApiServerURL + $"?Id={request.Id}&AuthToken={request.AuthToken}"; 
            var httpResponse = await _httpClient.GetAsync(queryString); 

            var responseString = await httpResponse.Content.ReadAsStringAsync();
            if (Enum.TryParse(responseString, out result) == false)
            {
                return ErrorCode.VerifyAuthTokenFail;
            }

        }
        catch (Exception ex)
        {
            _logger.ZLogError($"[Login] {ex.Message}");
            return ErrorCode.LoginFailException;
        }

        return result;
    }
}


public class LoginRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "ID CANNOT BE EMPTY")]
    [StringLength(10, ErrorMessage = "ID IS TOO LONG")]
    public string? Id { get; set; }

    public string? AuthToken { get; set; }
}

public class LoginResponse
{
    public ErrorCode Result { get; set; }
    public string? AuthToken { get; set; }
}
