namespace OAuth2Client.Security.OAuth.Kakao;

public class KakaoOAuthDefaults : OAuthDefaultsBase
{
    public static string AuthenticationScheme => "Kakao";
    public static string ClaimIssuer => "Kakao";
    public static string CallbackPath => $"{CallbackPathPrefix}/kakao";
    public static string KAuth => "https://kauth.kakao.com";
    public static string KApi => "https://kapi.kakao.com";
    public static string TokenEndpoint => $"{KAuth}/oauth/token";
    public static string UserInformationEndpoint => $"{KApi}/v2/user/me";
    public static string AuthorizationEndpoint => $"{KAuth}/oauth/authorize";
}