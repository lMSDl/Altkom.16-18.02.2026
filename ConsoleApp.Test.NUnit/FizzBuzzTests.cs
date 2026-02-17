namespace ConsoleApp.Test.NUnit
{
    internal class FizzBuzzTests
    {
        [Test]
        public void ReturnOne()
        {
            Assert.That(FizzBuzz.Compute(1), Is.EqualTo(new List<string> { "1" }));
        }

        [Test]
        public void ReturnFifteen()
        {
            int n = 15;
            List<string> result = FizzBuzz.Compute(n);
            List<string> expected = new List<string> {
                "1", "2", "Fizz", "4", "Buzz",
                "Fizz", "7", "8", "Fizz", "Buzz",
                "11", "Fizz", "13", "14", "FizzBuzz"
            };

            Assert.That(result.Count,
                        Is.EqualTo(expected.Count),
                        string.Format("Your function should return {0} objects for n = {1}", n, n));

            for (int i = 0; i < n; i++)
            {
                Assert.That(result[i],
                            Is.EqualTo(expected[i]),
                            string.Format("Your function should convert the value {0} to {1} instead of {2}",
                                          i + 1, expected[i], result[i]));
            }
        }

        [TestCase(1, new[] { "1" })]
        [TestCase(15, new[] { "1", "2", "Fizz", "4", "Buzz", "Fizz", "7", "8", "Fizz", "Buzz", "11", "Fizz", "13", "14", "FizzBuzz" })]
        [TestCase(20, new[] { "1", "2", "Fizz", "4", "Buzz", "Fizz", "7", "8", "Fizz", "Buzz", "11", "Fizz", "13", "14", "FizzBuzz", "16", "17", "Fizz", "19", "Buzz" })]
        public void Compute(int count, IEnumerable<string> expectedOutput)
        {
            //Act
            var result = FizzBuzz.Compute(count);
            //Assert
            Assert.That(result, Is.EqualTo(expectedOutput));
        }

        [TestCase(1, "Fizz", 0)]
        [TestCase(1, "Buzz", 0)]
        [TestCase(15, "Fizz", 5)]
        [TestCase(15, "Buzz", 3)]
        [TestCase(100, "Fizz", 33)]
        [TestCase(100, "Buzz", 20)]
        public void Compute_AnyInt_CorrectFizzBuzzCount(int count, string fizzBuzz, int expectedCount)
        {
            //Act
            var result = FizzBuzz.Compute(count);

            //Assert
            int actualCount = result.Count(s => s.Contains(fizzBuzz));
            Assert.That(actualCount, Is.EqualTo(expectedCount));
        }

        [TestCase(1)]
        [TestCase(15)]
        [TestCase(100)]
        public void Compute_AnyInt_NonFizzBuzzValues(int count)
        {
            //Arrange
            const string Fizz = "Fizz";
            const string Buzz = "Buzz";
            var expected = Enumerable.Range(1, count).Select(x => x.ToString()).ToArray();

            //Act
            var result = FizzBuzz.Compute(count);

            //Assert
            var zip = result.Zip(expected);
            Assert.That(zip.Where(x => !x.First.Contains(Fizz)).Where(x => !x.First.Contains(Buzz)).All(x => x.First.Equals(x.Second)));
        }
    }
}
