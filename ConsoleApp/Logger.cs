namespace ConsoleApp
{
    public class Logger
    {
        private Dictionary<DateTime, string> _logs = [];
        public event EventHandler<LoggerEventArgs>? MessageLogged;

        public void Log(string message)
        {
            DateTime timestamp = DateTime.Now;
            _logs[timestamp] = message;

            MessageLogged?.Invoke(this, new LoggerEventArgs(timestamp, message));
        }


        public Task<string> GetLogsAsync(DateTime from, DateTime to)
        {
            return Task.Run(() => string.Join("\n", _logs.Where(x => x.Key >= from).Where(x => x.Key <= to)
                .Select(x => $"{x.Key.ToShortDateString()} {x.Key.ToShortTimeString()}: {x.Value}")));
        }


        public class LoggerEventArgs(DateTime timestamp, string message) : EventArgs
        {
            public DateTime Timestamp { get; } = timestamp;
            public string Message { get; } = message;
        }
    }
}
