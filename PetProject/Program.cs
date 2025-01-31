using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PetProject.Data;
using PetProject.Models;
using PetProject.Services;
using PetProject.Utils;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<PasteService>();
builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<LogInService>();

// Настройка логирования
builder.Logging.ClearProviders(); // Удалить все стандартные провайдеры
builder.Logging.AddConsole(); // Добавить консольный логер
builder.Logging.AddDebug();   // Добавить логирование для отладки
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "UserCookie";
        options.Cookie.HttpOnly = true; 
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; 
        options.Cookie.SameSite = SameSiteMode.Strict; 
        options.LoginPath = "/login"; 
        options.AccessDeniedPath = "/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
    });

var connection = builder.Configuration.GetConnectionString("MySqlConn");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(connection, ServerVersion.AutoDetect(connection));
});

builder.Services.AddHostedService<CleanupService>();
var app = builder.Build();

app.MapDefaultControllerRoute();
app.UseStaticFiles();
app.Run();