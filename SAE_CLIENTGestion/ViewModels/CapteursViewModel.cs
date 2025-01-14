using CommunityToolkit.Mvvm.ComponentModel;
using SAE_CLIENTGestion.Models;
using SAE_CLIENTGestion.Models.DTO;
using SAE_CLIENTGestion.Services;

namespace SAE_CLIENTGestion.ViewModels
{
    public partial class CapteursViewModel : ObservableObject
    {
        private readonly IService<Capteur> _capteurService;
        private readonly IService<CapteurDTO> _capteurServiceDTO;
        private readonly IService<Salle> _salleService;
        private readonly IService<Mur> _murService;
        private readonly IService<Batiment> _batimentService;

        public CapteursViewModel(
            IService<Capteur> capteurService,
            IService<CapteurDTO> capteurServiceDTO,
            IService<Salle> salleService,
            IService<Batiment> batimentService,
            IService<Mur> murService)
        {
            _capteurService = capteurService;
            _capteurServiceDTO = capteurServiceDTO;
            _salleService = salleService;
            _murService = murService;
            _batimentService = batimentService;
        }

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private List<Capteur> _capteurs = new List<Capteur>();

        [ObservableProperty]
        private List<Salle> _salles = new List<Salle>();

        [ObservableProperty]
        private List<Batiment> _batiments = new List<Batiment>();

        [ObservableProperty]
        private List<Mur> _murs = new List<Mur>();

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
                    LoadCapteursAsync(),
                    LoadSallesAsync(),
                    LoadMursAsync(),
                    LoadBatimentsAsync(),
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

        private async Task LoadCapteursAsync()
        {
            try
            {
                Capteurs = await _capteurService.GetAllAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des capteurs : {ex.Message}";
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

        private async Task LoadMursAsync()
        {
            try
            {
                Murs = await _murService.GetAllAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des murs : {ex.Message}";
            }
        }

        public async Task<bool> AddCapteurAsync(CapteurDTO capteur)
        {
            try
            {
                await _capteurServiceDTO.PostAsync(capteur);
                await LoadCapteursAsync();
                SuccessMessage = "Capteur ajouté avec succès";
                ErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de l'ajout du capteur : {ex.Message}";
                SuccessMessage = null;
                return false;
            }
        }

        public async Task<bool> UpdateCapteurAsync(CapteurDTO capteur)
        {
            try
            {
                await _capteurServiceDTO.PutAsync(capteur.CapteurId, capteur);
                await LoadCapteursAsync();
                SuccessMessage = "Capteur modifié avec succès";
                ErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la modification du capteur : {ex.Message}";
                SuccessMessage = null;
                return false;
            }
        }

        public async Task<bool> DeleteCapteurAsync(int capteurId)
        {
            try
            {
                await _capteurService.DeleteAsync(capteurId);
                await LoadCapteursAsync();
                SuccessMessage = "Capteur supprimé avec succès";
                ErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression du capteur : {ex.Message}";
                SuccessMessage = null;
                return false;
            }
        }

        public async Task<Capteur?> GetCapteurByIdAsync(int capteurId)
        {
            try
            {
                return await _capteurService.GetByIdAsync(capteurId);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la récupération du capteur : {ex.Message}";
                return null;
            }
        }
    }
}