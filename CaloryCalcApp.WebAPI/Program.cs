using CaloryCalcApp.WebAPI.Data;
using CaloryCalcApp.WebAPI.Extensions;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string connStr = builder.Configuration.GetConnectionString("LocalDb")
    ?? throw new InvalidOperationException("You should provide connection string!");
builder.Services.AddDbContext<CaloriesContext>(options => {
    options.UseSqlServer(connStr);
});

builder.Services.AddIdentityApiEndpoints<HealthyUser>()
    .AddEntityFrameworkStores<CaloriesContext>();
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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.CustomMapIdentityApi<HealthyUser>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
