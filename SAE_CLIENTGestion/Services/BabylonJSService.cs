using Microsoft.JSInterop;
using SAE_CLIENTGestion.Models;
using System.Text.Json;

public interface IBabylonJSService
{
    ValueTask InitializeSceneAsync(string canvasId);
    List<Batiment> GetBuildings();
}

public class BabylonJSService : IBabylonJSService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly List<Batiment> _buildings;

    public BabylonJSService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        _buildings = InitializeDefaultBuilding();
    }

    private List<Batiment> InitializeDefaultBuilding()
    {
        TypeSalle typeSalle = new TypeSalle { TypeSalleId = 1, Nom = "type salle", Description = "aucune" };

        Capteur capteurDroite1 = new Capteur { CapteurId = 1, Nom = "Capteur D1", DistanceChauffage = 0, DistanceFenetre = 0, DistancePorte = 0, EstActif = true, Longeur = 15, Hauteur = 15, PositionX = 178, PositionY = 98 };
        Capteur capteurDroite2 = new Capteur { CapteurId = 2, Nom = "Capteur D2", DistanceChauffage = 0, DistanceFenetre = 0, DistancePorte = 0, EstActif = true, Longeur = 10, Hauteur = 10, PositionX = 585, PositionY = 161 };
        
        Capteur capteurGauche1 = new Capteur { CapteurId = 3, Nom = "Capteur D1", DistanceChauffage = 0, DistanceFenetre = 0, DistancePorte = 0, EstActif = true, Longeur = 10, Hauteur = 10, PositionX = 316, PositionY = 70 };
        Capteur capteurGauche2 = new Capteur { CapteurId = 4, Nom = "Capteur D2", DistanceChauffage = 0, DistanceFenetre = 0, DistancePorte = 0, EstActif = true, Longeur = 10, Hauteur = 10, PositionX = 662, PositionY = 156 };


        TypeEquipement typeRadiateur = new TypeEquipement { TypeEquipementId = 1, Nom = "Radiateur" };
        TypeEquipement typeFenetre = new TypeEquipement { TypeEquipementId = 2, Nom = "Fenetre" };
        TypeEquipement typeVitre = new TypeEquipement { TypeEquipementId = 3, Nom = "Vitre" };
        TypeEquipement typePorte = new TypeEquipement { TypeEquipementId = 4, Nom = "Porte" };

        Equipement radiateur1 = new Equipement { EquipementId = 1, Nom = "Radiateur 1", Longueur = 100, Hauteur = 165, PositionX = 34, PositionY = 180, TypeEquipement = typeRadiateur };
        Equipement radiateur2 = new Equipement { EquipementId = 2, Nom = "Radiateur 2", Longueur = 100, Hauteur = 165, PositionX = 256, PositionY = 180, TypeEquipement = typeRadiateur };

        Equipement fenetre1 = new Equipement { EquipementId = 3, Nom = "Fenetre 1", Longueur = 100, Hauteur = 165, PositionX = 6, PositionY = 3, TypeEquipement = typeFenetre };
        Equipement fenetre2 = new Equipement { EquipementId = 4, Nom = "Fenetre 2", Longueur = 100, Hauteur = 165, PositionX = 345, PositionY = 3, TypeEquipement = typeFenetre };

        Equipement vitre1 = new Equipement { EquipementId = 5, Nom = "Fenetre 1", Longueur = 89, Hauteur = 161, PositionX = 125, PositionY = 6, TypeEquipement = typeVitre };
        Equipement vitre2 = new Equipement { EquipementId = 6, Nom = "Fenetre 2", Longueur = 89, Hauteur = 161, PositionX = 237, PositionY = 6, TypeEquipement = typeVitre };
        Equipement vitre3 = new Equipement { EquipementId = 7, Nom = "Fenetre 3", Longueur = 89, Hauteur = 161, PositionX = 482, PositionY = 6, TypeEquipement = typeVitre };

        Equipement porte = new Equipement { EquipementId = 8, Nom = "Porte", Longueur = 93, Hauteur = 205, PositionX = 55, PositionY = 67, TypeEquipement = typePorte };


        Salle d101 = new Salle
        {
            SalleId = 1,
            BatimentId = 1,
            Nom = "D101",
            Surface = 10,
            TypeSalle = typeSalle,
            MurFace = new Mur { Hauteur = 270, Longueur = 575, Nom = "Mur Face", Equipements = [fenetre1, fenetre2, vitre1, vitre2, vitre3, radiateur1, radiateur2] },
            MurEntree = new Mur { Hauteur = 270, Longueur = 575, Nom = "Mur Entree", Equipements = [porte] },
            MurDroite = new Mur { Hauteur = 270, Longueur = 736, Nom = "Mur Droite", Capteurs = [capteurDroite1, capteurDroite2] },
            MurGauche = new Mur { Hauteur = 270, Longueur = 736, Nom = "Mur Gauche", Capteurs = [capteurGauche1, capteurGauche2] },
        };

        var buildings = new List<Batiment>();
        Batiment b1 = new Batiment
        {
            BatimentId = 1,
            Nom = "Bat D",
            Adresse = "jsp",
            Salles = [d101]
        };
        buildings.Add(b1);

        return buildings;
    }

    private List<Building> InitializeDefaultBuildingCopy()
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
                                Equipements = new List<Equipment>
                                {
                                    new Equipment { Nom = "Radiateur 1", Type = EquipmentType.Radiator, Largeur = 160, Hauteur = 80, PositionX = 34, PositionY = 180 },
                                    new Equipment { Nom = "Radiateur 2", Type = EquipmentType.Radiator, Largeur = 160, Hauteur = 80, PositionX = 256, PositionY = 180 }
                                }
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
                                Equipements = new List<Equipment>
                                {
                                    new Equipment { Nom = "Fenetre", Type = EquipmentType.Window, Largeur = 571, Hauteur = 266, PositionX = 2, PositionY = 2 },
                                }
                            },
                            EntranceWall = new Wall
                            {
                                Largeur = 575,
                                Hauteur = 270,
                                Equipements = new List<Equipment>
                                {
                                    new Equipment { Nom = "Fenetre", Type = EquipmentType.Window, Largeur = 571, Hauteur = 266, PositionX = 2, PositionY = 2 },
                                }
                            },
                            RightWall = new Wall
                            {
                                Largeur = 736,
                                Hauteur = 270,
                                Equipements = new List<Equipment>
                                {
                                    new Equipment { Nom = "Fenetre", Type = EquipmentType.Window, Largeur = 736, Hauteur = 266, PositionX = 2, PositionY = 2 },
                                }
                            },
                            LeftWall = new Wall
                            {
                                Largeur = 736,
                                Hauteur = 270,
                                Equipements = new List<Equipment>
                                {
                                    new Equipment { Nom = "Fenetre", Type = EquipmentType.Window, Largeur = 736, Hauteur = 266, PositionX = 2, PositionY = 2 },
                                }
                            }
                        }
                    }
                }
            }
        };
    }

    public List<Batiment> GetBuildings()
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