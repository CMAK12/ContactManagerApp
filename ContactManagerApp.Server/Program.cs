using ContactManagerApp.Core.Data;
using ContactManagerApp.Core.Stores;
using ContactManagerApp.Infrastructure.Services;
using ContactManagerApp.Infrastructure.Services.Interfaces;
using ContactManagerApp.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddScoped<IContactService, ContactService>();

// Repositories
builder.Services.AddScoped<IContactStore, ContactRepository>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ContactManagerSql"));
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Contacts}/{action=Index}/{id?}");

app.Run();