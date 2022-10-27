using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Options;
using OAuth2Client;
using OAuth2Client.Security;
using OAuth2Client.Security.Cookie;
using OAuth2Client.Security.Cryptography;
using OAuth2Client.Security.Jwt;
using OAuth2Client.Security.OAuth;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<CustomOAuthClientOptions>(
    KakaoOAuthDefaults.AuthenticationScheme,
    builder.Configuration.GetRequiredSection("OAuth:Kakao")
);
builder.Services.Configure<CustomJwtOptions>(builder.Configuration.GetRequiredSection("Jwt"));
builder.Services.Configure<AesOptions>(builder.Configuration.GetRequiredSection("Aes"));

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IConfigureOptions<OAuthOptions>, KakaoOAuthOptions>();
builder.Services.AddSingleton<IConfigureOptions<CookieAuthenticationOptions>, CustomCookieAuthOptions>();
builder.Services.AddTransient<ISecureDataFormat<AuthenticationTicket>, CustomTicketDataFormat>();
builder.Services.AddTransient<ISecureDataFormat<AuthenticationProperties>, CustomOAuthStateDataFormat>();
builder.Services.AddTransient<ITokenProvider, JwtTokenProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = KakaoOAuthDefaults.AuthenticationScheme;
    })
    .AddCookie(delegate { })
    .AddOAuth(KakaoOAuthDefaults.AuthenticationScheme, _ => { });

builder.Services.AddAuthorization(options => options.DefaultPolicy = new AuthorizationPolicy(
    new[]
    {
        new RolesAuthorizationRequirement(new[] { RoleName.User })
    },
    new[]
    {
        CookieAuthenticationDefaults.AuthenticationScheme,
        KakaoOAuthDefaults.AuthenticationScheme
    }
));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(routeBuilder => routeBuilder.MapControllers());

app.MapControllerRoute(
    "default",
    "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();