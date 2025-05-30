using Common.Core.Authentication;
using Common.Core.CQRS;
using Common.Core.DependencyInjection;
using Infra.Core.Middlewares;
using Infra.Database;
using MessageQueue.RabbitMQ.Extentions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSerilog(
    (configure) => 
        configure.ReadFrom.Configuration(builder.Configuration));

var connectionString = builder.Configuration.GetConnectionString("IdentityDatabase");
builder.Services.AddDbContextPool<IdDbContext>(
    options => options.UseSqlServer(
        connectionString,
        b => b.MigrationsAssembly("Infra.Database"))
);

builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowPolicy", builder => builder.AllowAnyOrigin().AllowAnyHeader());
});

builder.Services.AddDataProtection();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtCusScheme(builder.Configuration.GetSection("JWT").Get<JWTOptions>()!);

builder.Services.AddRabbitMQBus(builder.Configuration);

builder.Services.AddMemoryCache();

builder.Services
    .RegisterDomain("AuthService", "Infra.Database", "MessageQueue.RabbitMQ", "Infra.Shared.Core", "Infra.Core", "Application.Services")
    .RegisterNotifications("Application.Services");

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowPolicy");
app.MapControllers();

app.UseMiddleware<RequestArrivalMiddleware>();

app.Run();
