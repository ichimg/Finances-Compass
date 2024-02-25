using DebtsCompass;
using DebtsCompass.Application.Configurations;
using DebtsCompass.Application.Jobs;
using DebtsCompass.Application.Services;
using DebtsCompass.Application.Validators;
using DebtsCompass.DataAccess;
using DebtsCompass.DataAccess.Repositories;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Interfaces;
using EmailSender;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<IdentityOptions>(opts =>
{
    opts.SignIn.RequireConfirmedEmail = true;
});

builder.Services.AddIdentity<User, IdentityRole>()
               .AddEntityFrameworkStores<DebtsCompassDbContext>()
               .AddDefaultTokenProviders();

builder.Services.AddAuthentication();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("TokenAuthentication", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"Bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference= new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="TokenAuthentication"
                },
            },
        new List<string>(){ }
        }
    });
});

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(config =>
    {
        config.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidIssuer = "",
            ValidateAudience = false,
            ValidAudience = "",
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Secret"]))
        };
    });

builder.Services.AddAuthorization();

// Add Services
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IDebtsService, DebtsService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IFriendshipsService, FriendshipsService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IPaypalService, PaypalService>();
builder.Services.AddScoped<EmailValidator>();
builder.Services.AddScoped<PasswordValidator>();

// Add Jobs
builder.Services.AddScoped<ICurrencyRatesJob, CurrencyRatesJob>();

// Add Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDebtAssignmentRepository, DebtAssignmentRepository>();
builder.Services.AddScoped<IFriendshipRepository, FriendshipRepository>();
builder.Services.AddScoped<INonUserRepository, NonUserRepository> ();
builder.Services.AddScoped<IDebtRepository, DebtRepository>();


// Email
var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<EmailTemplatesService>();

// Paypal Config
var paypalConfig = builder.Configuration.GetSection("PaypalConfiguration").Get<PaypalConfiguration>();
builder.Services.AddSingleton(paypalConfig);

builder.Services.AddHttpClient();


//Allow CORS

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(allowedOrigins)
               .AllowAnyHeader()
               .AllowAnyMethod()
               .WithExposedHeaders("X-Pagination");

    });
});

builder.Services.AddDbContext<DebtsCompassDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DebtsCompassConnectionString")));


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

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

app.UseCors();

app.Run();
