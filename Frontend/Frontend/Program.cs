using Frontend.Components;
using Frontend.Repo;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Set Stripe secret key from configuration
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient("ApiCall", client =>
{
    client.BaseAddress = new Uri("http://10.133.51.109:6002/api/");
});
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IPlantRepo, PlantRepo>();
builder.Services.AddSingleton<IUserAuth, UserAuth>(); 
builder.Services.AddHttpClient();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAntiforgery();

app.Use(async (context, next) =>
{
    context.Response.Headers["Content-Security-Policy"] =
        "default-src 'self'; " +
        "script-src 'self' 'unsafe-inline' https://js.stripe.com https://m.stripe.network; " +
        "style-src 'self' 'unsafe-inline' https://m.stripe.network https://*.stripe.com; " +
        "img-src 'self' data: https://*.stripe.com; " +
        "connect-src 'self' http://localhost:* https://api.stripe.com https://m.stripe.network https://localhost:7192 ws://localhost:* wss://localhost:*; " +
        "frame-src https://js.stripe.com https://*.stripe.com;";
    await next();
});


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
