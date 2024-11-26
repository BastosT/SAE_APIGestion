using CommunityToolkit.Mvvm.ComponentModel;
using SAE_CLIENTGestion.Models;
using SAE_CLIENTGestion.Services;

namespace SAE_CLIENTGestion.ViewModels
{
    public partial class EquipementsViewModel : ObservableObject
    {

        private readonly IService<Equipement> _equipementService;

        public EquipementsViewModel(IService<Equipement> equipementService)
        {
            _equipementService = equipementService;
        }

        [ObservableProperty] private bool _isLoading;
        [ObservableProperty] private List<Equipement> _equipements = new List<Equipement>();

        public async Task LoadDataAsync()
        {
            IsLoading = true;
            try
            {
                Equipements = await _equipementService.GetAllAsync();
            }
            finally
            {
                IsLoading = false;
            }
        }

    }
}
