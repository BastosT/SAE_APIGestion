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
        private readonly IService<Mur> _murService;
        private readonly IService<Batiment> _batimentService;
        private readonly IService<TypeEquipementDTO> _typeEquipementServiceDTO;

        public EquipementsViewModel(
            IService<Equipement> equipementService,
            IService<EquipementDTO> equipementServiceDTO,
            IService<Salle> salleService,
            IService<Batiment> batimentService,
            IService<TypeEquipementDTO> typeEquipementServiceDTO,
            IService<Mur> murService)
        {
            _equipementService = equipementService;
            _equipementServiceDTO = equipementServiceDTO;
            _salleService = salleService;
            _murService = murService;
            _batimentService = batimentService;
            _typeEquipementServiceDTO = typeEquipementServiceDTO;
        }

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private List<Equipement> _equipements = new List<Equipement>();

        [ObservableProperty]
        private List<Salle> _salles = new List<Salle>();

        [ObservableProperty]
        private List<Batiment> _batiments = new List<Batiment>();

        [ObservableProperty]
        private List<TypeEquipementDTO> _typeEquipements = new List<TypeEquipementDTO>();

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
                    LoadEquipementsAsync(),
                    LoadSallesAsync(),
                    LoadMursAsync(),
                    LoadBatimentsAsync(),
                    LoadTypeEquipementsAsync(),
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

        private async Task LoadTypeEquipementsAsync()
        {
            try
            {
                TypeEquipements = await _typeEquipementServiceDTO.GetAllAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des types d'équipements : {ex.Message}";
            }
        }

        public async Task<bool> DeleteTypeEquipementAsync(int typeEquipementId)
        {
            IsLoading = true;
            try
            {
                await _typeEquipementServiceDTO.DeleteAsync(typeEquipementId);
                await LoadTypeEquipementsAsync();
                SuccessMessage = "Type de equipement supprimé avec succès";
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
        public async Task<bool> AddTypeEquipementAsync(TypeEquipementDTO typeEquipement)
        {
            IsLoading = true;
            try
            {
                await _typeEquipementServiceDTO.PostAsync(typeEquipement);
                await LoadTypeEquipementsAsync();
                SuccessMessage = "Type d'équipement ajouté avec succès";
                ErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de l'ajout du type d'équipement : {ex.Message}";
                SuccessMessage = null;
                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task<bool> UpdateTypeEquipementAsync(TypeEquipementDTO typeEquipement)
        {
            IsLoading = true;
            try
            {
                await _typeEquipementServiceDTO.PutAsync(typeEquipement.TypeEquipementId, typeEquipement);
                await LoadTypeEquipementsAsync();
                SuccessMessage = "Type d'équipement modifié avec succès";
                ErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la modification du type d'équipement : {ex.Message}";
                SuccessMessage = null;
                return false;
            }
            finally
            {
                IsLoading = false;
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

        public async Task<bool> AddEquipementAsync(EquipementDTO equipement)
        {
            try
            {
                await _equipementServiceDTO.PostAsync(equipement);
                await LoadEquipementsAsync();
                SuccessMessage = "Equipement ajouté avec succès";
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
                SuccessMessage = "Equipement modifié avec succès";
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
                SuccessMessage = "Equipement supprimé avec succès";
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