using WazeCredit.Models;

namespace WazeCredit.Services
{
    public interface ICreditValidator
    {
        Task<(bool, IEnumerable<string>)> passAllValidations(CreditApplication model);
    }
}
