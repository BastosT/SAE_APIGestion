using CommunityToolkit.Mvvm.ComponentModel;
using SAE_CLIENTGestion.Models;
using SAE_CLIENTGestion.Services;

namespace SAE_CLIENTGestion.ViewModels
{
    public partial class SallesViewModel : ObservableObject
    {
        private readonly IService<Salle> _salleService;
        private readonly IService<Batiment> _batimentService;
        private readonly IService<Mur> _murService;
        private readonly IService<Capteur> _capteurService;
        private readonly IService<Equipement> _equipementService;
        private readonly IService<TypeSalle> _typeSalleService;

        public SallesViewModel(
            IService<Salle> salleService,
            IService<Batiment> batimentService,
            IService<Capteur> capteurService,
            IService<Mur> murService,
            IService<Equipement> equipementService,
            IService<TypeSalle> typeSalleService)
        {
            _salleService = salleService;
            _batimentService = batimentService;
            _capteurService = capteurService;
            _murService = murService;
            _equipementService = equipementService;
            _typeSalleService = typeSalleService;
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
        private List<TypeSalle> _typesSalle = new List<TypeSalle>();

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
                    LoadTypesSalleAsync()
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


        // CRUD SALLE

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
                // Chargement des capteurs et équipements actuels
                await LoadCapteursSalleAsync(salle.SalleId);
                await LoadEquipementsSalleAsync(salle.SalleId);

                // Mise à jour de la salle
                await _salleService.PutAsync(salle.SalleId, salle);

                // Mise à jour des capteurs
                foreach (var capteur in CapteursSalle)
                {
                    await _capteurService.PutAsync(capteur.CapteurId, capteur);
                }

                // Mise à jour des équipements
                foreach (var equipement in EquipementsSalle)
                {
                    await _equipementService.PutAsync(equipement.EquipementId, equipement);
                }

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

        public async Task<bool> AddCapteurToSalleAsync(Capteur capteur)
        {
            try
            {
                await _capteurService.PostAsync(capteur);
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

        public async Task<bool> AddEquipementToSalleAsync(Equipement equipement)
        {
            try
            {
                await _equipementService.PostAsync(equipement);
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

        public async Task<bool> AddCapteurAsync(Capteur capteur)
        {
            try
            {
                await _capteurService.PostAsync(capteur);
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

        public async Task<bool> UpdateCapteurAsync(Capteur capteur)
        {
            try
            {
                await _capteurService.PutAsync(capteur.CapteurId, capteur);
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


        public async Task<bool> AddEquipementAsync(Equipement equipement)
        {
            try
            {
                await _equipementService.PostAsync(equipement);
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

        public async Task<bool> UpdateEquipementAsync(Equipement equipement)
        {
            try
            {
                await _equipementService.PutAsync(equipement.EquipementId, equipement);
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

        // CRUD TYPESALLE

        public async Task<bool> AddTypeSalleAsync(TypeSalle typeSalle)
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

        public async Task<bool> UpdateTypeSalleAsync(TypeSalle typeSalle)
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