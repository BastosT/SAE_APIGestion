using Microsoft.JSInterop;
using System.Text.Json;

public interface IBabylonJSService
{
    ValueTask InitializeSceneAsync(string canvasId);
}

public class BabylonJSService : IBabylonJSService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly RoomData _roomData;

    public BabylonJSService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        _roomData = new RoomData
        {
            MurFace = new Wall
            {
                Largeur = 575,
                Hauteur = 270,
                Equipements = new List<Equipment>
                {
                    new Equipment { Nom = "Fenetre 1", Largeur = 100, Hauteur = 165, PositionX = 6, PositionY = 3 },
                    new Equipment { Nom = "Vitre 1", Largeur = 89, Hauteur = 161, PositionX = 125, PositionY = 6 },
                    new Equipment { Nom = "Vitre 2", Largeur = 89, Hauteur = 161, PositionX = 237, PositionY = 6 },
                    new Equipment { Nom = "Fenetre 2", Largeur = 100, Hauteur = 165, PositionX = 345, PositionY = 3 },
                    new Equipment { Nom = "Vitre 3", Largeur = 89, Hauteur = 161, PositionX = 482, PositionY = 6 },
                    new Equipment { Nom = "Radiateur 1", Largeur = 160, Hauteur = 80, PositionX = 0, PositionY = 0 },
                    new Equipment { Nom = "Radiateur 2", Largeur = 160, Hauteur = 80, PositionX = 256, PositionY = 128 }
                }
            }
        };
    }

    public async ValueTask InitializeSceneAsync(string canvasId)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        string roomDataJson = JsonSerializer.Serialize(_roomData, options);
        await _jsRuntime.InvokeVoidAsync("babylonInterop.initializeScene", canvasId, roomDataJson);
    }
}

public class RoomData
{
    public Wall MurFace { get; set; }
}

public class Wall
{
    public int Largeur { get; set; }
    public int Hauteur { get; set; }
    public List<Equipment> Equipements { get; set; }
}

public class Equipment
{
    public string Nom { get; set; }
    public int Largeur { get; set; }
    public int Hauteur { get; set; }
    public int PositionX { get; set; }
    public int PositionY { get; set; }
}