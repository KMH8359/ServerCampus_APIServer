﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZLogger;
using static APIServer.Controllers.CheckMatching;

namespace APIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class CheckMatching : Controller
{
    IMatchWoker _matchWorker;


    public CheckMatching(IMatchWoker matchWorker)
    {
        _matchWorker = matchWorker;
    }

    [HttpPost]
    public CheckMatchingResponse Post(CheckMatchingRequest request)
    {
        CheckMatchingResponse response = new();

        (var result, var completeMatchingData) = _matchWorker.GetCompleteMatching(request.UserID);

        //TODO: 결과를 담아서 보낸다
        if (completeMatchingData != null)
        {
            response.Result = ErrorCode.None;
            response.ServerAddress = completeMatchingData.ServerAddress;
            response.RoomNumber = completeMatchingData.RoomNumber;
            response.PortNumber = completeMatchingData.PortNumber;
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
    public int PortNumber { get; set; } = 0;
    public int RoomNumber { get; set; } = 0;    
}
