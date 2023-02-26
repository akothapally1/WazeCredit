using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using WazeCredit.Data;
using WazeCredit.Models;
using WazeCredit.Models.ViewModels;
using WazeCredit.Services;
using WazeCredit.Utility.AppSettingsClasses;

namespace WazeCredit.Controllers
{
    public class HomeController : Controller
    {
        public HomeVM homeVM { get; set; }

        private readonly ApplicationDbContext _db;
        private readonly IMarketForecaster _marketForecaster;

        private readonly ICreditValidator _creditValidator;

        private readonly StripeSettings _stripeOptions;

        private readonly WazeForecastSettings _wazeOptions;

        private readonly SendGridSettings _sendGridOptions;

        private readonly TwilioSettings _twilioOptions;

        [BindProperty]
        public CreditApplication CreditModel { get; set; }
        public HomeController(IMarketForecaster marketForecaster ,IOptions<StripeSettings> stripeOptions,
            IOptions<WazeForecastSettings> wazeOptions , IOptions<SendGridSettings> sendGridOptions,IOptions<TwilioSettings> twilioOptions, ApplicationDbContext db,
            ICreditValidator creditValidator)
        {
             homeVM = new HomeVM();
            _marketForecaster = marketForecaster;
            _stripeOptions = stripeOptions.Value;
            _wazeOptions = wazeOptions.Value;
            _twilioOptions = twilioOptions.Value;
            _sendGridOptions = sendGridOptions.Value;
            _creditValidator = creditValidator;
            _db = db;
        }

        public IActionResult Index()
        {
            //MarketForecasterV2 marketForecaster = new MarketForecasterV2();
            MarketResult currentMarket = _marketForecaster.GetMarketPrediction();
            switch (currentMarket.MarketCondition)
            {
                case MarketCondition.StableDown:
                    homeVM.MarketForecast = "Market shows signs to go down in a stable state! It is a not a good sign to apply for credit applications! But extra credit is always piece of mind if you have handy when you need it.";
                    break;
                case MarketCondition.StableUp:
                    homeVM.MarketForecast = "Market shows signs to go up in a stable state! It is a great sign to apply for credit applications!";
                    break;
                case MarketCondition.Volatile:
                    homeVM.MarketForecast = "Market shows signs of volatility. In uncertain times, it is good to have credit handy if you need extra funds!";
                    break;
                default:
                    homeVM.MarketForecast = "Apply for a credit card using our application!";
                    break;
            }
            return View(homeVM);
        }

        public IActionResult AllConfigSettings()
        {
            List<string> messages = new List<string>();
            messages.Add($"Waze config - Forecast Tracker : " + _wazeOptions.ForecastTrackerEnabled);
            messages.Add($"Stripe config - SecretKey : " + _stripeOptions.SecretKey);
            messages.Add($"Stripe config - PublishableKey : "+_stripeOptions.PublishableKey);
            messages.Add($"Twilio config - PhoneNumber : " + _twilioOptions.PhoneNumber);
            messages.Add($"Twilio config - AuthToken : " + _twilioOptions.AuthToken);
            messages.Add($"Twilio config - AccountSid : " + _twilioOptions.AccountSid);
            messages.Add($"SendGrid config - SendGridKey : " + _sendGridOptions.SendGridKey);
            return View(messages);
        }

        public IActionResult CreditApplication()
        {
            CreditModel = new CreditApplication();
            return View(CreditModel);
        }

        public IActionResult CreditResult(CreditResult creditResult)
        {
          
            return View(creditResult);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("CreditApplication")]
        public async Task<IActionResult> CreditApplicationPOST()
        {
            if(ModelState.IsValid)
            {
                var (validationPassed,errorMessages) = await _creditValidator.passAllValidations(CreditModel);

                CreditResult creditResult = new CreditResult()
                {
                    ErrorList = errorMessages,
                    CreditID = 0,
                    Success = validationPassed
                };
                if(validationPassed)
                {
                    _db.Add(CreditModel);
                    _db.SaveChanges();
                    creditResult.CreditID = CreditModel.Id;
                    RedirectToAction(nameof(CreditResult), creditResult);
                }
                else
                {
                    return RedirectToAction(nameof(CreditResult), creditResult);
                }
            }
            return View(CreditModel);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}