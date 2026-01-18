using Common.Core.Authentication;
using Common.Core.CQRS;
using Common.Core.DependencyInjection;
using Infra.Core.Authorization;
using Infra.Core.Middlewares;
using Infra.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Database
var connectionString = builder.Configuration.GetConnectionString("IdentityDatabase");
builder.Services.AddDbContextPool<IdDbContext>(
    options => options.UseSqlServer(
        connectionString,
        b => b.MigrationsAssembly("Infra.Database"))
);

// Add CORS to allow cross domain query
builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowPolicy", builder => builder.AllowAnyOrigin().AllowAnyHeader());
});

// Add Serilog Support
builder.Services.AddSerilog(
    (configure) =>
        configure.ReadFrom.Configuration(builder.Configuration));

// Add Authentication
builder.Services.AddDataProtection();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtCusScheme(builder.Configuration.GetSection("JWT").Get<JWTOptions>()!);
builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("AppPolicy", policy => 
    {
        policy.RequireAssertion(AuthorizationEx.VerifyAppName);
    });
});

// Add Local Cache Support
builder.Services.AddMemoryCache();

// Add JWT Options
builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));

// Register Services
builder.Services
    .RegisterDomain("AuthService", "Infra.Database", "Infra.Shared.Core", "Infra.Core", "Application.Services", "Application.Gateway")
    .RegisterNotifications("Application.Services");

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowPolicy");
app.MapControllers();

// Generate TraceId
app.UseMiddleware<RequestArrivalMiddleware>();

app.Run();
