using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebLabRest.Tests;

public class TestSession : ISession
{
    private readonly Dictionary<string, byte[]> _sessionStorage = new();
    public bool IsAvailable => true;
    public string Id => "TestSessionId";
    public IEnumerable<string> Keys => _sessionStorage.Keys;

    public void Clear() => _sessionStorage.Clear();
    public void Remove(string key) => _sessionStorage.Remove(key);
    public void Set(string key, byte[] value) => _sessionStorage[key] = value;
    public Task LoadAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public Task CommitAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public bool TryGetValue(string key, out byte[] value) => _sessionStorage.TryGetValue(key, out value);
}
