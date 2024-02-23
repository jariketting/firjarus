using FitJarus.Exceptions;
using FitJarus.Helpers;
using FitJarus.Models.Auth;
using Microsoft.AspNetCore.Components.Authorization;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FitJarus.Providers;

// https://ferrywlto.medium.com/the-missing-piece-in-blazor-client-side-authentication-tutorial-a94b297b30a6
public class CustomAuthenticationStateProvider(CookieHelper cookieHelper, RequestHelper requestHelper) : AuthenticationStateProvider
{
    private readonly CookieHelper _cookieHelper = cookieHelper;

    private static ClaimsPrincipal AnonymousUser => new(new ClaimsIdentity(Array.Empty<Claim>()));

    private static IEnumerable<Claim> GetClaimsFromIdToken(string idToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(idToken) as JwtSecurityToken;

        var claims = jsonToken?.Claims;

        return claims ?? new List<Claim>();
    }

    private static bool IsValidIdToken(string idToken)
    {
        if (string.IsNullOrEmpty(idToken))
        {
            return false;
        }

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(idToken) as JwtSecurityToken;

            if(jsonToken.ValidTo <= DateTime.Now)
            {
                return true;
            }            
        }
        catch (Exception)
        {
            return false;
        }

        return false;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string tokenCookie = await _cookieHelper.GetValueAsync<string>("token");
        string tokenStart = "token=";

        int index = tokenCookie.IndexOf(tokenStart);
        if (index != -1)
        {
            string idToken = tokenCookie.Substring(index + tokenStart.Length);

            if (IsValidIdToken(idToken))
            {
                var claims = GetClaimsFromIdToken(idToken);
                var identity = new ClaimsIdentity(claims, "custom");
                var user = new ClaimsPrincipal(identity);
                var authState = new AuthenticationState(user);

                return authState;
            }
        }

        return await Task.FromResult(new AuthenticationState(AnonymousUser));
    }

    public async Task SignIn(AuthRequest authRequest)
    {
        var loginResult = await requestHelper.Login(authRequest);

        if(loginResult.Success)
        {
            var authResponse = loginResult.Content;

            await _cookieHelper.SetValueAsync("token", authResponse.IdToken);

            var claims = GetClaimsFromIdToken(authResponse.IdToken);
            var identity = new ClaimsIdentity(claims, "custom");
            var user = new ClaimsPrincipal(identity);
            var authState = new AuthenticationState(user);

            var result = Task.FromResult(authState);
            NotifyAuthenticationStateChanged(result);
        } else
        {
            throw new LoginException("Je email of wachtwoord is onjuist");
        }
    }

    public async Task SignOut()
    {
        await _cookieHelper.SetValueAsync("token", "");

        var result = Task.FromResult(new AuthenticationState(AnonymousUser));
        NotifyAuthenticationStateChanged(result);
    }
}
