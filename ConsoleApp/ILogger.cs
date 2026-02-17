namespace ConsoleApp
{
    public interface ILogger
    {
        void Log(string message);
        Task<string> GetLogsAsync(DateTime from, DateTime to);
    }
}
