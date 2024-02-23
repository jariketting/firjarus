namespace FitJarus.Models.Auth;

public class AuthRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool ReturnSecureToken { get; set; }

    public AuthRequest()
    {
        ReturnSecureToken = true;
    }
}
