using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class PredictionService
{
    private readonly HttpClient _httpClient;

    public PredictionService(HttpClient httpClient)
    {
        _httpClient = new HttpClient{ BaseAddress = new Uri("http://localhost:5001")};
    }

    public async Task<Dictionary<string, object>> FetchPredictKnnAsync()
    {
        return await FetchPredictionAsync("/predict_knn");
    }

    public async Task<Dictionary<string, object>> FetchPredictionWithHorizonAsync(int horizon)
    {
        string apiUrl = $"/predict_horizon?horizon={horizon}";
        return await FetchPredictionAsync(apiUrl);
    }

    private async Task<Dictionary<string, object>> FetchPredictionAsync(string apiUrl)
    {
        try
        {
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Réponse JSON: {jsonResponse}");

                var PredictionData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);

                // Ajoutez un log pour vérifier que la désérialisation a bien fonctionné
                Console.WriteLine($"Données de prédiction : {PredictionData}");

                return PredictionData;
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
