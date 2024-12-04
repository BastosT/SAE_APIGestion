using SAE_CLIENTGestion.Models.DTO;
using System.Net.Http.Json;

namespace SAE_CLIENTGestion.Services.DTO
{
    public class WSServiceTypeSalleDTO : IService<TypeSalleDTO>
    {
        private readonly HttpClient _httpClient;

        private string url = "api/TypeSalle/dto";

        public WSServiceTypeSalleDTO(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TypeSalleDTO>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TypeSalleDTO>>(url);
        }

        public async Task<TypeSalleDTO?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<TypeSalleDTO?>(url + $"/{id}");
        }

        public async Task<TypeSalleDTO> PostAsync(TypeSalleDTO typeSalleDTO)
        {
            var response = await _httpClient.PostAsJsonAsync(url, typeSalleDTO);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TypeSalleDTO>();
        }

        public async Task PutAsync(int id, TypeSalleDTO typeSalleDTO)
        {
            var response = await _httpClient.PutAsJsonAsync(url + $"/{id}", typeSalleDTO);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(url + $"/{id}");
            response.EnsureSuccessStatusCode();
        }

    }
}
