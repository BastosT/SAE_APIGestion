using SAE_CLIENTGestion.Models.DTO;
using System.Net.Http.Json;

namespace SAE_CLIENTGestion.Services.DTO
{
    public class WSServiceCapteurDTO : IService<CapteurDTO>
    {
        private readonly HttpClient _httpClient;

        private string url = "api/Capteur/dto";

        public WSServiceCapteurDTO(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CapteurDTO>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CapteurDTO>>(url);
        }

        public async Task<CapteurDTO?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<CapteurDTO?>(url + $"/{id}");
        }

        public async Task<CapteurDTO> PostAsync(CapteurDTO capteur)
        {
            var response = await _httpClient.PostAsJsonAsync(url, capteur);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CapteurDTO>();

        }

        public async Task PutAsync(int id, CapteurDTO capteur)
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
