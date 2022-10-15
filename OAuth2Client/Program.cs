using Microsoft.AspNetCore.Authentication;
using OAuth2Client.Authentication;
using OAuth2Client.Authentication.Jwt;
using OAuth2Client.Authentication.OAuth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ISecureDataFormat<AuthenticationTicket>, CustomTicketDataFormat>();
builder.Services.AddTransient<ITokenProvider, JwtTokenProvider>();
builder.Services.AddOptions<CustomJwtOptions>()
    .Bind(builder.Configuration.GetSection("Jwt"));
builder.Services.AddOptions<CustomOAuthOptions>()
    .Bind(builder.Configuration.GetSection("OAuth:Kakao"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    "default",
    "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();