using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using OAuth2Client.Security.Cryptography;
using OAuth2Client.Security.Cryptography.AesGcm;

namespace OAuth2Client.Security.OAuth;

public class CustomOAuthStateDataFormat : ISecureDataFormat<AuthenticationProperties>
{
    public CustomOAuthStateDataFormat(IOptions<AesOptions> wrappedAesOptions, IEncryptionManagerHolder encryptionManagerHolder)
    {
        var aesOptions = wrappedAesOptions.Value;
        Password = aesOptions.Password;
        EncryptionManager = encryptionManagerHolder.Get(EncryptionManagerType.AesGcm);
    }

    private string Password { get; }

    private IEncryptionManager EncryptionManager { get; }

    public string Protect(AuthenticationProperties data) => Protect(data, null);

    public string Protect(AuthenticationProperties data, string? purpose) => 
        EncryptionManager.Encrypt(data, Password);

    public AuthenticationProperties? Unprotect(string? protectedText) => Unprotect(protectedText, null);

    public AuthenticationProperties? Unprotect(string? protectedText, string? purpose) => 
        protectedText is null ? null : EncryptionManager.Decrypt<AuthenticationProperties>(protectedText, Password);
}