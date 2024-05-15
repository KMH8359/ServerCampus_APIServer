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
public class CheckMatching : ControllerBase
{
    private readonly ILogger _logger;
    private readonly HttpClient _httpClient;   // API 서버와 통신하기 위한 HTTP 클라이언트 객체
    private readonly string MatchingServerURL;


    public CheckMatching(ILogger<Matching> logger,  HttpClient httpClient, IConfiguration configuration)
    {
        _logger = logger;
        _httpClient = httpClient;
        MatchingServerURL = configuration["MatchingServerAddress"];
    }

    [HttpPost]
    public async Task<CheckMatchingResponse> CheckMatchingAsync(CheckMatchingRequest request)
    {
        var response = new CheckMatchingResponse();

        try
        {
            string url = MatchingServerURL + "/CheckMatching";
            string jsonContent = $"{{ \"UserID\": \"{request.UserID}\" }}";
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PostAsync(url, content);

            if (httpResponse.IsSuccessStatusCode)
            {
                var responseString = await httpResponse.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<CheckMatchingResponse>(responseString);
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


public class CheckMatchingRequest
{
    public string UserID { get; set; }
}

public class CheckMatchingResponse
{
    public ErrorCode Result { get; set; } = ErrorCode.MatchingInProgress;
    public string ServerAddress { get; set; } = "";
    public int RoomNumber { get; set; } = 0;
}
