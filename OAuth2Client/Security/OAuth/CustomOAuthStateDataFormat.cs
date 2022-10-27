using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using OAuth2Client.Security.Cryptography;

namespace OAuth2Client.Security.OAuth;

public class CustomOAuthStateDataFormat : ISecureDataFormat<AuthenticationProperties>
{
    public CustomOAuthStateDataFormat(IOptions<AesOptions> wrappedAesOptions)
    {
        var aesOptions = wrappedAesOptions.Value;
        PasswordBytes = Encoding.UTF8.GetBytes(aesOptions.Password);
    }

    private byte[] PasswordBytes { get; }

    public string Protect(AuthenticationProperties data) => Protect(data, null);

    public string Protect(AuthenticationProperties data, string? purpose)
    {
        var plainText = JsonSerializer.SerializeToUtf8Bytes(data);
        
        using var aes = new AesGcm(PasswordBytes);
        
        var tag = new byte[AesGcm.TagByteSizes.MaxSize];
        var nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
        var cipherText = new byte[plainText.Length];
        
        aes.Encrypt(nonce, plainText, cipherText, tag);
        
        var encrypted = new EncryptedAesGcm(nonce, cipherText, tag);
        var serialized = JsonSerializer.SerializeToUtf8Bytes(encrypted);
        
        return Convert.ToBase64String(serialized);
    }

    public AuthenticationProperties? Unprotect(string? protectedText) => Unprotect(protectedText, null);

    public AuthenticationProperties? Unprotect(string? protectedText, string? purpose)
    {
        if (protectedText is null) return null;

        var protectedBytes = Convert.FromBase64String(protectedText);
        var encrypted = JsonSerializer.Deserialize<EncryptedAesGcm>(protectedBytes);

        if (encrypted is null) return null;

        var plainTextBytes = encrypted.Decrypt(PasswordBytes);

        return JsonSerializer.Deserialize<AuthenticationProperties>(plainTextBytes);
    }
}