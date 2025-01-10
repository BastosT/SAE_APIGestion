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
        private readonly IService<BatimentDTO> _batimentService;
        private readonly IService<Mur> _murService;
        private readonly IService<MurDTO> _murServiceDTO;
        private readonly IService<CapteurDTO> _capteurServiceDTO;
        private readonly IService<Capteur> _capteurService;
        private readonly IService<EquipementDTO> _equipementServiceDTO;
        private readonly IService<Equipement> _equipementService;
        private readonly IService<TypeSalleDTO> _typeSalleService;
        private readonly IService<TypeEquipement> _typeEquipementService;

        public SallesViewModel(
            IService<Salle> salleService,
            IService<SalleDTO> salleServiceDTO,
            IService<BatimentDTO> batimentService,
            IService<Mur> murService,
            IService<MurDTO> murServiceDTO,
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
            _murServiceDTO = murServiceDTO;
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
        private List<Mur> _murs = new List<Mur>();

        [ObservableProperty]
        private List<BatimentDTO> _batiments = new List<BatimentDTO>();

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

        public async Task LoadCapteursSalleAsync(int? salleId)
        {
            try
            {
                var allCapteurs = await _capteurService.GetAllAsync();
                if (allCapteurs != null)
                {
                    CapteursSalle = allCapteurs.Where(c => c.SalleId == salleId).ToList();
                }
                else
                {
                    CapteursSalle = new List<Capteur>(); // Valeur par défaut si la liste est nulle
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des capteurs : {ex.Message}";
                CapteursSalle = new List<Capteur>(); // Valeur par défaut en cas d'erreur
            }
        }


        public async Task LoadEquipementsSalleAsync(int? salleId)
        {
            try
            {
                var allEquipements = await _equipementService.GetAllAsync();
                if (allEquipements != null)
                {
                    EquipementsSalle = allEquipements.Where(e => e.SalleId == salleId).ToList();
                }
                else
                {
                    EquipementsSalle = new List<Equipement>(); // Valeur par défaut si la liste est nulle
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des équipements : {ex.Message}";
                EquipementsSalle = new List<Equipement>(); // Valeur par défaut en cas d'erreur
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

        private async Task LoadMurAsync()
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

        public async Task<SalleDTO?> AddSalleAsync(SalleDTO salle)
        {
            IsLoading = true;
            try
            {
                var createdSalle = await _salleServiceDTO.PostAsync(salle);
                await LoadSallesAsync();
                SuccessMessage = "Salle ajoutée avec succès";
                ErrorMessage = null;
                return createdSalle;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de l'ajout de la salle : {ex.Message}";
                SuccessMessage = null;
                return null;
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



        public async Task DeleteAllMursForSalleAsync(int salleId)
        {
            try
            {
                var allMurs = await _murService.GetAllAsync();
                var mursSalle = allMurs.Where(m => m.SalleId == salleId).ToList();

                foreach (var mur in mursSalle)
                {
                    await _murService.DeleteAsync(mur.MurId);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression des murs : {ex.Message}";
                throw;
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

                foreach (var mur in Murs)
                {
                    if (mur.SalleId == id)
                    {
                        await _murService.DeleteAsync(mur.MurId);
                    }
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

        public async Task<List<Mur>> GetMursBySalleIdAsync(int salleId)
        {
            try
            {
                var allMurs = await _murService.GetAllAsync();
                return allMurs.Where(m => m.SalleId == salleId)
                             .ToList();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la récupération des murs : {ex.Message}";
                return new List<Mur>();
            }
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

        public async Task<MurDTO> AddMurAsync(string nom, MurOrientation orientation, double longueurMur, double hauteurMur, int salleid)
        {
            try
            {
                var mur = new MurDTO
                {
                    Nom = nom,
                    Longueur = longueurMur,
                    Hauteur = hauteurMur,
                    Orientation = orientation,
                    SalleId = salleid
                };

                var createdMur = await _murServiceDTO.PostAsync(mur);
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


        // CRUD BATIMENT

        public async Task<bool> AddBatimentAsync(BatimentDTO batiment)
        {
            IsLoading = true;
            try
            {
                await _batimentService.PostAsync(batiment);
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
                await _batimentService.PutAsync(batiment.BatimentId, batiment);
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
                await _batimentService.DeleteAsync(id);
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

        public async Task<bool> UpdateCapteurMurId(int oldMurId, int newMurId)
        {
            try
            {
                // Récupérer tous les capteurs
                var allCapteurs = await _capteurService.GetAllAsync();
                // Filtrer les capteurs qui ont l'ancien MurId
                var capteursToUpdate = allCapteurs.Where(c => c.MurId == oldMurId).ToList();

                // Pour chaque capteur trouvé, mettre à jour son MurId
                foreach (var capteur in capteursToUpdate)
                {
                    var capteurDTO = new CapteurDTO
                    {
                        CapteurId = capteur.CapteurId,
                        Nom = capteur.Nom,
                        EstActif = capteur.EstActif,
                        Longueur = capteur.Longueur,
                        Hauteur = capteur.Hauteur,
                        PositionX = capteur.PositionX,
                        PositionY = capteur.PositionY,
                        DistancePorte = capteur.DistancePorte,
                        DistanceChauffage = capteur.DistanceChauffage,
                        DistanceFenetre = capteur.DistanceFenetre,
                        SalleId = capteur.SalleId,
                        MurId = newMurId
                    };

                    await _capteurServiceDTO.PutAsync(capteur.CapteurId, capteurDTO);
                }

                SuccessMessage = "Capteurs mis à jour avec succès";
                ErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la mise à jour des capteurs : {ex.Message}";
                SuccessMessage = null;
                return false;
            }
        }

        public async Task<List<Mur>> GetMursWithCapteursBySalleId(int salleId)
        {
            try
            {
                Console.WriteLine("==========================================");
                Console.WriteLine("==========================================");
                Console.WriteLine("\n=== DÉBUT RÉCUPÉRATION MURS ET CAPTEURS ===");

                Console.WriteLine("\nRécupération de tous les murs ---");
                var allMurs = await _murService.GetAllAsync();
                Console.WriteLine($"Nombre total de murs: {allMurs.Count}");

                Console.WriteLine($"\nFiltrage des murs pour la salle {salleId} ---");
                Console.WriteLine("\n--- Tous les murs et leurs SalleId ---");
                foreach (var mur in allMurs)
                {
                    Console.WriteLine($"Mur {mur.Nom} (ID: {mur.MurId}) - SalleId: {mur.SalleId}");
                }

                Console.WriteLine($"\n--- Tentative de filtrage pour salleId: {salleId} ---");
                var mursSalle = allMurs.Where(m => m.SalleId == salleId).ToList();
                Console.WriteLine($"Nombre de murs dans la salle: {mursSalle.Count}");
                foreach (var mur in mursSalle)
                {
                    Console.WriteLine($"Mur trouvé: {mur.Nom} (ID: {mur.MurId})");
                }

                Console.WriteLine("\nRécupération de tous les capteurs ---");
                var allCapteurs = await _capteurService.GetAllAsync();
                Console.WriteLine($"Nombre total de capteurs: {allCapteurs.Count}");

                Console.WriteLine("\nAssociation des capteurs aux murs ---");
                foreach (var mur in mursSalle)
                {
                    var capteursDuMur = allCapteurs.Where(c => c.MurId == mur.MurId).ToList();
                    mur.Capteurs = capteursDuMur;
                    Console.WriteLine($"Mur {mur.Nom} (ID: {mur.MurId}): {capteursDuMur.Count} capteurs trouvés");
                    foreach (var capteur in capteursDuMur)
                    {
                        Console.WriteLine($"Capteur: {capteur.Nom} (ID: {capteur.CapteurId})");
                    }
                }

                Console.WriteLine("\nFIN RÉCUPÉRATION MURS ET CAPTEURS ===\n");
                Console.WriteLine("==========================================");
                Console.WriteLine("==========================================");
                return mursSalle;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n=== ERREUR ===");
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                Console.WriteLine("=============\n");
                ErrorMessage = $"Erreur lors de la récupération des murs avec capteurs : {ex.Message}";
                return new List<Mur>();
            }
        }

        public async Task<bool> DeleteMurAsync(int murId)
        {
            try
            {
                await _murService.DeleteAsync(murId);
                await LoadMurAsync();  // Recharger la liste des murs
                SuccessMessage = "Mur supprimé avec succès";
                ErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression du mur : {ex.Message}";
                SuccessMessage = null;
                return false;
            }
        }
    }
}