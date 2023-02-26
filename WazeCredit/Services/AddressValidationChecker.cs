using WazeCredit.Models;

namespace WazeCredit.Services
{
    public class CreditValidationChecker : IValidationChecker
    {
        public string ErrorMessage => "Salary/Age requirements were not met ";

        public bool ValidatorLogic(CreditApplication model)
        {
            if(DateTime.Now.AddYears(-18) < model.DOB)
            {
                return false;
            }
            if(model.Salary <10000)
            {
                return false;
            }
            return true;
        }
    }
}
