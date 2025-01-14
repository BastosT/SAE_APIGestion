using SAE_CLIENTGestion.Models.DTO;
using System.Net.Http.Json;

namespace SAE_CLIENTGestion.Services.DTO
{
    public class WSServiceTypeEquipementDTO : IService<TypeEquipementDTO>
    {
        private readonly HttpClient _httpClient;

        private string url = "api/TypeEquipement/dto";

        public WSServiceTypeEquipementDTO(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TypeEquipementDTO>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TypeEquipementDTO>>(url);
        }

        public async Task<TypeEquipementDTO?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<TypeEquipementDTO?>(url + $"/{id}");
        }

        public async Task<TypeEquipementDTO> PostAsync(TypeEquipementDTO typeSalleDTO)
        {
            var response = await _httpClient.PostAsJsonAsync(url, typeSalleDTO);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TypeEquipementDTO>();
        }

        public async Task PutAsync(int id, TypeEquipementDTO typeEquipementDTO)
        {
            var response = await _httpClient.PutAsJsonAsync(url + $"/{id}", typeEquipementDTO);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(url + $"/{id}");
            response.EnsureSuccessStatusCode();
        }

    }
}
