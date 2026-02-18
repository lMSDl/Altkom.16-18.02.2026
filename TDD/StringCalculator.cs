namespace TDD
{
    public class StringCalculator
    {
        public int Add(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            var numbers = Parse(input);
            Validate(numbers);

            return numbers.Sum();
        }

        private static void Validate(IEnumerable<int> numbers)
        {
            if (numbers.Any(x => x < 0))
                throw new ArgumentException("Negative numbers not allowed");
        }

        private IEnumerable<int> Parse(string input)
        {
            return input.Split(',').Select(x => int.TryParse(x, out var result) ? result : 0).ToArray();
        }

    }
}
