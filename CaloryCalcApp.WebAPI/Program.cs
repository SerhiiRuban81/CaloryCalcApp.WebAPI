using CaloryCalcApp.WebAPI.Data;
using CaloryCalcApp.WebAPI.Profiles;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string connStr = builder.Configuration.GetConnectionString("CaloriesContext")
    ?? throw new InvalidOperationException("Connection string 'CaloriesContext' not found!");
builder.Services.AddDbContext<CaloriesContext>(options => {
    options.UseSqlServer(connStr);
});


builder.Services.AddIdentityApiEndpoints<HealthyUser>()
    .AddEntityFrameworkStores<CaloriesContext>();

//builder.Services.AddAutoMapper(cfg => { }, typeof(ProductProfile),
//    typeof(DishProfile), typeof(HealthyUserProfile), typeof(HealthyUserDishProfile),
//    typeof(DishProductProfile));
builder.Services.AddAutoMapper(cfg => { });

builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Name = "Authorization"
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
}
    );
builder.Services.AddEndpointsApiExplorer();
// Adding Controllers with Views
builder.Services.AddControllersWithViews();
var app = builder.Build();

/////////////////////////////////////////
//using (IServiceScope scope = app.Services.CreateScope())
//{
//    IServiceProvider serviceProvider = scope.ServiceProvider;
//    await SeedData.Initialize(
//    serviceProvider,
//    app.Environment,
//    app.Configuration
//    );
//}



/////////////////////////////////////////
///
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseExceptionHandler("/Home/Error");
    //app.UseHsts();
}
app.MapIdentityApi<HealthyUser>();
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseDefaultFiles(); // Serve default files like index.html
app.UseStaticFiles(); // Serve static files from wwwroot

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(


);
