using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebLabRest.Models;
using WebLabRest.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHttpClient<IProductService, ApiProductService>(client => 
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrl"]);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
    });

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("admin", p => 
        p.RequireClaim(ClaimTypes.Role, "admin"));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();