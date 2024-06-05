using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ProjektGruppeAWebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ProjektGruppeWebApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using ProjektGruppeAWebApi.Services;
using ProjektGruppeAWebApi.Services.BackgroundServices;
using Microsoft.AspNetCore.Identity;
using ProjektGruppeAWebApi.Models;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ProjektGruppeAContext>(options =>
    {
        //Use Entity Framework Core with MySQL
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    
    });
builder.Services.AddIdentity<User, IdentityRole>()
      .AddEntityFrameworkStores<ProjektGruppeAContext>()
      .AddDefaultTokenProviders();

builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowOrigin",
            builder => builder.WithOrigins(
                "http://localhost:5173",
                "http://github",
                "http://172.22.93.49",
                "http://lebedev-systems.de")
                .AllowAnyHeader()
                .AllowAnyMethod());
    });
var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("Jwt:Key"));
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization(options => 
    {
        options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
        options.AddPolicy("FTPUserPolicy", policy => policy.RequireRole("FTPUser", "Admin"));
        options.AddPolicy("UserPolicy", policy => policy.RequireRole("User", "FTPUser", "Admin"));
    });

builder.Services.AddSingleton<IAuthorizationHandler, AuthorizationHandler>();
builder.Services.AddScoped<MigrationService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddHostedService<MigrationBackgroundService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowOrigin");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
