using Microsoft.JSInterop;
using System.Text.Json;
using SAE_CLIENTGestion.Models;
using SAE_CLIENTGestion.Services;
using Microsoft.Extensions.DependencyInjection;

public interface IBabylonJSService
{
    ValueTask InitializeSceneAsync(string canvasId);
    Task<List<Batiment>> GetBuildingsAsync();
    ValueTask DisposeSceneAsync();
}

public sealed class BabylonJSService : IBabylonJSService
{
    private static BabylonJSService _instance;
    private static readonly object _lock = new object();
    private readonly IJSRuntime _jsRuntime;
    private readonly IServiceProvider _serviceProvider;
    private List<Batiment> _cachedBuildings;
    private bool _isInitialized;

    public static BabylonJSService Initialize(IJSRuntime jsRuntime, IServiceProvider serviceProvider)
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new BabylonJSService(jsRuntime, serviceProvider);
                }
            }
        }
        return _instance;
    }

    private BabylonJSService(IJSRuntime jsRuntime, IServiceProvider serviceProvider)
    {
        _jsRuntime = jsRuntime;
        _serviceProvider = serviceProvider;
        _cachedBuildings = new List<Batiment>();
        _isInitialized = false;
    }

    public async Task<List<Batiment>> GetBuildingsAsync()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var batimentService = scope.ServiceProvider.GetRequiredService<IService<Batiment>>();

            if (!_cachedBuildings.Any())
            {
                _cachedBuildings = await batimentService.GetAllAsync();
            }
            return _cachedBuildings;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erreur lors de la récupération des bâtiments: {ex.Message}", ex);
        }
    }

    public async ValueTask InitializeSceneAsync(string canvasId)
    {
        try
        {
            // Attendre un court instant pour s'assurer que babylonInterop est initialisé
            await Task.Delay(100);  // Petit délai pour laisser le temps au JS de s'initialiser

            // Vérifier si babylonInterop existe et est correctement initialisé
            var isInitialized = await _jsRuntime.InvokeAsync<bool>("eval", @"
            typeof window.babylonInterop === 'object' && 
            typeof window.babylonInterop.initializeScene === 'function' &&
            typeof window.babylonInterop.disposeScene === 'function'
        ");

            if (!isInitialized)
            {
                throw new Exception("babylonInterop n'est pas correctement initialisé");
            }

            if (_isInitialized)
            {
                await _jsRuntime.InvokeVoidAsync("babylonInterop.disposeScene");
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

    public async ValueTask DisposeSceneAsync()
    {
        if (_isInitialized)
        {
            try
            {
                // Vérifie si la fonction existe
                var functionExists = await _jsRuntime.InvokeAsync<bool>("eval",
                    "typeof window.babylonInterop !== 'undefined' && typeof window.babylonInterop.disposeScene === 'function'");

                if (functionExists)
                {
                    await _jsRuntime.InvokeVoidAsync("babylonInterop.disposeScene");
                    _isInitialized = false;
                }
                else
                {
                    Console.WriteLine("Warning: babylonInterop.disposeScene is not available");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error disposing scene: {ex.Message}");
            }
        }
    }


}