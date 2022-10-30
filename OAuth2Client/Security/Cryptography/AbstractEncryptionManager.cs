namespace OAuth2Client.Security.Cryptography;

public abstract class AbstractEncryptionManager : IEncryptionManager
{
    protected IEncryptionManagerHolder Holder { get; }

    public abstract string Encrypt(object value, string password);
    public abstract T? Decrypt<T>(string encrypted, string password) where T : class;
}