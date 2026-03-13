using CaloryCalcApp.Web.Data;
using CaloryCalcApp.Web.Profiles;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

string connStr = builder.Configuration.GetConnectionString("CaloriesContext")
    ?? throw new InvalidOperationException("Connection string 'CaloriesContext' not found!");

builder.Services.AddDbContext<CaloriesContext>(options => {
    options.UseSqlServer(connStr);
});

builder.Services.AddIdentity<HealthyUser, IdentityRole>()
    .AddEntityFrameworkStores<CaloriesContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(
    options => {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile(new ProductProfile());
    cfg.AddProfile(new DishProductProfile());
    cfg.AddProfile(new DishProfile());
    cfg.AddProfile(new HealthyUserDishProfile());
    cfg.AddProfile(new HealthyUserProfile());
    cfg.AddProfile(new RoleProfile());
}
);

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider serviceProvider = scope.ServiceProvider;
    await SeedData.Initialize(
    serviceProvider,
    app.Environment,
    app.Configuration
    );
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseDefaultFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
