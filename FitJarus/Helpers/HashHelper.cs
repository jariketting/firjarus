using System.Security.Cryptography;
using System.Text;

namespace FitJarus.Helpers;

public static class HashHelper
{
    public static string CalculateSHA256(byte[] bytes)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(bytes);
        var stringBuilder = new StringBuilder();

        foreach (byte b in hashBytes)
        {
            stringBuilder.Append(b.ToString("x2"));
        }

        return stringBuilder.ToString();
    }
}
