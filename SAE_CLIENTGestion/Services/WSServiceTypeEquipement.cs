using SAE_CLIENTGestion.Models;
using System.Net.Http.Json;

namespace SAE_CLIENTGestion.Services
{
    public class WSServiceTypeEquipement : IService<TypeEquipement>
    {
        private readonly HttpClient _httpClient;

        private string url = "api/TypeEquipement";

        public WSServiceTypeEquipement(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TypeEquipement>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TypeEquipement>>(url);
        }

        public async Task<TypeEquipement?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<TypeEquipement?>(url + $"/{id}");
        }

        public async Task<TypeEquipement> PostAsync(TypeEquipement typeEquipement)
        {
            var response = await _httpClient.PostAsJsonAsync(url, typeEquipement);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TypeEquipement>();
        }

        public async Task PutAsync(int id, TypeEquipement typeEquipement)
        {
            var response = await _httpClient.PutAsJsonAsync(url + $"/{id}", typeEquipement);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(url + $"/{id}");
            response.EnsureSuccessStatusCode();
        }

    }
}
