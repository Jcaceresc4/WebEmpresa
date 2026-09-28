using WebEmpresa.Components;
using WebEmpresa.Services;

var builder = WebApplication.CreateBuilder(args);

// Componentes Razor con render interactivo del lado del servidor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Conexión con la API (ApiTarea) para el módulo de Clientes
builder.Services.AddHttpClient<ClientesService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7266/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

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