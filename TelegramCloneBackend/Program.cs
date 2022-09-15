using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MediatR;
using Database.Contexts;
using Database.Models;
using Database.Repositories;
using MediatR.Handlers.Login;
using MediatR.JWT;
using TGBackend.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddCors(o => {
    o.AddDefaultPolicy(builder =>
    {
        builder
        .SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});
var connectionString = "Host=localhost;Port=5432;Database=Telegram;Username=postgres;Password=20612061";
builder.Services.AddEntityFrameworkNpgsql()
                .AddDbContext<ChatContext>(options => { options.UseNpgsql(connectionString); })
                .AddDbContext<UserContext>(options => { options.UseNpgsql(connectionString); });
builder.Services.AddMvc(option =>
{
    // Отключаем маршрутизацию конечных точек на основе endpoint-based logic из EndpointMiddleware
    // и продолжаем использование маршрутизации на основе IRouter. 
    option.EnableEndpointRouting = false;
    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser().RequireAuthenticatedUser().Build();
    option.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddMediatR(typeof(LoginHandler));

builder.Services
    .AddIdentityCore<User>()
    .AddEntityFrameworkStores<UserContext>()
    .AddSignInManager<SignInManager<User>>();

builder.Services.AddScoped<ChatRepository>()
    .AddScoped<UserRepository>();
builder.Services.AddScoped<IJwtGenerator, DefaultJwtGenerator>();
builder.Services.TryAddSingleton<ISystemClock, SystemClock>();

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"]));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(
        opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateAudience = false,
                ValidateIssuer = false,
            };
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller}/{action=Index}/{id?}");
app.MapHub<ChatHub>("hubs/notifications");
app.MapFallbackToFile("index.html");

app.Run();