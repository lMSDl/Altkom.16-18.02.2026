using ConsoleApp.Properties;

namespace ConsoleApp
{
    public class Garden
    {
        public int Size { get; set; }
        private ICollection<string> Items { get; } = [];
        private ILogger? Logger { get; set; }

        public Garden(int size)
        {
            Size = size;
        }
        public Garden(int size, ILogger logger) : this(size)
        {
            Logger = logger;
        }


        public bool Plant(string item)
        {
            Logger?.Log(Resources.PlangingStarted);

            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (string.IsNullOrWhiteSpace(item))
            {
                throw new ArgumentException(Resources.emptyStringException, nameof(item));
            }

            if (Items.Count >= Size)
            {
                Logger?.Log(string.Format(Resources.NoSpaceInGardenFor, item));
                return false;
            }

            if (Items.Contains(item))
            {
                item = AddDuplicationCounter(item);
            }

            Items.Add(item);
            Logger?.Log(string.Format(Resources.PlantedInGarden, item));
            return true;
        }

        private string AddDuplicationCounter(string item)
        {
            var newItem = item + (Items.Count(x => x == item || (x.StartsWith(item) && x.Length > item.Length && int.TryParse(x.Substring(item.Length), out _))) + 1);
            Logger?.Log(string.Format(Resources.PlantNameChanged, item, newItem));
            return newItem;
        }

        internal IEnumerable<string> GetItems()
        {
            return Items.ToList();
        }


        public string? GetLastLog()
        {
            string? log = Logger?.GetLogsAsync(DateTime.Now.AddMinutes(-10), DateTime.Now).Result;
            return log?.Split('\n').LastOrDefault();
        }
    }
}
