namespace ConsoleApp
{
    public interface IService
    {
        string UniqueId { get; }
        string Name { get; set; }

        event EventHandler? OnServiceStarted;


        void StartService();
    }
}