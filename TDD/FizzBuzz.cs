namespace TDD
{
    public class FizzBuzz
    {
        public static IEnumerable<string> GetSequence(int count)
        {
            return Enumerable.Range(1, count).Select(x => GetFizzBuzz(x));
        }

        public static string GetFizzBuzz(int value)
        {
            string result = string.Empty;

            if (IsFizz(value))
                result += "Fizz";
            if (IsBuzz(value))
                result += "Buzz";

            if (result == string.Empty)
                result = value.ToString();

            return result;
        }

        private static bool IsBuzz(int value)
        {
            return value % 5 == 0;
        }

        private static bool IsFizz(int value)
        {
            return value % 3 == 0;
        }
    }
}
