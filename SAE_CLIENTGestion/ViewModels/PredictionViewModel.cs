using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PredictionViewModel
{
    private readonly PredictionService _predictionService;

    public int Horizon { get; set; } = 10;
    public Dictionary<string, object> PredictionKnnData { get; private set; }
    public Dictionary<string, object> PredictionWithHorizonData { get; private set; }
    public bool IsLoading { get; private set; }
    public string ErrorMessage { get; private set; }

    public PredictionViewModel(PredictionService predictionService)
    {
        _predictionService = predictionService;
    }

    public async Task FetchPredictKnnAsync()
    {
        await FetchPredictionAsync(
            fetchFunction: () => _predictionService.FetchPredictKnnAsync(),
            setResult: result => PredictionKnnData = result
        );
    }

    public async Task FetchPredictionWithHorizonAsync()
    {
        await FetchPredictionAsync(
            fetchFunction: () => _predictionService.FetchPredictionWithHorizonAsync(Horizon),
            setResult: result => PredictionWithHorizonData = result
        );
    }

    private async Task FetchPredictionAsync(Func<Task<Dictionary<string, object>>> fetchFunction, Action<Dictionary<string, object>> setResult)
    {
        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            var result = await fetchFunction();
            setResult(result);
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
