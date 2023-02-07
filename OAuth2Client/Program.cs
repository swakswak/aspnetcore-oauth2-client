using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Options;
using OAuth2Client;
using OAuth2Client.Security;
using OAuth2Client.Security.Cookie;
using OAuth2Client.Security.Jwt;
using OAuth2Client.Security.OAuth;
using OAuth2Client.Security.OAuth.Kakao;
using OAuth2Client.Security.Policy;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CustomJwtOptions>(builder.Configuration.GetRequiredSection("Jwt"))
    .Configure<CustomOAuthClientOptions>(
        KakaoOAuthDefaults.AuthenticationScheme,
        builder.Configuration.GetRequiredSection("OAuth:Kakao")
    );

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IConfigureOptions<OAuthOptions>, KakaoOAuthOptions>()
    .AddSingleton<IConfigureOptions<CookieAuthenticationOptions>, CustomCookieAuthOptions>()
    .AddSingleton<ISecureDataFormat<AuthenticationTicket>, CustomTicketDataFormat>()
    .AddSingleton<ITokenProvider, JwtTokenProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = KakaoOAuthDefaults.AuthenticationScheme;
    })
    .AddCookie(delegate { })
    .AddOAuth(KakaoOAuthDefaults.AuthenticationScheme, _ => { });

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicy(
        new[]
        {
            new RolesAuthorizationRequirement(new[] { RoleName.User })
        },
        new[]
        {
            CookieAuthenticationDefaults.AuthenticationScheme,
            KakaoOAuthDefaults.AuthenticationScheme
        }
    );
    
    options.AddPolicy(PolicyName.ResourcePolicy, policy =>
    {
        policy.Requirements.Add(new ResourceAuthorizationHandler());
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseHsts();

app.UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(routeBuilder => routeBuilder.MapControllers());

app.MapControllerRoute(
    "default",
    "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();