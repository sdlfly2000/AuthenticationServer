using AuthService;
using AuthService.Actions;
using Infra.Database;
using Infra.Database.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(
//option => { 
//    var scheme = new OpenApiSecurityScheme()
//    {
//        Description = "Authorization header. \r\nExample: 'Bearer 12345abcdef'",
//        Reference = new OpenApiReference
//        {
//            Type = ReferenceType.SecurityScheme,
//            Id = "Authorization"
//        },
//        Scheme = "oauth2",
//        Name = "Authorization",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.ApiKey,
//    };
//    option.AddSecurityDefinition("Authorization", scheme);
//    var requirement = new OpenApiSecurityRequirement();
//    requirement[scheme] = new List<string>();
//    option.AddSecurityRequirement(requirement);
//}
);
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
builder.Services.AddIdentityCore<UserEntity>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
});

var idBuilder = new IdentityBuilder(typeof(UserEntity), typeof(RoleEntity), builder.Services);
idBuilder.AddEntityFrameworkStores<IdDbContext>()
    .AddDefaultTokenProviders()
    .AddRoleManager<RoleManager<RoleEntity>>()
    .AddUserManager<UserManager<UserEntity>>()
    .AddSignInManager<SignInManager<UserEntity>>();

builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtCusScheme(builder.Configuration.GetSection("JWT").Get<JWTOptions>()!);

builder.Services.AddMemoryCache();
builder.Services.AddTransient<IGenerateJWTAction, GenerateJWTAction>();
builder.Services.AddTransient<IAuthenticateAction, AuthenticateAction>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowPolicy");
app.MapControllers();

app.Run();
