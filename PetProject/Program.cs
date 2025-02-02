using Microsoft.AspNetCore.Authentication.Cookies;
using PetProject.Data;
using PetProject.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<PasteService>();
builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<LogInService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole(); 
builder.Logging.AddDebug();   
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