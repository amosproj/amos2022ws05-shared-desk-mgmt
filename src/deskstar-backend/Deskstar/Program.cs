using System.Text;
using AutoMapper;
using Deskstar;
using Deskstar.Core;

using Deskstar.DataAccess;
using Deskstar.Models;
using Deskstar.Usecases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("IsCompanyAdmin"));
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => c.SchemaFilter<EnumSchemaFilter>());

builder.Configuration.AddEnvironmentVariables();
var dbHost = builder.Configuration.GetValue<string>(Constants.CONFIG_DB_HOST) ?? null;
var dbDatabase = builder.Configuration.GetValue<string>(Constants.CONFIG_DB_DATABASE) ?? null;
var dbUsername = builder.Configuration.GetValue<string>(Constants.CONFIG_DB_USERNAME) ?? null;
var dbPassword = builder.Configuration.GetValue<string>(Constants.CONFIG_DB_PASSWORD) ?? null;
if (dbHost == null || dbDatabase == null || dbUsername == null || dbPassword == null)
{
    Console.Error.WriteLine($"missing db configuration. database configuration has host({dbHost != null}), database name({dbDatabase != null}), username({dbUsername != null}), password({dbPassword != null})");
    return;
}

builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql($"Host={dbHost};Database={dbDatabase};Username={dbUsername};Password={dbPassword}"));
builder.Services.AddScoped<IAuthUsecases, AuthUsecases>();
builder.Services.AddScoped<IBookingUsecases, BookingUsecases>();
builder.Services.AddScoped<IUserUsecases, UserUsecases>();
builder.Services.AddScoped<IAutoMapperConfiguration, AutoMapperConfiguration>();

var app = builder.Build();
// global cors policy
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
