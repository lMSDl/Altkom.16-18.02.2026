namespace TDD.Tests
{
    public class FizzBuzzTests
    {

        [Theory]
        [InlineData(1, "1")]
        [InlineData(14, "14")]
        [InlineData(3, "Fizz")]
        [InlineData(9, "Fizz")]
        [InlineData(5, "Buzz")]
        [InlineData(10, "Buzz")]
        [InlineData(15, "FizzBuzz")]
        [InlineData(30, "FizzBuzz")]
        public void GetFizzBuzz_AnyNumber_CorrectString(int input, string output)
        {
            //Act
            var result = FizzBuzz.GetFizzBuzz(input);

            //Assert
            Assert.Equal(output, result);
        }

        [Theory]
        [InlineData(1, new[] { "1" })]
        [InlineData(2, new[] { "1", "2" })]
        [InlineData(20, new[] { "1", "2", "Fizz", "4", "Buzz", "Fizz", "7", "8", "Fizz", "Buzz", "11", "Fizz", "13", "14", "FizzBuzz", "16", "17", "Fizz", "19", "Buzz" })]
        public void GetSequence(int count, IEnumerable<string> output)
        {
            //Act
            var result = FizzBuzz.GetSequence(count);

            //Assert
            Assert.Equal(output, result);
        }

    }
}
