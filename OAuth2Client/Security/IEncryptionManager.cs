namespace OAuth2Client.Security;

public interface IEncryptionManager
{
    string Encrypt(object value, string password);
    T? Decrypt<T>(string encrypted, string password) where T : class;
}