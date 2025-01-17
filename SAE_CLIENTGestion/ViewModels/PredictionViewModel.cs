using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Text.Json;

public class PredictionViewModel
{
    private readonly PredictionService _predictionService;

    public int Horizon { get; set; } = 10;
    public ObservableCollection<TemperaturePredictionModel> AllData { get; private set; } // Liste des données et prédictions
    public bool IsLoading { get; private set; }
    public string ErrorMessage { get; private set; }

    public PredictionViewModel(PredictionService predictionService)
    {
        _predictionService = predictionService;
        AllData = new ObservableCollection<TemperaturePredictionModel>(); // Initialiser la collection
    }

    // Méthode combinée pour récupérer les données de température et les prédictions
    public async Task FetchAllDataAsync()
    {
        IsLoading = true;
        ErrorMessage = string.Empty;
        AllData.Clear(); // Vider la collection avant d'ajouter les nouvelles données

        try
        {
            // Récupérer les données de température et les prédictions via le service
            var temperatureData = await _predictionService.FetchTemperatureDataAsync();

            // Ajouter les données de température actuelles et les prédictions
            foreach (var data in temperatureData)
            {
                try
                {
                    Console.WriteLine(data["isPredicted"]);
                    // Créer l'objet TemperaturePredictionModel
                    var temperaturePredictionModel = new TemperaturePredictionModel
                    {
                        // Vérifier si les données sont prédites ou non
                        IsPredicted = data["isPredicted"] != null && (bool)data["isPredicted"],
                        Temperature = data["temperature"].ToString(),

                        // Extraire la valeur de 'time' en utilisant GetValue<double>()
                        Time = ConvertTimeToDateTime(data["time"] as JsonNode) // Conversion de "time" en JsonNode si possible
                    };

                    // Ajouter le modèle à la collection
                    AllData.Add(temperaturePredictionModel); // Ajouter à la collection ObservableCollection
                }
                catch (Exception innerEx)
                {
                    // Si une erreur se produit dans le boucle, afficher l'exception spécifique
                    Console.WriteLine("Erreur lors de l'ajout de la donnée : " + innerEx.Message);
                }
            }
        }
        catch (Exception ex)
        {
            // Afficher l'exception globale si l'appel API échoue
            ErrorMessage = ex.Message;
            Console.WriteLine("Erreur générale : " + ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    // Méthode pour convertir "time" en DateTime avec gestion des erreurs
    private DateTime ConvertTimeToDateTime(JsonNode timeNode)
    {
        try
        {
            // Extraire la valeur de 'time' en utilisant GetValue<double>()
            double time = timeNode?.GetValue<double>() ?? 0;  // Par défaut, 0 si null

            // Conversion du timestamp Unix avec fractions
            return DateTimeOffset.FromUnixTimeMilliseconds((long)(time * 1000)).DateTime;
        }
        catch (Exception ex)
        {
            // Gérer toute exception sur la conversion du time
            Console.WriteLine("Erreur lors de la conversion de time: " + ex.Message);
            throw;
        }
    }



}


// Modèle pour afficher les données de température et les prédictions
public class TemperaturePredictionModel
{
    public bool IsPredicted { get; set; }
    public string Temperature { get; set; }
    public DateTime Time { get; set; }
}
