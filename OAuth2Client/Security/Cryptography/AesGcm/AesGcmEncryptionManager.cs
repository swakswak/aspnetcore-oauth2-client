using System.Text;
using System.Text.Json;

namespace OAuth2Client.Security.Cryptography.AesGcm;

public class AesGcmEncryptionManager : AbstractEncryptionManager
{
    public override string Encrypt(object value, string password)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var plainText = JsonSerializer.SerializeToUtf8Bytes(value);
        using var aesGcm = new System.Security.Cryptography.AesGcm(passwordBytes);

        var tag = new byte[System.Security.Cryptography.AesGcm.TagByteSizes.MaxSize];
        var nonce = new byte[System.Security.Cryptography.AesGcm.NonceByteSizes.MaxSize];
        var cipherText = new byte[plainText.Length];

        aesGcm.Encrypt(nonce, plainText, cipherText, tag);

        var encrypted = new EncryptedAesGcm(nonce, cipherText, tag);
        var serialized = JsonSerializer.SerializeToUtf8Bytes(encrypted);

        return Convert.ToBase64String(serialized);
    }

    public override T? Decrypt<T>(string encrypted, string password) where T : class
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var protectedBytes = Convert.FromBase64String(encrypted);
        var deserialized = JsonSerializer.Deserialize<EncryptedAesGcm>(protectedBytes);

        if (deserialized is null) return default;

        using var aes = new System.Security.Cryptography.AesGcm(passwordBytes);
        var plainTextBytes = new byte[deserialized.CipherText.Length];
        aes.Decrypt(deserialized.Nonce, deserialized.CipherText, deserialized.Tag, plainTextBytes);

        return JsonSerializer.Deserialize<T>(plainTextBytes);
    }
}