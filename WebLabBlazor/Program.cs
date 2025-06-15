using WebLabBlazor.Components;
using WebLabBlazor.Services;
using WebLabRest.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddPolicy("AllowBlazor", policy => 
{
    policy.WithOrigins("https://localhost:5001") 
        .AllowAnyHeader()
        .AllowAnyMethod();
}));

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddHttpClient<IProductService<Dish>, ApiProductService>(c =>
        c.BaseAddress = new Uri("https://localhost:7002/api/dishes"));


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); 

app.UseRouting();
app.UseCors("AllowBlazor");
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();