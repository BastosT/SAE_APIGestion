using SAE_CLIENTGestion.Services;
using System.Net.Http.Json;
using SAE_CLIENTGestion.Models;

namespace TD1Client.Services
{
    public class WSServiceBatiment : IService<Batiment>
    {
        private readonly HttpClient _httpClient;

        private string url = "api/Batiment";

        public WSServiceBatiment(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Batiment>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Batiment>>(url);
        }

        public async Task<Batiment?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Batiment?>(url+$"/{id}");
        }

        public async Task<Batiment> PostAsync(Batiment batiment)
        {
            var response = await _httpClient.PostAsJsonAsync(url, batiment);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Batiment>();
        }

        public async Task PutAsync(int id, Batiment batiment)
        {
            var response = await _httpClient.PutAsJsonAsync(url+$"/{id}", batiment);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(url+$"/{id}");
            response.EnsureSuccessStatusCode();
        }

    }
}