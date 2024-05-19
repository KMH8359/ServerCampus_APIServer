using System;
using System.Threading.Tasks;

namespace HIVESERVER.Repository;

public interface IMemoryDb : IDisposable
{    
    public Task<ErrorCode> RegisterUserAsync(string id, string authToken, long accountId);

    public Task<ErrorCode> CheckUserAuthAsync(string id, string authToken);

    public Task<(bool, UserAuthData)> GetUserAsync(string id);

    
}