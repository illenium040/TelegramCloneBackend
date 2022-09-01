using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TelegramCloneBackend.Database.Contexts;
using TelegramCloneBackend.Database.Repositories;
using TelegramCloneBackend.Hubs;

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
builder.Services.AddMvc();

builder.Services
    .AddScoped<ChatRepository>()
    .AddScoped<UserRepository>();

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
