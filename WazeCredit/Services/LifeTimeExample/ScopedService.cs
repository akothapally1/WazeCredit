namespace WazeCredit.Services.LifeTimeExample
{
    public class ScopedService
    {
        private readonly Guid guid;

        public ScopedService()
        {
            guid = Guid.NewGuid();
        }
        public string GetGuid()
        {
            return guid.ToString();
        }
    }
}
