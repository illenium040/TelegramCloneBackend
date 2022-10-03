using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DatabaseLayer.Contexts;
using DatabaseLayer.Models;
using DatabaseLayer.Repositories;
using MediatR.Handlers.Login;
using MediatR.JWT;
using TGBackend.Hubs;
using CQRSLayer.JWT;
using TelegramCloneBackend;
using DatabaseLayer.Repositories.Base;
using EFCoreSecondLevelCacheInterceptor;
using EasyCaching.InMemory;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json");
#if !DEBUG
builder.Configuration.AddKeyPerFile(directoryPath: "/etc/secrets", optional: true);
#endif

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORS", builder =>
    {
        builder.SetIsOriginAllowed((url) =>
                {
                    var uri = new Uri(url);
                    return uri.Host == "localhost" || uri.Host == "127.0.0.1";
                })
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()
              .WithOrigins("https://tgfrontend.onrender.com");
    });
});
#if DEBUG
var connectionString = "Host=localhost;Port=5432;Database=Telegram;Username=postgres;Password=20612061";
#else
var connectionString = builder.Configuration["ConnectionStringServer"];
#endif
builder.Services.AddEntityFrameworkNpgsql()
                .AddDbContext<ChatContext>(options => { options.UseNpgsql(connectionString); })
                .AddDbContext<UserContext>(options => { options.UseNpgsql(connectionString); });
builder.Services.AddMvc(option =>
{
    option.EnableEndpointRouting = false;
    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser().Build();
    option.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddMediatR(typeof(LoginHandler));

builder.Services
    .AddIdentityCore<User>((options) =>
    {
        options.User.AllowedUserNameCharacters =
        "¿‡¡·¬‚√„ƒ‰≈Â®∏∆Ê«Á»Ë…È ÍÀÎÃÏÕÌŒÓœÔ–—Ò“Ú”Û‘Ù’ı÷ˆ◊˜ÿ¯Ÿ˘⁄˙€˚‹¸›˝ﬁ˛ﬂˇabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    })
    .AddEntityFrameworkStores<UserContext>()
    .AddSignInManager<SignInManager<User>>();

builder.Services.AddScoped<PrivateChatRepository>()
    .AddScoped<IUserRepository, UserRepository>()
    .AddScoped<IUserChatRepository, UserChatRepository>()
    .AddScoped<IChatRepository, PrivateChatRepository>()
    .AddScoped<IMessagingRepository, UserChatRepository>()
    .AddScoped<IConnectionRepository, UserRepository>();

builder.Services.AddScoped<IJwtGenerator, DefaultJwtGenerator>();
builder.Services.TryAddSingleton<ISystemClock, SystemClock>();

#if DEBUG
var token = builder.Configuration["TokenKey"];
#else
var token = builder.Configuration["TokenKeyServer"];
#endif
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(
        opt =>
        {
            opt.SaveToken = true;
            opt.RequireHttpsMetadata = false;
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

const string providerName1 = "InMemory1";
builder.Services.AddEFSecondLevelCache(options =>
                options.UseEasyCachingCoreProvider(providerName1, false).UseCacheKeyPrefix("EF_"));

builder.Services.AddEasyCaching(options =>
{
    // use memory cache with your own configuration
    options.UseInMemory(config =>
    {
        config.DBConfig = new InMemoryCachingOptions
        {
            // scan time, default value is 60s
            ExpirationScanFrequency = 60,
            // total count of cache items, default value is 10000
            SizeLimit = 100,

            // enable deep clone when reading object from cache or not, default value is true.
            EnableReadDeepClone = false,
            // enable deep clone when writing object to cache or not, default value is false.
            EnableWriteDeepClone = false,
        };
        // the max random second will be added to cache's expiration, default value is 120
        config.MaxRdSecond = 120;
        // whether enable logging, default is false
        config.EnableLogging = true;
        // mutex key's alive time(ms), default is 5000
        config.LockMs = 5000;
        // when mutex key alive, it will sleep some time, default is 300
        config.SleepMs = 300;
    }, providerName1);
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseMiddleware<JwtMiddleware>();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("CORS");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller}/{action=Index}/{id?}");
app.MapHub<ChatHub>("hubs/notifications");
app.MapFallbackToFile("index.html");

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var repo = scope.ServiceProvider.GetRequiredService<IUserChatRepository>();
    DbSeed.SeedUsers(userManager, repo);
}

app.Run();