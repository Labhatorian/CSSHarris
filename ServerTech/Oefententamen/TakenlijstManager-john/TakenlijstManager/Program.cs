using Microsoft.EntityFrameworkCore;
using TakenlijstManager.Data;
using TakenlijstManager.Data.Initializers;

var builder = WebApplication.CreateBuilder(args);

var connectionstring = builder.Configuration.GetConnectionString("TakenManagerDbConnection");

builder.Services.AddDbContext<TakenManagerDbContext>(options =>
{
    options.UseSqlServer(connectionstring);
});

builder.Services.AddCookiePolicy(options =>
{
    //options.Cookie.Expiration= TimeSpan.FromMinutes(3);
    //options.Cookie.SameSite = SameSiteMode.Strict;
    //options.Cookie.HttpOnly = true;
    //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddSession( options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else if (app.Environment.IsDevelopment()) { 
    app.UseSeedTakenlijst();//iedere keer wanneer we de webapplicatie starten wordt de seeder aangespoken
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Taken}/{action=Index}/{id?}");

app.Run();
