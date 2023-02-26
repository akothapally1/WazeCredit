using WazeCredit.Models;

namespace WazeCredit.Services
{
    public class MarketForecasterV2 : IMarketForecaster

    {
        public MarketResult GetMarketPrediction()
        {
            return new MarketResult
            {
                MarketCondition = Models.MarketCondition.Volatile
            };
        }
    }

  
}
