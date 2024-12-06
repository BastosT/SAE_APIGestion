using SAE_CLIENTGestion.Models.DTO;
using System.Net.Http.Json;

namespace SAE_CLIENTGestion.Services.DTO
{
    public class WSServiceEquipementDTO : IService<EquipementDTO>
    {
        private readonly HttpClient _httpClient;

        private string url = "api/Equipement/dto";

        public WSServiceEquipementDTO(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<EquipementDTO>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<EquipementDTO>>(url);
        }

        public async Task<EquipementDTO?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<EquipementDTO?>(url + $"/{id}");
        }

        public async Task<EquipementDTO> PostAsync(EquipementDTO equipement)
        {
            var response = await _httpClient.PostAsJsonAsync(url, equipement);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<EquipementDTO>();

        }

        public async Task PutAsync(int id, EquipementDTO equipement)
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
