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

        Equipement radiateur1 = new Equipement { EquipementId = 1, Nom = "Radiateur 1", Longueur = 100, Hauteur = 80, PositionX = 34, PositionY = 180, TypeEquipement = typeRadiateur };
        Equipement radiateur2 = new Equipement { EquipementId = 2, Nom = "Radiateur 2", Longueur = 100, Hauteur = 80, PositionX = 256, PositionY = 180, TypeEquipement = typeRadiateur };

        Equipement fenetre1 = new Equipement { EquipementId = 3, Nom = "Fenetre 1", Longueur = 100, Hauteur = 165, PositionX = 6, PositionY = 3, TypeEquipement = typeFenetre };
        Equipement fenetre2 = new Equipement { EquipementId = 4, Nom = "Fenetre 2", Longueur = 100, Hauteur = 165, PositionX = 345, PositionY = 3, TypeEquipement = typeFenetre };

        Equipement vitre1 = new Equipement { EquipementId = 5, Nom = "Fenetre 1", Longueur = 89, Hauteur = 161, PositionX = 125, PositionY = 6, TypeEquipement = typeVitre };
        Equipement vitre2 = new Equipement { EquipementId = 6, Nom = "Fenetre 2", Longueur = 89, Hauteur = 161, PositionX = 237, PositionY = 6, TypeEquipement = typeVitre };
        Equipement vitre3 = new Equipement { EquipementId = 7, Nom = "Fenetre 3", Longueur = 89, Hauteur = 161, PositionX = 482, PositionY = 6, TypeEquipement = typeVitre };

        Equipement porte = new Equipement { EquipementId = 8, Nom = "Porte", Longueur = 93, Hauteur = 205, PositionX = 55, PositionY = 67, TypeEquipement = typePorte };


        Salle d101 = new Salle
        {
            SalleId = 1,
            Nom = "D101",
            Surface = 10,
            TypeSalle = typeSalle,
            MurFace = new Mur { Hauteur = 270, Longueur = 575, Nom = "Mur Face", Equipements = [fenetre1, fenetre2, vitre1, vitre2, vitre3, radiateur1, radiateur2] },
            MurEntree = new Mur { Hauteur = 270, Longueur = 575, Nom = "Mur Entree", Equipements = [porte] },
            MurDroite = new Mur { Hauteur = 270, Longueur = 736, Nom = "Mur Droite", Capteurs = [capteurDroite1, capteurDroite2] },
            MurGauche = new Mur { Hauteur = 270, Longueur = 736, Nom = "Mur Gauche", Capteurs = [capteurGauche1, capteurGauche2] },
        };

        Salle d102 = new Salle
        {
            SalleId = 2,
            Nom = "D102",
            Surface = 5,
            TypeSalle = typeSalle,
            MurFace = new Mur { Hauteur = 270, Longueur = 575, Nom = "Mur Face", Equipements = [new Equipement { Longueur = 571, Hauteur = 266, PositionX = 2, PositionY = 2, TypeEquipement = typeVitre}] },
            MurEntree = new Mur { Hauteur = 270, Longueur = 575, Nom = "Mur Entree", Equipements = [new Equipement { Longueur = 571, Hauteur = 266, PositionX = 2, PositionY = 2, TypeEquipement = typeVitre }] },
            MurDroite = new Mur { Hauteur = 270, Longueur = 736, Nom = "Mur Droite", Equipements = [new Equipement { Longueur = 732, Hauteur = 266, PositionX = 2, PositionY = 2, TypeEquipement = typeVitre }] },
            MurGauche = new Mur { Hauteur = 270, Longueur = 736, Nom = "Mur Gauche", Equipements = [new Equipement { Longueur = 732, Hauteur = 266, PositionX = 2, PositionY = 2, TypeEquipement = typeVitre }] },
        };

        var buildings = new List<Batiment>();
        Batiment b1 = new Batiment
        {
            BatimentId = 1,
            Nom = "Bat D",
            Adresse = "jsp",
            Salles = [d101, d102]
        };

        Batiment b2 = new Batiment
        {
            BatimentId = 1,
            Nom = "Bat D",
            Adresse = "jsp",
            Salles = [d102, d102]
        };
        buildings.Add(b1);
        buildings.Add(b2);

        return buildings;
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