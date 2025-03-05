using AspNetCoreRateLimit;
using Mayhem.LeaderBoardApi.Filters;
using Mayhem.ApplicationSetup;
using Mayhem.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

string CorsPolicy = nameof(CorsPolicy);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("V1", new OpenApiInfo
    {
        Version = "V1",
        Title = "WebAPI",
        Description = "Crypto Mayhem Leaderboard WebAPI"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Enter the Authorization string: `Generated-JWT-Token`",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;
    options.StackBlockedRequests = false;
    options.HttpStatusCode = 429;
    options.RealIpHeader = "X-Real-IP";
    options.ClientIdHeader = "X-ClientId";
    options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "*",
            Period = "60s",
            Limit = 100
        }
    };
});
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddInMemoryRateLimiting();

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy,
        builder => builder.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()
    .Build());
});

builder.Services.AddMvc(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});

builder.Configuration.ConfigureKeyVault();
string sqlConnectionString = builder.Configuration["SqlConnectionString"];
string JWTValidIssuer = builder.Configuration["JWTValidIssuer"];
string JWTValidAudience = builder.Configuration["JWTValidAudience"];
string JWTSecret = builder.Configuration["JWTSecret"];
string web3ProviderEndpoint = builder.Configuration["Web3ProviderEndpoint"];
string JWTDurationInMinutes = builder.Configuration["JWTDurationInMinutes"];
string adminLogin = builder.Configuration["AdminLogin"];
string adminPassword = builder.Configuration["AdminPassword"];

string alturaTournamentAbi = builder.Configuration["AlturaTournamentAbi"];
string alturaTournamentAddress = builder.Configuration["AlturaTournamentAddress"];
string alturaTournamentTicketId = builder.Configuration["AlturaTournamentTicketId"];
string privateKeyWallet = builder.Configuration["PrivateKeyWallet"];

builder.Services.AddSingleton(new MayhemConfiguration(
    web3ProviderEndpoint,
    sqlConnectionString, 
    JWTValidIssuer, 
    JWTValidAudience, 
    JWTSecret,
    JWTDurationInMinutes,
    adminLogin,
    adminPassword,
    alturaTournamentAbi,
    alturaTournamentAddress,
    alturaTournamentTicketId,
    privateKeyWallet));

builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = JWTValidIssuer,
            ValidAudience = JWTValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSecret))
        };
    });

builder.Services.AddMayhemContext(sqlConnectionString);
builder.Services.AddAutoMapperConfiguration();
builder.Services.AddBlockchain(web3ProviderEndpoint, privateKeyWallet);
builder.Services.AddServices();
builder.Services.AddRepository();
builder.Services.AddValidators();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/V1/swagger.json", "Crypto Mayhem Leaderboard WebAPI");
    });
}

app.UseIpRateLimiting();
app.UseHttpsRedirection();

app.UseCors(CorsPolicy);
app.UseAuthorization();

app.MapControllers();

app.Run();
