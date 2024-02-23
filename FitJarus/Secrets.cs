using FitJarus.Services;

namespace FitJarus;

public class Secrets(IConfiguration Configuration)
{
    public string GetFireBaseApiKey { get
        {
            return EncryptionService.Decrypt(Configuration["SuperSecretKey"]);
        }
    }
}