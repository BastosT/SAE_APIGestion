using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.Json.Nodes;

public class PredictionService
{
    private readonly HttpClient _httpClient;

    public PredictionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    // Nouvelle méthode pour récupérer les données de température et inclure les prédictions
    public async Task<List<Dictionary<string, object>>> FetchTemperatureDataAsync()
    {
        try
        {
            // Appel de l'API pour récupérer les données de température
            var jsonResponse = await _httpClient.GetStringAsync("http://51.83.36.122:5005/temperature_data");

            // Désérialisation du JSON dans un JsonObject
            var result = JsonSerializer.Deserialize<JsonObject>(jsonResponse);

            // Vérifier si "data_by_sensor" est dans la réponse
            if (result.ContainsKey("data_by_sensor"))
            {
                var dataBySensor = result["data_by_sensor"];

                // Vérifier si "9_in_1_multi_sensor_air_temperature" existe dans les données
                if (dataBySensor is JsonObject sensorData && sensorData.ContainsKey("9_in_1_multi_sensor_air_temperature"))
                {
                    // Récupérer les données de température
                    var sensorTemperatureData = sensorData["9_in_1_multi_sensor_air_temperature"] as JsonArray;

                    // Liste pour stocker les résultats
                    var responseList = new List<Dictionary<string, object>>();

                    // Boucler sur chaque élément et ajouter les températures au response
                    foreach (var item in sensorTemperatureData)
                    {
                        if (item is JsonObject temperatureData)
                        {
                            if (temperatureData.ContainsKey("temperature"))
                            {
                                // Créer un dictionnaire pour chaque élément de température non prédit
                                var responseItem = new Dictionary<string, object>
                                {
                                    { "sensor", "9_in_1_multi_sensor_air_temperature" },  // Nom du capteur
                                    { "isPredicted", false },  // Non prédiction
                                    { "temperature", temperatureData["temperature"].GetValue<double>() },  // Température
                                    { "time", temperatureData["time"] }
                                };

                                // Ajouter l'élément à la liste de réponse
                                responseList.Add(responseItem);
                            }
                        }
                    }

                    // Récupérer les prédictions
                    var pred = await _httpClient.GetStringAsync("http://185.218.125.75:5001/predict_horizon?horizon=10");
                    var predictions = JsonSerializer.Deserialize<JsonObject>(pred);

                    // Boucler sur les prédictions et les ajouter à responseList
                    if (predictions.ContainsKey("predictions"))
                    {
                        var predictionArray = predictions["predictions"] as JsonArray;
                        if (predictionArray != null)
                        {
                            foreach (var predItem in predictionArray)
                            {
                                if (predItem is JsonObject predictionData && predictionData.ContainsKey("temperature"))
                                {
                                    // Ajouter un élément prédit à la liste de réponse
                                    var predictedItem = new Dictionary<string, object>
                {
                    { "sensor", "9_in_1_multi_sensor_air_temperature" },  // Nom du capteur
                    { "isPredicted", true },  // Prédiction
                    { "temperature", predictionData["temperature"].GetValue<double>() },  // Température prédit
                    { "time", predictionData["time"].GetValue<double>() }  // Time en timestamp
                };
                                    // Ajouter l'élément prédit à la liste de réponse
                                    responseList.Add(predictedItem);
                                }
                            }
                        }
                    }

                    // Retourner la liste des objets avec les prédictions ajoutées
                    return responseList;
                }
            }

            throw new Exception("Données de température non trouvées dans la réponse.");
        }
        catch (Exception ex)
        {
            throw new Exception($"Erreur lors de la récupération des données de température : {ex.Message}");
        }
    }





    private async Task<Dictionary<string, object>> FetchPredictionAsync(string apiUrl)
    {
        try
        {
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);

                // Ajouter un indicateur de prédiction pour l'ensemble des prédictions
                return result;
            }
            else
            {
                throw new HttpRequestException($"Erreur : {response.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Une erreur est survenue : {ex.Message}");
        }
    }
}
