using Practica_2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<ListasDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "YourAppCookieName";
        options.Cookie.SameSite = SameSiteMode.None; // Adjust as needed
        options.Cookie.HttpOnly = true; // Ensure the cookie is only accessed via HTTP(S)
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensure the cookie is only sent over HTTPS
        options.Cookie.IsEssential = true;

        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Adjust as needed
        options.SlidingExpiration = true; // Extend the expiration time with each request

        options.LoginPath = "/Home/Login"; // Specify the login page
        options.AccessDeniedPath = "/Home/AccessDenied"; // Specify the access denied page
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
