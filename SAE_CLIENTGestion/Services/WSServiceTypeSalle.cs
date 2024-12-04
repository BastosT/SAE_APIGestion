using SAE_CLIENTGestion.Services;
using System.Net.Http.Json;
using SAE_CLIENTGestion.Models;

namespace TD1Client.Services
{
    public class WSServiceTypeSalle : IService<TypeSalle>
    {
        private readonly HttpClient _httpClient;

        private string url = "api/TypeSalle";

        public WSServiceTypeSalle(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TypeSalle>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TypeSalle>>(url);
        }

        public async Task<TypeSalle?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<TypeSalle?>(url + $"/{id}");
        }

        public async Task<TypeSalle> PostAsync(TypeSalle typeSalle)
        {
            var response = await _httpClient.PostAsJsonAsync(url, typeSalle);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TypeSalle>();
        }

        public async Task PutAsync(int id, TypeSalle typeSalle)
        {
            var response = await _httpClient.PutAsJsonAsync(url + $"/{id}", typeSalle);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(url + $"/{id}");
            response.EnsureSuccessStatusCode();
        }

    }
}