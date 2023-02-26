using WazeCredit.Models;

namespace WazeCredit.Services
{
    public interface IMarketForecaster
    {
        MarketResult GetMarketPrediction();
    }
}