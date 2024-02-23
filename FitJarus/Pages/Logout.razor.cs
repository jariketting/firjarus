using Microsoft.AspNetCore.Components.Web;

namespace FitJarus.Pages;

public partial class Logout
{
    protected override void OnInitialized()
    {
        _authenticationStateProvider.SignOut();
        _navigationManager.NavigateTo("/login");
    }
}