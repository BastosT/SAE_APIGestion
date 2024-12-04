using SAE_CLIENTGestion.Services;
using System.Net.Http.Json;
using SAE_CLIENTGestion.Models;

namespace TD1Client.Services
{
    public class WSServiceEquipement : IService<Equipement>
    {
        private readonly HttpClient _httpClient;

        private string url = "api/Equipement";

        public WSServiceEquipement(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Equipement>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Equipement>>(url);
        }

        public async Task<Equipement?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Equipement?>(url + $"/{id}");
        }

        public async Task<Equipement> PostAsync(Equipement equipement)
        {
            var response = await _httpClient.PostAsJsonAsync(url, equipement);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Equipement>();

        }

        public async Task PutAsync(int id, Equipement equipement)
        {
            var response = await _httpClient.PutAsJsonAsync(url + $"/{id}", equipement);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(url + $"/{id}");
            response.EnsureSuccessStatusCode();
        }

    }
}