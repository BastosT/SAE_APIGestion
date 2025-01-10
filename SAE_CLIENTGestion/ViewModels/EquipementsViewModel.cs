﻿using CommunityToolkit.Mvvm.ComponentModel;
using SAE_CLIENTGestion.Models;
using SAE_CLIENTGestion.Models.DTO;
using SAE_CLIENTGestion.Services;

namespace SAE_CLIENTGestion.ViewModels
{
    public partial class EquipementsViewModel : ObservableObject
    {
        private readonly IService<Equipement> _equipementService;
        private readonly IService<EquipementDTO> _equipementServiceDTO;
        private readonly IService<Salle> _salleService;
        private readonly IService<TypeEquipement> _typeEquipementService;
        private readonly IService<Mur> _murService;

        public EquipementsViewModel(
            IService<Equipement> equipementService,
            IService<EquipementDTO> equipementServiceDTO,
            IService<Salle> salleService,
            IService<TypeEquipement> typeEquipementService,
            IService<Mur> murService)
        {
            _equipementService = equipementService;
            _equipementServiceDTO = equipementServiceDTO;
            _salleService = salleService;
            _typeEquipementService = typeEquipementService;
            _murService = murService;
        }

        [ObservableProperty]
        private bool _isLoading;

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