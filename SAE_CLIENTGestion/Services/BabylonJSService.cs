using Microsoft.JSInterop;
using System.Text.Json;
using SAE_CLIENTGestion.Models;
using SAE_CLIENTGestion.Services;

public interface IBabylonJSService
{
    ValueTask InitializeSceneAsync(string canvasId);
    Task<List<Batiment>> GetBuildingsAsync();
}

public class BabylonJSService : IBabylonJSService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IService<Batiment> _batimentService;
    private List<Batiment> _cachedBuildings;
    private bool _isInitialized = false;

    public BabylonJSService(
        IJSRuntime jsRuntime,
        IService<Batiment> batimentService)
    {
        _jsRuntime = jsRuntime;
        _batimentService = batimentService;
        _cachedBuildings = new List<Batiment>();
    }

    public async Task<List<Batiment>> GetBuildingsAsync()
    {
        try
        {
            // Rafraîchit le cache si nécessaire
            if (!_cachedBuildings.Any())
            {
                _cachedBuildings = await _batimentService.GetAllAsync();
            }
            return _cachedBuildings;
        }
        catch (Exception ex)
        {
            // Log l'erreur si nécessaire
            throw new Exception($"Erreur lors de la récupération des bâtiments: {ex.Message}", ex);
        }
    }

    private async Task CleanupAsync()
    {
        try
        {
            bool cleanupExists = await _jsRuntime.InvokeAsync<bool>(
                "eval",
                "typeof babylonInterop !== 'undefined' && typeof babylonInterop.cleanup === 'function'"
            );

            if (cleanupExists)
            {
                await _jsRuntime.InvokeVoidAsync("babylonInterop.cleanup");
            }
            _isInitialized = false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors du nettoyage de la scène: {ex.Message}");
        }
    }

    public async ValueTask InitializeSceneAsync(string canvasId)
    {
        try
        {
            if (_isInitialized)
            {
                await CleanupAsync();
            }

            var buildings = await GetBuildingsAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            string buildingsJson = JsonSerializer.Serialize(buildings, options);

            await _jsRuntime.InvokeVoidAsync("babylonInterop.initializeScene", canvasId, buildingsJson);
            _isInitialized = true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erreur lors de l'initialisation de la scène: {ex.Message}", ex);
        }
    }

    public void Dispose()
    {
        _ = CleanupAsync();
    }
}