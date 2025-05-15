using Frontend.Components;
using PotPalFrontend.Repo;
using PotPalFrontend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient();

builder.Services.AddHttpClient("ApiCall", client =>
{
    client.BaseAddress = new Uri("http://localhost:5187/");
});

builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<UserRepo>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
