using System.Text;

namespace ConsoleApp
{
    public class SentenceUtils
    {
        public static string ToTitleCase(string sentence)
        {
            if (sentence == null || sentence.Length == 0)
            {
                return sentence;
            }

            StringBuilder result = new StringBuilder(sentence);
            result[0] = char.ToUpper(result[0]);
            for (int i = 1; i < result.Length; i++)
            {
                result[i] = char.IsWhiteSpace(result[i - 1]) ? char.ToUpper(result[i]) : char.ToLower(result[i]);
            }

            return result.ToString();
        }
    }
}
