using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace OAuth2Client.Security.Cryptography.Aes256;

public class AesEncryptionManager : AbstractEncryptionManager
{
    public override string Encrypt(object value, string password)
    {
        using var aes = CreateAes(password);
        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        using var streamWriter = new StreamWriter(cryptoStream);
        
        var serialized = JsonSerializer.Serialize(value);
        streamWriter.Write(serialized);
        streamWriter.Flush();

        var encrypted = memoryStream.ToArray();

        return Convert.ToBase64String(encrypted);
    }

    public override T? Decrypt<T>(string encrypted, string password) where T : class
    {
        using var aes = CreateAes(password);
        var cipher = Convert.FromBase64String(encrypted);
        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var memoryStream = new MemoryStream(cipher);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);
        var decrypted = streamReader.ReadToEnd();

        return JsonSerializer.Deserialize<T>(decrypted);
    }

    private Aes CreateAes(string password)
    {
        var aes = Aes.Create();
        aes.KeySize = 256;
        aes.BlockSize = 128;
        aes.Key = Encoding.UTF8.GetBytes(password);
        return aes;
    }
}