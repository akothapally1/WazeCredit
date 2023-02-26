
namespace WazeCredit.Services.LifeTimeExample
{
    public class TransientService
    {
        private readonly Guid guid;

        public TransientService()
        {
            guid = Guid.NewGuid();
        }
        public string GetGuid()
        {
            return guid.ToString();
        }
    }
}
