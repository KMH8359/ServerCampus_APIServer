using System;
using System.Threading.Tasks;

namespace GAMESERVER.Repository;

public interface IMemoryDb : IDisposable
{    
    public Task<ErrorCode> RegisterUserAsync(string id, string authToken);

    public Task<ErrorCode> CheckUserAuthAsync(string id, string authToken);

    public Task<(bool, UserAuthData)> GetUserAsync(string id);

    
}