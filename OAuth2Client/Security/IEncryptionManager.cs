namespace OAuth2Client.Security;

public interface IEncryptionManager<T, TData>
{
    T Encrypt(TData value, byte[] passwordBytes);
    TData Decrypt(T encrypted, byte[] passwordBytes);
}