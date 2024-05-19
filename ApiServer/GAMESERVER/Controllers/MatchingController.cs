using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text;
using CloudStructures;
using GAMESERVER.Repository;
using GAMESERVER.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ZLogger;

namespace APISERVER.Controllers;

[ApiController]
[Route("[controller]")]
public class Matching : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IMemoryDb _memoryDb;
    private readonly HttpClient _httpClient;   // API 서버와 통신하기 위한 HTTP 클라이언트 객체
    private readonly string MatchingServerURL;


    public Matching(ILogger<Matching> logger,  IMemoryDb memoryDb, HttpClient httpClient, IConfiguration configuration)
    {
        _logger = logger;
        _memoryDb = memoryDb;
        _httpClient = httpClient;
        MatchingServerURL = configuration["MatchingServerAddress"];
    }

    [HttpPost]
    public async Task<MatchingResponse> ReqMatchingAsync(MatchingRequest request)
    {
        var response = new MatchingResponse();

        try
        {
            (bool succeed, UserAuthData userAuthData) = await _memoryDb.GetUserAsync(request.UserID);   // redis에 로그인 정보가 존재하는지 확인
            if (!succeed ||  userAuthData.AuthToken != request.AuthToken)
            {
                response.Result = ErrorCode.CheckAuthFailNotMatch;
                return response;
            }


            string url = MatchingServerURL + "/Matching";
            string jsonContent = $"{{ \"UserID\": \"{request.UserID}\" }}";
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PostAsync(url, content);

            if (httpResponse.IsSuccessStatusCode)
            {
                var responseString = await httpResponse.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<MatchingResponse>(responseString);
            }
            else
            {
                response.Result = ErrorCode.MatchingReqFail;
                _logger.ZLogError($"[Matching] {httpResponse.ReasonPhrase} ");
            }
        }
        catch (Exception ex)
        {
            _logger.ZLogError($"[Matching] {ex.Message}");
            response.Result = ErrorCode.UnhandleException;
            return response;
        }

        return response;
    }
}


public class MatchingRequest
{
    public string UserID { get; set; }
    public string? AuthToken { get; set; }
}

public class MatchingResponse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
}
