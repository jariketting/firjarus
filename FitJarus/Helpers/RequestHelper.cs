using FitJarus.Models;
using FitJarus.Models.Auth;
using System.Net.Http.Json;

namespace FitJarus.Helpers;

public class RequestHelper(Secrets secrets)
{
    private const string DefaultApiUri = "https://fitjarus-default-rtdb.europe-west1.firebasedatabase.app/";

    public async Task<ApiResponse<AuthResponse>> Login(AuthRequest authRequest)
    {
        var result = new ApiResponse<AuthResponse>();

        using (HttpClient client = new HttpClient())
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
}
