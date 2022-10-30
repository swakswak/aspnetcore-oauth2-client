namespace OAuth2Client.Security.Cryptography;

public interface IEncryptionManagerHolder
{
    IEncryptionManager Get(EncryptionManagerType name);
}