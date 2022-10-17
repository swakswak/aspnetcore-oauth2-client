using System.Text.Json.Serialization;

namespace OAuth2Client.Security.OAuth;

public class KakaoUser
{
    public KakaoUser(long id, KakaoUserProperties properties, KakaoUserAccount kakaoAccount)
    {
        Id = id;
        Properties = properties;
        KakaoAccount = kakaoAccount;
    }

    [JsonPropertyName("id")]
    public long Id { get; }
    
    [JsonPropertyName("properties")]
    public KakaoUserProperties Properties { get; }
    
    [JsonPropertyName("kakao_account")]
    public KakaoUserAccount KakaoAccount { get; }
}

public class KakaoUserAccount
{
    public KakaoUserAccount(string? email)
    {
        Email = email;
    }

    [JsonPropertyName("email")]
    public string? Email { get; }
}

public class KakaoUserProperties
{
    public KakaoUserProperties(string nickname, string profileImage)
    {
        Nickname = nickname;
        ProfileImage = profileImage;
    }

    [JsonPropertyName("nickname")]
    public string Nickname { get; }

    [JsonPropertyName("profile_image")] 
    public string ProfileImage { get; }
}