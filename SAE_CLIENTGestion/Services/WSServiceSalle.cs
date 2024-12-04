using SAE_CLIENTGestion.Services;
using System.Net.Http.Json;
using SAE_CLIENTGestion.Models;

namespace TD1Client.Services
{
    public class WSServiceSalle : IService<Salle>
    {
        private readonly HttpClient _httpClient;

        private string url = "api/Salle";

        public WSServiceSalle(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Salle>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Salle>>(url);
        }

        public async Task<Salle?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Salle?>(url + $"/{id}");
        }

        public async Task<Salle> PostAsync(Salle salle)
        {
            var response = await _httpClient.PostAsJsonAsync(url, salle);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Salle>();
        }

        public async Task PutAsync(int id, Salle salle)
        {
            var response = await _httpClient.PutAsJsonAsync(url + $"/{id}", salle);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(url + $"/{id}");
            response.EnsureSuccessStatusCode();
        }

    }
}