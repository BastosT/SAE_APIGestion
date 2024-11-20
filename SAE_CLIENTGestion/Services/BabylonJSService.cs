using Microsoft.JSInterop;
using System.Text.Json;

public interface IBabylonJSService
{
    ValueTask InitializeSceneAsync(string canvasId);
    List<Building> GetBuildings();
}

public class BabylonJSService : IBabylonJSService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly List<Building> _buildings;

    public BabylonJSService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        _buildings = InitializeDefaultBuilding();
    }

    private List<Building> InitializeDefaultBuilding()
    {
        return new List<Building>
        {
            new Building
            {
                Name = "Bâtiment Principal",
                Rooms = new List<Room>
                {
                    new Room
                    {
                        Name = "Salle 101",
                        Walls = new RoomWalls
                        {
                            FrontWall = new Wall
                            {
                                Largeur = 575,
                                Hauteur = 270,
                                Equipements = new List<Equipment>
                                {
                                    new Equipment { Nom = "Fenetre 1", Type = EquipmentType.Window, Largeur = 100, Hauteur = 165, PositionX = 6, PositionY = 3 },
                                    new Equipment { Nom = "Vitre 1", Type = EquipmentType.DoorWindow, Largeur = 89, Hauteur = 161, PositionX = 125, PositionY = 6 },
                                    new Equipment { Nom = "Vitre 2", Type = EquipmentType.DoorWindow, Largeur = 89, Hauteur = 161, PositionX = 237, PositionY = 6 },
                                    new Equipment { Nom = "Fenetre 2", Type = EquipmentType.Window, Largeur = 100, Hauteur = 165, PositionX = 345, PositionY = 3 },
                                    new Equipment { Nom = "Vitre 3", Type = EquipmentType.DoorWindow, Largeur = 89, Hauteur = 161, PositionX = 482, PositionY = 6 },
                                    new Equipment { Nom = "Radiateur 1", Type = EquipmentType.Radiator, Largeur = 160, Hauteur = 80, PositionX = 34, PositionY = 180 },
                                    new Equipment { Nom = "Radiateur 2", Type = EquipmentType.Radiator, Largeur = 160, Hauteur = 80, PositionX = 256, PositionY = 180 }
                                }
                            },
                            EntranceWall = new Wall
                            {
                                Largeur = 575,
                                Hauteur = 270,
                                Equipements = new List<Equipment>
                                {
                                    new Equipment { Nom = "Porte", Type = EquipmentType.Door, Largeur = 93, Hauteur = 205, PositionX = 55, PositionY = 67 },
                                }
                            },
                            RightWall = new Wall
                            {
                                Largeur = 736,
                                Hauteur = 270,
                                Equipements = new List<Equipment>
                                {
                                    new Equipment { Nom = "Capteur 1", Type = EquipmentType.Sensor, Largeur = 15, Hauteur = 15, PositionX = 178, PositionY = 98 },
                                    new Equipment { Nom = "Capteur 2", Type = EquipmentType.Sensor, Largeur = 10, Hauteur = 10, PositionX = 585, PositionY = 161 }
                                }
                            },
                            LeftWall = new Wall
                            {
                                Largeur = 736,
                                Hauteur = 270,
                                Equipements = new List<Equipment>
                                {
                                    new Equipment { Nom = "Capteur 1", Type = EquipmentType.Sensor, Largeur = 10, Hauteur = 10, PositionX = 316, PositionY = 70 },
                                    new Equipment { Nom = "Capteur 2", Type = EquipmentType.Sensor, Largeur = 10, Hauteur = 10, PositionX = 662, PositionY = 156 }
                                }
                            }
                        }
                    },
                    new Room
                    {
                        Name = "Salle 102",
                        Walls = new RoomWalls
                        {
                            FrontWall = new Wall
                            {
                                Largeur = 575,
                                Hauteur = 270,
                            },
                            EntranceWall = new Wall
                            {
                                Largeur = 575,
                                Hauteur = 270,
                            },
                            RightWall = new Wall
                            {
                                Largeur = 736,
                                Hauteur = 270,
                            },
                            LeftWall = new Wall
                            {
                                Largeur = 736,
                                Hauteur = 270,
                            }
                        }
                    }
                }
            },
            new Building
            {
                Name = "Bâtiment Principal",
                Rooms = new List<Room>
                {
                    new Room
                    {
                        Name = "Salle 101",
                        Walls = new RoomWalls
                        {
                            FrontWall = new Wall
                            {
                                Largeur = 752,
                                Hauteur = 270,
                                Equipements = new List<Equipment>
                                {
                                    new Equipment { Nom = "Fenetre 1", Type = EquipmentType.Window, Largeur = 100, Hauteur = 165, PositionX = 6, PositionY = 3 },
                                    new Equipment { Nom = "Vitre 1", Type = EquipmentType.DoorWindow, Largeur = 89, Hauteur = 161, PositionX = 125, PositionY = 6 },
                                    new Equipment { Nom = "Vitre 2", Type = EquipmentType.DoorWindow, Largeur = 89, Hauteur = 161, PositionX = 237, PositionY = 6 },
                                    new Equipment { Nom = "Fenetre 2", Type = EquipmentType.Window, Largeur = 100, Hauteur = 165, PositionX = 345, PositionY = 3 },
                                    new Equipment { Nom = "Vitre 3", Type = EquipmentType.DoorWindow, Largeur = 89, Hauteur = 161, PositionX = 482, PositionY = 6 },
                                }
                            },
                            EntranceWall = new Wall
                            {
                                Largeur = 752,
                                Hauteur = 270,

                            },
                            RightWall = new Wall
                            {
                                Largeur = 950,
                                Hauteur = 270,

                            },
                            LeftWall = new Wall
                            {
                                Largeur = 950,
                                Hauteur = 270,

                            }
                        }
                    },
                    new Room
                    {
                        Name = "Salle 102",
                        Walls = new RoomWalls
                        {
                            FrontWall = new Wall
                            {
                                Largeur = 575,
                                Hauteur = 270,
                            },
                            EntranceWall = new Wall
                            {
                                Largeur = 575,
                                Hauteur = 270,
                            },
                            RightWall = new Wall
                            {
                                Largeur = 736,
                                Hauteur = 270,
                            },
                            LeftWall = new Wall
                            {
                                Largeur = 736,
                                Hauteur = 270,
                            }
                        }
                    }
                }
            }
        };
    }

    public List<Building> GetBuildings()
    {
        return _buildings;
    }

    public async ValueTask InitializeSceneAsync(string canvasId)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        string buildingsJson = JsonSerializer.Serialize(_buildings, options);
        await _jsRuntime.InvokeVoidAsync("babylonInterop.initializeScene", canvasId, buildingsJson);
    }
}

public class Building
{
    public string Name { get; set; }
    public List<Room> Rooms { get; set; }
}

public class Room
{
    public string Name { get; set; }
    public RoomWalls Walls { get; set; }
}

public class RoomWalls
{
    public Wall FrontWall { get; set; }
    public Wall EntranceWall { get; set; }
    public Wall LeftWall { get; set; }
    public Wall RightWall { get; set; }
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
    public EquipmentType Type { get; set; }
    public int Largeur { get; set; }
    public int Hauteur { get; set; }
    public int PositionX { get; set; }
    public int PositionY { get; set; }
}

public enum EquipmentType
{
    Window,     // Fenêtre
    DoorWindow, // Vitre
    Door,       // Porte
    Radiator,   // Radiateur
    Sensor,     // Capteur
    Other       // Autre équipement non spécifié
}