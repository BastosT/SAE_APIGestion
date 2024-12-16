using SAE_CLIENTGestion.Models.DTO;
using System.Net.Http.Json;
namespace SAE_CLIENTGestion.Services.DTO
{
    public class WSServiceBatimentDTO : IService<BatimentDTO>
    {
        private readonly HttpClient _httpClient;

        private string url = "api/Batiment/dto";

        public WSServiceBatimentDTO(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<BatimentDTO>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<BatimentDTO>>(url);
        }

        public async Task<BatimentDTO?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<BatimentDTO?>(url + $"/{id}");
        }

        public async Task<BatimentDTO> PostAsync(BatimentDTO batiment)
        {
            var response = await _httpClient.PostAsJsonAsync(url, batiment);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<BatimentDTO>();
        }

        public async Task PutAsync(int id, BatimentDTO batiment)
        {
            var response = await _httpClient.PutAsJsonAsync(url + $"/{id}", batiment);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(url + $"/{id}");
            response.EnsureSuccessStatusCode();
        }

    }
}
