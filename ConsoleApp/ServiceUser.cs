namespace ConsoleApp
{
    public class ServiceUser
    {
        private readonly IService _service;

        public ServiceUser(IService service)
        {
            _service = service;
            _service.OnServiceStarted += Service_OnServiceStarted;
        }

        private void Service_OnServiceStarted(object? sender, EventArgs e)
        {
            IsServiceStarted = true;
        }

        public void SetServiceFriendlyName(string name)
        {
            _service.Name = name;
        }

        public bool ValidateUniqueId()
        {
            if (string.IsNullOrEmpty(_service.UniqueId))
            {
                return false;
            }
            return Guid.TryParse(_service.UniqueId, out _);
        }

        public void StartService()
        {
            _service.StartService();
        }

        public bool IsServiceStarted { get; private set; } = false;
    }
}
