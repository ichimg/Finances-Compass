using DebtsCompass.Application.Services;
using DebtsCompass.Application.Validators;
using DebtsCompass.DataAccess;
using DebtsCompass.DataAccess.Repositories;
using DebtsCompass.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAuthentication();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    //options.AddSecurityDefinition("TokenAuthentication", new OpenApiSecurityScheme
    //{
    //    Description = "Standard Authorization header using the Bearer scheme (\"Bearer {token}\")",
    //    In = ParameterLocation.Header,
    //    Name = "Authorization",
    //    Type = SecuritySchemeType.Http,
    //    Scheme = "Bearer"
    //});

    //options.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference= new OpenApiReference
    //            {
    //                Type=ReferenceType.SecurityScheme,
    //                Id="TokenAuthentication"
    //            },
    //        },
    //    new List<string>(){ }
    //    }
    //});
});

// Add Services
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<EmailValidator>();
builder.Services.AddScoped<PasswordValidator>();


// Add Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();


//Allow CORS

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(allowedOrigins)
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<DebtsCompassDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DebtsCompassConnectionString")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();
