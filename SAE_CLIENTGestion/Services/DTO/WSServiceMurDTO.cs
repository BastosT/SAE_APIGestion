using SAE_CLIENTGestion.Models.DTO;
using System.Net.Http.Json;

namespace SAE_CLIENTGestion.Services.DTO
{
    public class WSServiceMurDTO : IService<MurDTO>
    {
        private readonly HttpClient _httpClient;

        private string url = "api/Mur/dto";

        public WSServiceMurDTO(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<MurDTO>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<MurDTO>>(url);
        }

        public async Task<MurDTO?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<MurDTO?>(url + $"/{id}");
        }

        public async Task<MurDTO> PostAsync(MurDTO capteur)
        {
            var response = await _httpClient.PostAsJsonAsync(url, capteur);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MurDTO>();

        }

        public async Task PutAsync(int id, MurDTO capteur)
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
