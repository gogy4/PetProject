using Microsoft.EntityFrameworkCore;
using PetProject.Data;
using PetProject.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var connection = builder.Configuration.GetConnectionString("MySqlConn");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(connection, ServerVersion.AutoDetect(connection));
});
var app = builder.Build();

app.MapDefaultControllerRoute();
app.Run();