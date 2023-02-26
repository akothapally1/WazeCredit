namespace WazeCredit.Services.LifeTimeExample
{
    public class SingletonService
    {
        private readonly Guid guid;

        public SingletonService()
        {
            guid = Guid.NewGuid();
        }
        public string GetGuid()
        {
            return guid.ToString();
        }
    }
}
