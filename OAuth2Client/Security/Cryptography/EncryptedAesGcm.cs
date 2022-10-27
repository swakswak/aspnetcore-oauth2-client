using System.Security.Cryptography;

namespace OAuth2Client.Security.Cryptography;

public record EncryptedAesGcm
(
    byte[] Nonce,
    byte[] CipherText,
    byte[] Tag
)
{
    public byte[] Decrypt(byte[] passwordBytes)
    {
        using var aes = new AesGcm(passwordBytes);
        var plainTextBytes = new byte[CipherText.Length];
        aes.Decrypt(Nonce, CipherText, Tag, plainTextBytes);

        return plainTextBytes;
    }
}
