using System.Collections.Generic;
using System.Threading.Tasks;

public class PredictionViewModel
{
    private readonly PredictionService _predictionService;

    public int Horizon { get; set; } = 10;
    public Dictionary<string, object> PredictionData { get; private set; }
    public bool IsLoading { get; private set; }
    public string ErrorMessage { get; private set; }

    public PredictionViewModel(PredictionService predictionService)
    {
        _predictionService = predictionService;
    }

    public async Task FetchPredictKnnAsync()
    {
        await FetchPredictionAsync(() => _predictionService.FetchPredictKnnAsync());
    }

    public async Task FetchPredictionWithHorizonAsync()
    {
        await FetchPredictionAsync(() => _predictionService.FetchPredictionWithHorizonAsync(Horizon));
    }

    private async Task FetchPredictionAsync(Func<Task<Dictionary<string, object>>> fetchFunction)
    {
        IsLoading = true;
        PredictionData = null;
        ErrorMessage = string.Empty;

        try
        {
            PredictionData = await fetchFunction();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }
}
