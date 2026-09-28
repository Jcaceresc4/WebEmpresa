using System.Net;
using System.Net.Http.Json;
using WebEmpresa.Models;

namespace WebEmpresa.Services;

public class ClientesService
{
    private const string Ruta = "api/Clientes";
    private readonly HttpClient _http;

    public ClientesService(HttpClient http) => _http = http;

    public async Task<(List<Clientes> Datos, string? Error)> ObtenerTodosAsync()
    {
        try
        {
            var lista = await _http.GetFromJsonAsync<List<Clientes>>(Ruta);
            return (lista ?? new List<Clientes>(), null);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            return (new List<Clientes>(), "No se pudo conectar con la API. Verifica que ApiTarea esté en ejecución.");
        }
    }

    public async Task<(bool Ok, string? Error)> CrearAsync(Clientes cliente)
    {
        try
        {
            var resp = await _http.PostAsJsonAsync(Ruta, cliente);
            return resp.IsSuccessStatusCode
                ? (true, null)
                : (false, $"No se pudo agregar el cliente (código {(int)resp.StatusCode}).");
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            return (false, "No se pudo conectar con la API.");
        }
    }

    public async Task<(bool Ok, string? Error)> ActualizarAsync(Clientes cliente)
    {
        try
        {
            var resp = await _http.PutAsJsonAsync($"{Ruta}/{cliente.Id_cliente}", cliente);
            if (resp.StatusCode == HttpStatusCode.NotFound)
                return (false, "El cliente ya no existe en la base de datos.");
            return resp.IsSuccessStatusCode
                ? (true, null)
                : (false, $"No se pudo actualizar el cliente (código {(int)resp.StatusCode}).");
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            return (false, "No se pudo conectar con la API.");
        }
    }

    public async Task<(bool Ok, string? Error)> EliminarAsync(int id)
    {
        try
        {
            var resp = await _http.DeleteAsync($"{Ruta}/{id}");
            if (resp.StatusCode == HttpStatusCode.NotFound)
                return (false, "El cliente ya no existe en la base de datos.");
            return resp.IsSuccessStatusCode
                ? (true, null)
                : (false, $"No se pudo eliminar el cliente (código {(int)resp.StatusCode}).");
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            return (false, "No se pudo conectar con la API.");
        }
    }
}