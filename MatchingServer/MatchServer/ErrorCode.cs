using System;

// 1000 ~ 19999
public enum ErrorCode : UInt16
{
    None = 0,

    AythCheckFail = 5021,
    ReceiptCheckFail = 5022,
    MatchingInProgress = 5023,
    MatchingReqFail = 5024,


}