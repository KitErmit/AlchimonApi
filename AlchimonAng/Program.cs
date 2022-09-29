using AlchimonAng.Accessors;
using AlchimonAng.DB.Repository;
using AlchimonAng.Helpers;
using AlchimonAng.Models;
using AlchimonAng.Providers;
using AlchimonAng.Services;
using AlchimonAng.Utils.Configs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AlchimonAng.DB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // указывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = AuthOptions.ISSUER,
            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = AuthOptions.AUDIENCE,
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            // установка ключа безопасности
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };
    });
builder.Services.AddScoped<IUserService, SimpleUserService>();
builder.Services.AddSingleton<ISaveLoader<Dictionary<string, Player>>, JsonLoader<Dictionary<string, Player>>>();
builder.Services.AddSingleton<ISaveLoader<Alchemon>, JsonLoader<Alchemon>>();
builder.Services.AddScoped<IPlayerRepository, PstgrPlayerRepository>();
builder.Services.AddScoped<IAdminService, SimpleAdminService>();
builder.Services.AddScoped<TestService>();
builder.Services.AddScoped<RegistrationService>();
builder.Services.AddSingleton<HashHepler>();
builder.Services.AddSingleton<JwtProvider>();
builder.Services.AddSingleton<UserContextAccessor>();
builder.Services.AddDbContext<AlBdContext>();
builder.Services.Configure<AdminConfig>(builder.Configuration.GetSection("Admin"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCors(builder => builder
    .SetIsOriginAllowed(_ => true)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");


app.Run();

public class AuthOptions
{
    public const string ISSUER = "MyAuthServer"; // издатель токена
    public const string AUDIENCE = "MyAuthClient"; // потребитель токена
    const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}