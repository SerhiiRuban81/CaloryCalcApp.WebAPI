using CaloryCalcApp.WebAPI.Data;
using CaloryCalcApp.WebAPI.Profiles;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);


// MVC + Views
// Add services to the container.
//builder.Services.AddControllers();
// Adding Controllers with Views
builder.Services.AddControllersWithViews();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


string connStr = builder.Configuration.GetConnectionString("CaloriesContext")
    ?? throw new InvalidOperationException("Connection string 'CaloriesContext' not found!");
// DB CONTEXT
builder.Services.AddDbContext<CaloriesContext>(options => {
    options.UseSqlServer(connStr);
});

// Identity
//builder.Services.AddIdentityApiEndpoints<HealthyUser>()
//    .AddEntityFrameworkStores<CaloriesContext>();
builder.Services.AddIdentity<HealthyUser, IdentityRole>()
    .AddEntityFrameworkStores<CaloriesContext>()
    .AddDefaultTokenProviders();

// Customize Identity cookie
builder.Services.ConfigureApplicationCookie(
    options => {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

//builder.Services.AddSwaggerGen(options => {
//    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.ApiKey,
//        Name = "Authorization"
//    });
//    options.OperationFilter<SecurityRequirementsOperationFilter>();
//    }
// );


// Adding AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile(new ProductProfile());
    cfg.AddProfile(new DishProductProfile());
    cfg.AddProfile(new DishProfile());
    cfg.AddProfile(new HealthyUserDishProfile());
    cfg.AddProfile(new HealthyUserProfile());
}
);

//// Let's configure our Services to make sure that User Authentificated before getting access to the app
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = "Cookies";
//    options.DefaultChallengeScheme = "Cookies";
//})
// .AddCookie("Cookies", options =>
// {
//     options.LoginPath = "/Account/Login"; // Path to the login page
//     options.LogoutPath = "/Account/Logout"; // Path to the logout page
//     options.AccessDeniedPath = "/Account/AccessDenied"; // Path to the access denied page
// });

var app = builder.Build();

/////////////////////////////////////////
/// LET'S INITIALIZE OUR DATABASE WITH STARTING DATA ON CREATION
/////////////////////////////////////////
using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider serviceProvider = scope.ServiceProvider;
    await SeedData.Initialize(
    serviceProvider,
    app.Environment,
    app.Configuration
    );
}


/////////////////////////////////////////
///
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//    //app.UseExceptionHandler("/Home/Error");
//    //app.UseHsts();
//}

//app.MapIdentityApi<HealthyUser>();
app.UseHttpsRedirection();
app.UseStaticFiles(); // Serve static files from wwwroot

app.UseDefaultFiles(); // Serve default files like index.html

app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();
app.MapControllerRoute(
    name: "default",
    //pattern: "{controller=Home}/{action=Index}/{id?}");
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();