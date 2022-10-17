namespace OAuth2Client.Security.OAuth;

public class KakaoOAuthDefaults : OAuthDefaultsBase
{
    public static string AuthenticationScheme => "Kakao";
    public static string ClaimIssuer => "Kakao";
    public static string CallbackPath => $"{CallbackPathPrefix}/kakao";
    public static string TokenEndpoint => "https://kauth.kakao.com/oauth/token";
    public static string UserInformationEndpoint => "https://kapi.kakao.com/v2/user/me";
    public static string AuthorizationEndpoint => "https://kauth.kakao.com/oauth/authorize";
}