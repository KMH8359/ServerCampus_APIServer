﻿using System;
using System.Threading.Tasks;


namespace GAMESERVER.Services;

// interface : 클래스 간의 통신을 정의하는 객체
public interface IAccountDb : IDisposable   // IDisposable - Dispose() 함수를 통해 개발자 차원에서 명시적 자원 제거가 가능한 클래스
{
    public Task<ErrorCode> CreateUserGameDataAsync(String id);

    public Task<bool> GetUserAsync(string id);
}