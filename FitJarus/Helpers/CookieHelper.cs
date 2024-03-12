using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

using System.Security.Claims;

namespace FitJarus.Helpers;

public class CookieHelper
{
    private Lazy<IJSObjectReference> _accessorJsRef = new();
    private readonly IJSRuntime _jsRuntime;

    public CookieHelper(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    private async Task WaitForReference()
    {
        if (_accessorJsRef.IsValueCreated is false)
        {
            _accessorJsRef = new(await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/CookieStorageAccessor.js"));
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_accessorJsRef.IsValueCreated)
        {
            await _accessorJsRef.Value.DisposeAsync();
        }
    }

    public async Task<T> GetValueAsync<T>(string key)
    {
        await WaitForReference();
        var result = await _accessorJsRef.Value.InvokeAsync<T>("get", key);

        return result;
    }

    public async Task SetValueAsync<T>(string key, T value)
    {
        await WaitForReference();
        await _accessorJsRef.Value.InvokeVoidAsync("set", key, value);
    }

    public async Task<string> GetAuthCookie()
    {
        string tokenCookie = await GetValueAsync<string>("token");
        string tokenStart = "token=";

        int index = tokenCookie.IndexOf(tokenStart);
        if (index != -1)
        {
            return tokenCookie.Substring(index + tokenStart.Length);
        }

        return "";
    }
}
