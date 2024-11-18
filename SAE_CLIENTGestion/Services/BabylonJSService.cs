using Microsoft.JSInterop;

public interface IBabylonJSService
{
    ValueTask InitializeSceneAsync(string canvasId);
}

public class BabylonJSService : IBabylonJSService
{
    private readonly IJSRuntime _jsRuntime;

    public BabylonJSService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async ValueTask InitializeSceneAsync(string canvasId)
    {
        await _jsRuntime.InvokeVoidAsync("babylonInterop.initializeScene", canvasId);
    }
}