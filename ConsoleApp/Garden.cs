using ConsoleApp.Properties;

namespace ConsoleApp
{
    public class Garden
    {
        public int Size { get; set; }
        private ICollection<string> Items { get; } = [];

        public Garden(int size)
        {
            Size = size;
        }

        public bool Plant(string item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (string.IsNullOrWhiteSpace(item))
            {
                throw new ArgumentException(Resources.emptyStringException, nameof(item));
            }

            if (Items.Count >= Size)
                return false;

            if (Items.Contains(item))
            {
                item += Items.Count(x => x == item || (x.StartsWith(item) && x.Length > item.Length && int.TryParse(x.Substring(item.Length), out _))) + 1;
            }

            Items.Add(item);
            return true;
        }

        public IEnumerable<string> GetItems()
        {
            return Items.ToList();
        }
    }
}
