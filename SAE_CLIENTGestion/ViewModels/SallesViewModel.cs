using CommunityToolkit.Mvvm.ComponentModel;
using SAE_CLIENTGestion.Models;
using SAE_CLIENTGestion.Services;

namespace SAE_CLIENTGestion.ViewModels
{
    public partial class SallesViewModel : ObservableObject
    {
        private readonly IService<Salle> _salleService;

        public SallesViewModel(IService<Salle> salleService)
        {
            _salleService = salleService;
        }

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private List<Salle> _salles = new List<Salle>();

        [ObservableProperty]
        private List<TypeSalle> _typesSalle = new List<TypeSalle>();

        [ObservableProperty]
        private List<Batiment> _batiments = new List<Batiment>();

        [ObservableProperty]
        private string? _successMessage;

        [ObservableProperty]
        private string? _errorMessage;

        public async Task LoadDataAsync()
        {
            IsLoading = true;
            try
            {
                var tasks = new List<Task>
                {
                    LoadSallesAsync(),
                };

                await Task.WhenAll(tasks);
                SuccessMessage = null;
                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des données : {ex.Message}";
                SuccessMessage = null;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadSallesAsync()
        {
            try
            {
                Salles = await _salleService.GetAllAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des salles : {ex.Message}";
            }
        }

        public async Task<bool> AddSalleAsync(Salle salle)
        {
            IsLoading = true;
            try
            {
                await _salleService.PostAsync(salle);
                await LoadSallesAsync();
                SuccessMessage = "Salle ajoutée avec succès";
                ErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de l'ajout de la salle : {ex.Message}";
                SuccessMessage = null;
                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task<bool> UpdateSalleAsync(Salle salle)
        {
            IsLoading = true;
            try
            {
                await _salleService.PutAsync(salle.SalleId, salle);
                await LoadSallesAsync();
                SuccessMessage = "Salle modifiée avec succès";
                ErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la modification de la salle : {ex.Message}";
                SuccessMessage = null;
                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task<bool> DeleteSalleAsync(int id)
        {
            IsLoading = true;
            try
            {
                await _salleService.DeleteAsync(id);
                await LoadSallesAsync();
                SuccessMessage = "Salle supprimée avec succès";
                ErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression de la salle : {ex.Message}";
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