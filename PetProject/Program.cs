using Microsoft.EntityFrameworkCore;
using PetProject.Data;
using PetProject.Models;
using PetProject.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<PasteService>();
builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<EditUserService>();
var connection = builder.Configuration.GetConnectionString("MySqlConn");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(connection, ServerVersion.AutoDetect(connection));
});
builder.Services.AddHostedService<CleanupService>();
var app = builder.Build();

app.MapDefaultControllerRoute();
app.Run();