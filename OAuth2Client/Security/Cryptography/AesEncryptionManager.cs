// using System.Security.Cryptography;
// using Microsoft.AspNetCore.Authentication;
//
// namespace OAuth2Client.Security.Cryptography;
//
// public class AesEncryptionManager : IEncryptionManager<EncryptedAes, AuthenticationProperties>
// {
//     public EncryptedAes Encrypt(AuthenticationProperties value, byte[] passwordBytes)
//     {
//         using var aes = Aes.Create();
//         aes.Key = aes.Key;
//     }
//
//     public AuthenticationProperties Decrypt(EncryptedAes encrypted, byte[] passwordBytes)
//     {
//         throw new NotImplementedException();
//     }
// }