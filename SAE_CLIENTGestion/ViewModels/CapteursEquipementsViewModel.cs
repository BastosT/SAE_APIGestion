using CommunityToolkit.Mvvm.ComponentModel;
using SAE_CLIENTGestion.Models;
using SAE_CLIENTGestion.Models.DTO;
using SAE_CLIENTGestion.Services;

namespace SAE_CLIENTGestion.ViewModels
{
    public partial class CapteursEquipementsViewModel : ObservableObject
    {
        private readonly IService<Capteur> _capteurService;
        private readonly IService<CapteurDTO> _capteurServiceDTO;
        private readonly IService<Equipement> _equipementService;
        private readonly IService<EquipementDTO> _equipementServiceDTO;
        private readonly IService<Salle> _salleService;
        private readonly IService<TypeEquipement> _typeEquipementService;
        private readonly IService<Mur> _murService;

        public CapteursEquipementsViewModel(
            IService<Capteur> capteurService,
            IService<CapteurDTO> capteurServiceDTO,
            IService<Equipement> equipementService,
            IService<EquipementDTO> equipementServiceDTO,
            IService<Salle> salleService,
            IService<TypeEquipement> typeEquipementService,
            IService<Mur> murService)
        {
            _capteurService = capteurService;
            _capteurServiceDTO = capteurServiceDTO;
            _equipementService = equipementService;
            _equipementServiceDTO = equipementServiceDTO;
            _salleService = salleService;
            _typeEquipementService = typeEquipementService;
            _murService = murService;
        }

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private List<Capteur> _capteurs = new List<Capteur>();

        [ObservableProperty]
        private List<Equipement> _equipements = new List<Equipement>();

        [ObservableProperty]
        private List<Salle> _salles = new List<Salle>();

        [ObservableProperty]
        private List<Mur> _murs = new List<Mur>();

        [ObservableProperty]
        private List<TypeEquipement> _typesEquipement = new List<TypeEquipement>();

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
                    LoadEquipementsAsync(),
                    LoadSallesAsync(),
                    LoadMursAsync(),
                    LoadTypesEquipementAsync()
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

        private async Task LoadEquipementsAsync()
        {
            try
            {
                Equipements = await _equipementService.GetAllAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des équipements : {ex.Message}";
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

        private async Task LoadTypesEquipementAsync()
        {
            try
            {
                TypesEquipement = await _typeEquipementService.GetAllAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des types d'équipement : {ex.Message}";
            }
        }

        // CRUD Capteurs
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

        // CRUD Équipements
        public async Task<bool> AddEquipementAsync(EquipementDTO equipement)
        {
            try
            {
                await _equipementServiceDTO.PostAsync(equipement);
                await LoadEquipementsAsync();
                SuccessMessage = "Équipement ajouté avec succès";
                ErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de l'ajout de l'équipement : {ex.Message}";
                SuccessMessage = null;
                return false;
            }
        }

        public async Task<bool> UpdateEquipementAsync(EquipementDTO equipement)
        {
            try
            {
                await _equipementServiceDTO.PutAsync(equipement.EquipementId, equipement);
                await LoadEquipementsAsync();
                SuccessMessage = "Équipement modifié avec succès";
                ErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la modification de l'équipement : {ex.Message}";
                SuccessMessage = null;
                return false;
            }
        }

        public async Task<bool> DeleteEquipementAsync(int equipementId)
        {
            try
            {
                await _equipementService.DeleteAsync(equipementId);
                await LoadEquipementsAsync();
                SuccessMessage = "Équipement supprimé avec succès";
                ErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression de l'équipement : {ex.Message}";
                SuccessMessage = null;
                return false;
            }
        }

        // Méthodes utilitaires
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

        public async Task<Equipement?> GetEquipementByIdAsync(int equipementId)
        {
            try
            {
                return await _equipementService.GetByIdAsync(equipementId);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la récupération de l'équipement : {ex.Message}";
                return null;
            }
        }
    }
}