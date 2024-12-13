using CommunityToolkit.Mvvm.ComponentModel;
using SAE_CLIENTGestion.Models;
using SAE_CLIENTGestion.Models.DTO;
using SAE_CLIENTGestion.Services;
using System.Net.Http;
using static SAE_CLIENTGestion.Pages.Salles;

namespace SAE_CLIENTGestion.ViewModels
{
    public partial class SallesViewModel : ObservableObject
    {
        private readonly IService<Salle> _salleService;
        private readonly IService<SalleDTO> _salleServiceDTO;
        private readonly IService<Batiment> _batimentService;
        private readonly IService<Mur> _murService;
        private readonly IService<CapteurDTO> _capteurServiceDTO;
        private readonly IService<Capteur> _capteurService;
        private readonly IService<EquipementDTO> _equipementServiceDTO;
        private readonly IService<Equipement> _equipementService;
        private readonly IService<TypeSalleDTO> _typeSalleService;
        private readonly IService<TypeEquipement> _typeEquipementService;

        public SallesViewModel(
            IService<Salle> salleService,
            IService<SalleDTO> salleServiceDTO,
            IService<Batiment> batimentService,
            IService<Mur> murService,
            IService<Capteur> capteurService,
            IService<CapteurDTO> capteurServiceDTO,
            IService<Equipement> equipementService,
            IService<EquipementDTO> equipementServiceDTO,
            IService<TypeSalleDTO> typeSalleService,
            IService<TypeEquipement> typeEquipementService)
        {
            _salleService = salleService;
            _salleServiceDTO = salleServiceDTO;
            _batimentService = batimentService;
            _murService = murService;
            _capteurService = capteurService;
            _capteurServiceDTO = capteurServiceDTO;
            _equipementService = equipementService;
            _equipementServiceDTO = equipementServiceDTO;
            _typeSalleService = typeSalleService;
            _typeEquipementService = typeEquipementService;
        }

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private List<Salle> _salles = new List<Salle>();

        [ObservableProperty]
        private List<Batiment> _batiments = new List<Batiment>();

        [ObservableProperty]
        private List<Capteur> _capteursSalle = new List<Capteur>();

        [ObservableProperty]
        private List<Equipement> _equipementsSalle = new List<Equipement>();

        [ObservableProperty]
        private List<TypeSalleDTO> _typesSalle = new List<TypeSalleDTO>();

        [ObservableProperty]
        private List<TypeEquipement> _typesEquipement = new List<TypeEquipement>();

        [ObservableProperty]
        private string? _successMessage;

        [ObservableProperty]
        private string? _errorMessage;

        // Loaders

        public async Task LoadDataAsync()
        {
            IsLoading = true;
            try
            {
                var tasks = new List<Task>
                {
                    LoadSallesAsync(),
                    LoadBatimentsAsync(),
                    LoadTypesSalleAsync(),
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

        public async Task LoadCapteursSalleAsync(int salleId)
        {
            try
            {
                var allCapteurs = await _capteurService.GetAllAsync();
                CapteursSalle = allCapteurs.Where(c => c.SalleId == salleId).ToList();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des capteurs : {ex.Message}";
            }
        }

        public async Task LoadEquipementsSalleAsync(int salleId)
        {
            try
            {
                var allEquipements = await _equipementService.GetAllAsync();
                EquipementsSalle = allEquipements.Where(e => e.SalleId == salleId).ToList();
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

        private async Task LoadTypesSalleAsync()
        {
            try
            {
                TypesSalle = await _typeSalleService.GetAllAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des types de salle : {ex.Message}";
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
                ErrorMessage = $"Erreur lors du chargement des types d'équipements : {ex.Message}";
            }
        }


        // CRUD SALLE
        public async Task<Salle> GetSalleByIdAsync(int salleId)
        {
            // Implémentez la récupération d'une salle par son ID
            return await _salleService.GetByIdAsync(salleId);
        }

        public async Task<bool> AddSalleAsync(SalleDTO salle)
        {
            IsLoading = true;
            try
            {
                await _salleServiceDTO.PostAsync(salle);
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

        public async Task<bool> UpdateSalleAsync(SalleDTO salle)
        {
            IsLoading = true;
            try
            {

                // Mise à jour de la salle
                await _salleServiceDTO.PutAsync(salle.SalleId, salle);

                await LoadSallesAsync();
                SuccessMessage = "Salle et ses éléments modifiés avec succès";
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

        public async Task<bool> AddCapteurToSalleAsync(CapteurDTO capteur)
        {
            try
            {
                await _capteurServiceDTO.PostAsync(capteur);
                await LoadCapteursSalleAsync(capteur.SalleId);
                SuccessMessage = "Capteur ajouté avec succès";
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de l'ajout du capteur : {ex.Message}";
                return false;
            }
        }

        public async Task<bool> AddEquipementToSalleAsync(EquipementDTO equipement)
        {
            try
            {
                await _equipementServiceDTO.PostAsync(equipement);
                await LoadEquipementsSalleAsync(equipement.SalleId);
                SuccessMessage = "Équipement ajouté avec succès";
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de l'ajout de l'équipement : {ex.Message}";
                return false;
            }
        }

        public async Task<bool> DeleteSalleAsync(int id)
        {
            IsLoading = true;
            try
            {
                // Suppression des capteurs et équipements associés
                await LoadCapteursSalleAsync(id);
                await LoadEquipementsSalleAsync(id);

                foreach (var capteur in CapteursSalle)
                {
                    await _capteurService.DeleteAsync(capteur.CapteurId);
                }

                foreach (var equipement in EquipementsSalle)
                {
                    await _equipementService.DeleteAsync(equipement.EquipementId);
                }

                // Suppression de la salle
                await _salleService.DeleteAsync(id);
                await LoadSallesAsync();
                SuccessMessage = "Salle et ses éléments supprimés avec succès";
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

        // CRUD CAPTEUR

        public async Task<bool> AddCapteurAsync(CapteurDTO capteur)
        {
            try
            {
                await _capteurServiceDTO.PostAsync(capteur);
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


        // CRUD EQUIPEMENT


        public async Task<bool> AddEquipementAsync(EquipementDTO equipement)
        {
            try
            {
                await _equipementServiceDTO.PostAsync(equipement);
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


        // CRUD MUR

        public async Task<Mur> GetMurByIdAsync(int murId)
        {
            var mur = await _murService.GetByIdAsync(murId);

            // Initialiser les collections si elles sont null
            mur.Equipements ??= new List<Equipement>();
            mur.Capteurs ??= new List<Capteur>();

            return mur;
        }

        public async Task<bool> UpdateMurAsync(Mur mur)
        {
            try
            {
                await _murService.PutAsync(mur.MurId, mur);
                await LoadSallesAsync(); // Recharger les données pour mettre à jour l'affichage
                SuccessMessage = "Mur modifié avec succès";
                ErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la modification du mur : {ex.Message}";
                SuccessMessage = null;
                return false;
            }
        }

        public async Task<Mur> AddMurAsync(string nom, TypeMur typeMur, double longueurMur, double hauteurMur)
        {
            try
            {
                var mur = new Mur
                {
                    Nom = nom,
                    Type = typeMur,
                    Longueur = longueurMur,
                    Hauteur = hauteurMur,
                };

                var createdMur = await _murService.PostAsync(mur);
                SuccessMessage = $"Mur {nom} créé avec succès";
                ErrorMessage = null;
                return createdMur;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la création du mur {nom} : {ex.Message}";
                SuccessMessage = null;
                return null;
            }
        }


        // CRUD TYPESALLE

        public async Task<bool> AddTypeSalleAsync(TypeSalleDTO typeSalle)
        {
            IsLoading = true;
            try
            {
                await _typeSalleService.PostAsync(typeSalle);
                await LoadTypesSalleAsync();
                SuccessMessage = "Type de salle ajouté avec succès";
                ErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de l'ajout du type de salle : {ex.Message}";
                SuccessMessage = null;
                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task<bool> UpdateTypeSalleAsync(TypeSalleDTO typeSalle)
        {
            IsLoading = true;
            try
            {
                await _typeSalleService.PutAsync(typeSalle.TypeSalleId, typeSalle);
                await LoadTypesSalleAsync();
                SuccessMessage = "Type de salle modifié avec succès";
                ErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la modification du type de salle : {ex.Message}";
                SuccessMessage = null;
                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task<bool> DeleteTypeSalleAsync(int typeSalleId)
        {
            IsLoading = true;
            try
            {
                await _typeSalleService.DeleteAsync(typeSalleId);
                await LoadTypesSalleAsync();
                SuccessMessage = "Type de salle supprimé avec succès";
                ErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression du type de salle : {ex.Message}";
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