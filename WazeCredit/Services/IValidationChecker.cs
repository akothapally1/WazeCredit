using WazeCredit.Models;

namespace WazeCredit.Services
{
    public interface IValidationChecker
    {
        bool ValidatorLogic( CreditApplication model);

        string ErrorMessage { get; }
    }
}
