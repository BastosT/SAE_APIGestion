using SAE_CLIENTGestion.Services;
using System.Net.Http.Json;
using SAE_CLIENTGestion.Models;

namespace TD1Client.Services
{
    public class WSServiceCapteur : IService<Capteur>
    {
        private readonly HttpClient _httpClient;

        private string url = "api/Capteur";

        public WSServiceCapteur(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Capteur>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Capteur>>(url);
        }

        public async Task<Capteur?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Capteur?>(url + $"/{id}");
        }

        public async Task<Capteur> PostAsync(Capteur capteur)
        {
            var response = await _httpClient.PostAsJsonAsync(url, capteur);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Capteur>();
        }

        public async Task PutAsync(int id, Capteur capteur)
        {
            var response = await _httpClient.PutAsJsonAsync(url + $"/{id}", capteur);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(url + $"/{id}");
            response.EnsureSuccessStatusCode();
        }

    }
}