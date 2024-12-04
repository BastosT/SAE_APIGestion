using SAE_CLIENTGestion.Models.DTO;
using System.Net.Http.Json;

namespace SAE_CLIENTGestion.Services.DTO
{
    public class WSServiceSalleDTO : IService<SalleDTO>
    {
        private readonly HttpClient _httpClient;

        private string url = "api/Salle/dto";

        public WSServiceSalleDTO(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SalleDTO>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<SalleDTO>>(url);
        }

        public async Task<SalleDTO?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<SalleDTO?>(url + $"/{id}");
        }

        public async Task<SalleDTO> PostAsync(SalleDTO salleDTO)
        {
            var response = await _httpClient.PostAsJsonAsync(url, salleDTO);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SalleDTO>();
        }

        public async Task PutAsync(int id, SalleDTO salleDTO)
        {
            var response = await _httpClient.PutAsJsonAsync(url + $"/{id}", salleDTO);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(url + $"/{id}");
            response.EnsureSuccessStatusCode();
        }

    }
}
