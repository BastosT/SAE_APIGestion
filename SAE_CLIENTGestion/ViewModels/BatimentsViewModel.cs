﻿using CommunityToolkit.Mvvm.ComponentModel;
using SAE_CLIENTGestion.Models;
using SAE_CLIENTGestion.Services;

namespace SAE_CLIENTGestion.ViewModels
{
    public partial class BatimentsViewModel : ObservableObject
    {
        private readonly IService<Batiment> _batimentService;

        public BatimentsViewModel(IService<Batiment> batimentService)
        {
            _batimentService = batimentService;
        }

        [ObservableProperty]
        private bool _isLoading;

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
                Batiments = await _batimentService.GetAllAsync();
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

        public async Task<bool> AddBatimentAsync(Batiment batiment)
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

        public async Task<bool> UpdateBatimentAsync(Batiment batiment)
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
    }
}