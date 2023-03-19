using CSSHarris.Data;
using CSSHarris.Hubs;
using CSSHarris.Models;
using CSSHarris.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthorization(o =>
{
    // Only admin can access.
    o.AddPolicy("RequireAdminRole", p => p.RequireRole("Admin"));

    // Only Mod can access.
    o.AddPolicy("RequireModRole", p => p.RequireRole("Moderator", "Admin"));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options => builder.Configuration.Bind("CookieSettings", options))
    .AddJwtBearer(o =>
        {
            using var loggerFactory = LoggerFactory.Create(loggingBuilder =>
    loggingBuilder
        .SetMinimumLevel(LogLevel.Trace)
        .AddConsole());
            ILogger logger = loggerFactory.CreateLogger<Program>();

            o.Events = new JwtBearerEvents()
            {
                OnAuthenticationFailed = c =>
                {
                    logger.LogInformation(c.HttpContext.User.Identity.Name +  " has failed authentication");
                    return Task.CompletedTask;
                },

                OnChallenge = c =>
                {
                    logger.LogInformation(c.HttpContext.User.Identity.Name + " is OnChallenge");
                    return Task.CompletedTask;
                },

                OnMessageReceived = c =>
                {
                    logger.LogInformation(c.HttpContext.User.Identity.Name + " received a message");
                    return Task.CompletedTask;
                },

                OnTokenValidated = c =>
                {
                    logger.LogInformation(c.HttpContext.User.Identity.Name + " has validated token");
                    return Task.CompletedTask;
                }
            };
        });

builder.Services.AddSingleton<IAuthorizationService, DefaultAuthorizationService>();

//SignalR
builder.Services.AddSignalR(o =>
{
    o.EnableDetailedErrors = true;
});

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapHub<ChatHub>("/chatHub");
app.MapHub<FriendHub>("/friendHub");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
