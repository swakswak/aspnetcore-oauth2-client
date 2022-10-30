using OAuth2Client.Security;
using OAuth2Client.Security.Cryptography.Aes256;
using Xunit.Abstractions;

namespace OAuth2Client.Tests;

public class EncryptionManagerSpec
{
    private IEncryptionManager AesEncryptionManager => new AesEncryptionManager();

    public EncryptionManagerSpec(ITestOutputHelper output) => Output = output;

    private ITestOutputHelper Output { get; }

    [Fact]
    public void Should_Encrypt()
    {
        var passwordString = "mysecretpasswordmysecretpassword";
        
        var dictionary = new Dictionary<string, string>()
        {
            ["nickname"] = "swakswak",
            ["email"] = "swak@swak.swak"
        };
        var encrypted = AesEncryptionManager.Encrypt(dictionary, passwordString);
        
        Output.WriteLine(encrypted);
        Assert.NotEmpty(encrypted);
    }
}