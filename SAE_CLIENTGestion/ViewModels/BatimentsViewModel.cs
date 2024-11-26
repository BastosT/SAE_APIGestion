using CommunityToolkit.Mvvm.ComponentModel;
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

        [ObservableProperty] private bool _isLoading;
        [ObservableProperty] private List<Batiment> _batiments = new List<Batiment>();

        public async Task LoadDataAsync()
        {
            IsLoading = true;
            try
            {
                Batiments = await _batimentService.GetAllAsync();
            }
            finally
            {
                IsLoading = false;
            }
        }

    }
}
