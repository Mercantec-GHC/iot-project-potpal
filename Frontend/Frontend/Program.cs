using Frontend.Components;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Set Stripe secret key from configuration
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient("ApiCall", client =>
{
    //client.BaseAddress = new Uri("http://10.133.51.109:6002/api/");
    client.BaseAddress = new Uri("https://localhost:7192/api/");
});
builder.Services.AddHttpClient();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Detailed error page for development
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
        "script-src 'self' https://js.stripe.com https://m.stripe.network " +
        "'sha256-5DA+a07wxWmEka9IdoWjSPVHb17Cp5284/lJzfbl8KA=' " +
        "'sha256-/5Guo2nzv5n/w6ukZpOBZOtTJBJPSkJ6mhHpnBgm3Ls='; " +
        "style-src 'self' https://m.stripe.network 'unsafe-inline'; " +
        "img-src 'self' data: https://*.stripe.com; " +
        "connect-src 'self' https://api.stripe.com https://m.stripe.network; " +
        "frame-src https://js.stripe.com;";
    await next();
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
