using FitJarus.Exceptions;
using FitJarus.Models.Auth;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace FitJarus.Pages;

public partial class Login
{
    readonly AuthRequest AuthRequest = new();
    string error = "";
    bool loginLoading = false;

    async Task FormSubmittedAsync(EditContext editContext)
    {
        loginLoading = true;

        try
        {
            await _authenticationStateProvider.SignIn(AuthRequest);
            _navigationManager.NavigateTo("/");
        } catch (LoginException ex)
        {
            loginLoading = false;
            error = ex.Message;
        }
    }
}