using CommunityToolkit.Mvvm.ComponentModel;
using SAE_CLIENTGestion.Models;
using SAE_CLIENTGestion.Models.DTO;
using SAE_CLIENTGestion.Services;

namespace SAE_CLIENTGestion.ViewModels
{
    public partial class BatimentsViewModel : ObservableObject
    {
        private readonly IService<BatimentDTO> _batimentServiceDTO;

        public BatimentsViewModel(IService<BatimentDTO> batimentServiceDTO)
        {
            _batimentServiceDTO = batimentServiceDTO;
        }

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private List<BatimentDTO> _batiments = new List<BatimentDTO>();

        [ObservableProperty]
        private string? _successMessage;

        [ObservableProperty]
        private string? _errorMessage;

        public async Task LoadDataAsync()
        {
            IsLoading = true;
            try
            {
                Batiments = await _batimentServiceDTO.GetAllAsync();
                SuccessMessage = null;
                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des bâtiments : {ex.Message}";
                SuccessMessage = null;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task<bool> AddBatimentAsync(BatimentDTO batiment)
        {
            IsLoading = true;
            try
            {
                await _batimentServiceDTO.PostAsync(batiment);
                await LoadDataAsync();
                SuccessMessage = "Bâtiment ajouté avec succès";
                ErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de l'ajout du bâtiment : {ex.Message}";
                SuccessMessage = null;
                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task<bool> UpdateBatimentAsync(BatimentDTO batiment)
        {
            IsLoading = true;
            try
            {
                await _batimentServiceDTO.PutAsync(batiment.BatimentId, batiment);
                await LoadDataAsync();
                SuccessMessage = "Bâtiment modifié avec succès";
                ErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la modification du bâtiment : {ex.Message}";
                SuccessMessage = null;
                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task<bool> DeleteBatimentAsync(int id)
        {
            IsLoading = true;
            try
            {
                await _batimentServiceDTO.DeleteAsync(id);
                await LoadDataAsync();
                SuccessMessage = "Bâtiment supprimé avec succès";
                ErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression du bâtiment : {ex.Message}";
                SuccessMessage = null;
                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}