using System.Collections;

namespace ConsoleApp.Tests.xUnit
{
    internal class GardenInvalidNames : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "" };
            yield return new object[] { " " };
            yield return new object[] { "\r" };
            yield return new object[] { "\n" };
            yield return new object[] { "\t" };
            yield return new object[] { "   \n  \t" };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
