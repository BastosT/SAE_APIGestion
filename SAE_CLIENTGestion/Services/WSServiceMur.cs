using SAE_CLIENTGestion.Services;
using System.Net.Http.Json;
using SAE_CLIENTGestion.Models;

namespace TD1Client.Services
{
    public class WSServiceMur : IService<Mur>
    {
        private readonly HttpClient _httpClient;

        private string url = "api/Mur";

        public WSServiceMur(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Mur>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Mur>>(url);
        }

        public async Task<Mur?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Mur?>(url + $"/{id}");
        }

        public async Task<Mur> PostAsync(Mur mur)
        {
            var response = await _httpClient.PostAsJsonAsync(url, mur);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Mur>();
        }

        public async Task PutAsync(int id, Mur mur)
        {
            var response = await _httpClient.PutAsJsonAsync(url + $"/{id}", mur);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(url + $"/{id}");
            response.EnsureSuccessStatusCode();
        }

    }
}