using CommunityToolkit.Mvvm.ComponentModel;
using SAE_CLIENTGestion.Models;
using SAE_CLIENTGestion.Services;

namespace SAE_CLIENTGestion.ViewModels
{
    public partial class SallesViewModel : ObservableObject
    {
        private readonly IService<Salle> _salleService;
        private readonly IService<Batiment> _batimentService;

        public SallesViewModel(
            IService<Salle> salleService,
            IService<Batiment> batimentService)
        {
            _salleService = salleService;
            _batimentService = batimentService;
        }

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private List<Salle> _salles = new List<Salle>();


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
                    LoadBatimentsAsync()
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

        private async Task LoadBatimentsAsync()
        {
            try
            {
                Batiments = await _batimentService.GetAllAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des bâtiments : {ex.Message}";
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