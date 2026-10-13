using WebEmpresa.Components;
using WebEmpresa.Services;

var builder = WebApplication.CreateBuilder(args);

// Componentes Razor con render interactivo del lado del servidor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Conexión con la API (ApiTarea) para el módulo de Clientes
builder.Services.AddHttpClient<ClientesService>(c =>
{
    c.BaseAddress = new Uri("https://localhost:7266/");
});

// Mitigación: ocultar la cabecera "Server: Kestrel"
builder.WebHost.ConfigureKestrel(o => o.AddServerHeader = false);

var app = builder.Build();

// Mitigación: cabeceras de seguridad (CSP, nosniff, etc.)
app.Use(async (context, next) =>
{
    // OnStarting asegura que nuestra política es la última en escribirse
    context.Response.OnStarting(() =>
    {
        var h = context.Response.Headers;
        h["X-Content-Type-Options"] = "nosniff";
        h["Referrer-Policy"] = "no-referrer";
        h["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";
        h["Content-Security-Policy"] =
            "default-src 'self'; " +
            "script-src 'self'; " +
            "style-src 'self'; " +
            "img-src 'self' data:; " +
            "font-src 'self'; " +
            "connect-src 'self'; " +
            "object-src 'none'; " +
            "base-uri 'self'; " +
            "form-action 'self'; " +
            "frame-ancestors 'self'";
        return Task.CompletedTask;
    });
    await next();
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();