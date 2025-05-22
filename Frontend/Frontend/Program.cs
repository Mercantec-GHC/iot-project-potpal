using Frontend.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient("ApiCall", client =>
{
<<<<<<< HEAD
    client.BaseAddress = new Uri("http://10.133.51.109:6002/api/"); 
=======
    //client.BaseAddress = new Uri("http://10.133.51.109:6002/api/"); 
    client.BaseAddress = new Uri("https://localhost:7192/api/"); 
>>>>>>> parent of f590a74 (Revert "Simon")
});
builder.Services.AddHttpClient();

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
