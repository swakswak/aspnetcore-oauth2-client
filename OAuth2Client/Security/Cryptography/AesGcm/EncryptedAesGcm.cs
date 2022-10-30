namespace OAuth2Client.Security.Cryptography.AesGcm;

public record EncryptedAesGcm
(
    byte[] Nonce,
    byte[] CipherText,
    byte[] Tag
);
