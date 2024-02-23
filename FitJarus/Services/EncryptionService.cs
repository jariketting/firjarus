using System.Security.Cryptography;
using System.Text;

namespace FitJarus.Services;

public static class EncryptionService
{
    public static string Encrypt(string text)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(Consts.UnlockKey);
        byte[] textBytes = Encoding.UTF8.GetBytes(text);

        for (int i = 0; i < textBytes.Length; i++)
        {
            textBytes[i] = (byte)(textBytes[i] ^ keyBytes[i % keyBytes.Length]);
        }

        return Convert.ToBase64String(textBytes);
    }

    public static string Decrypt(string encryptedText)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(Consts.UnlockKey);
        byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

        for (int i = 0; i < encryptedBytes.Length; i++)
        {
            encryptedBytes[i] = (byte)(encryptedBytes[i] ^ keyBytes[i % keyBytes.Length]);
        }

        return Encoding.UTF8.GetString(encryptedBytes);
    }
}
