using FitJarus.Models;
using FitJarus.Models.Auth;
using System.Net.Http.Json;

namespace FitJarus.Helpers;

public class RequestHelper(Secrets secrets, CookieHelper cookiehelper)
{
    private const string DefaultApiUri = "https://fitjarus-default-rtdb.europe-west1.firebasedatabase.app";

    public async Task<ApiResponse<AuthResponse>> Login(AuthRequest authRequest)
    {
        var result = new ApiResponse<AuthResponse>();

        using (HttpClient client = new())
        {
            var content = JsonContent.Create(authRequest);
            var response = await client.PostAsync($"https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key={secrets.GetFireBaseApiKey}", content);

            if (response.IsSuccessStatusCode)
            {
                result.Ok();
                result.Content = await response.Content.ReadFromJsonAsync<AuthResponse>();
            } else
            {
                result.Error = "Login failed";
            }
        }

        return result;
    }

    public async Task<ApiResponse<T>> Get<T>(string path)
    {
        var result = new ApiResponse<T>();
        var authCookie = await cookiehelper.GetAuthCookie();

        using (HttpClient client = new())
        {
            var response = await client.GetAsync($"{DefaultApiUri}/{path}?auth={authCookie}");

            if (response.IsSuccessStatusCode)
            {
                result.Ok();
                result.Content = await response.Content.ReadFromJsonAsync<T>();
            }
            else
            {
                result.Error = "Request failed";
            }
        }

        return result;
    }

    public async Task<ApiResponse<T>> Post<T>(string path, object data)
    {
        var result = new ApiResponse<T>();
        var authCookie = await cookiehelper.GetAuthCookie();

        using (HttpClient client = new())
        {
            var content = JsonContent.Create(data);
            var response = await client.PostAsync($"{DefaultApiUri}/{path}?auth={authCookie}", content);

            if (response.IsSuccessStatusCode)
            {
                result.Ok();
                result.Content = await response.Content.ReadFromJsonAsync<T>();
            }
            else
            {
                result.Error = "Request failed";
            }
        }

        return result;
    }

    public async Task<ApiResponse<T>> Put<T>(string path, object data)
    {
        var result = new ApiResponse<T>();
        var authCookie = await cookiehelper.GetAuthCookie();

        using (HttpClient client = new())
        {
            var content = JsonContent.Create(data);
            var response = await client.PutAsync($"{DefaultApiUri}/{path}?auth={authCookie}", content);

            if (response.IsSuccessStatusCode)
            {
                result.Ok();
                result.Content = await response.Content.ReadFromJsonAsync<T>();
            }
            else
            {
                result.Error = "Request failed";
            }
        }

        return result;
    }

    public async Task<ApiResponse> Delete(string path)
    {
        var result = new ApiResponse();
        var authCookie = await cookiehelper.GetAuthCookie();

        using (HttpClient client = new())
        {
            var response = await client.DeleteAsync($"{DefaultApiUri}/{path}?auth={authCookie}");

            if (response.IsSuccessStatusCode)
            {
                result.Ok();
            }
            else
            {
                result.Error = "Request failed";
            }
        }

        return result;
    }
}
