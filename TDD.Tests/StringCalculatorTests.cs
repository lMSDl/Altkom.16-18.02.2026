namespace TDD.Tests
{
    public class StringCalculatorTests
    {
        /*[Fact]
        public void Add_EmptyString_Zero()
        {
            //Arrange
            var stringCalculator = new StringCalculator();
            var expected = 0;

            //Act
            var result = stringCalculator.Add("");

            //Assert
            Assert.Equal(expected, result);
        }


        [Fact]
        public void Add_SingleNumber_Number()
        {
            //Arrange
            var stringCalculator = new StringCalculator();
            var expected = 5;

            //Act
            var result = stringCalculator.Add("5");

            //Assert
            Assert.Equal(expected, result);
        }*/

        [Theory]
        [InlineData("", 0)]
        [InlineData(" ", 0)]
        [InlineData("5", 5)]
        [InlineData("23456", 23456)]
        [InlineData("123,456", 579)]
        [InlineData("123,456,3,2", 584)]
        public void Add_ReturnsSum(string input, int expected)
        {
            //Arrange
            var stringCalculator = new StringCalculator();

            //Act
            var result = stringCalculator.Add(input);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("-2")]
        [InlineData("-2, 2")]
        public void Add_WhenNegativeNumber_ArgumentException(string input)
        {
            //Arrange
            var stringCalculator = new StringCalculator();

            //Act
            Action action = () => stringCalculator.Add(input);

            //Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Theory]
        [InlineData("a", 0)]
        [InlineData("3,5,a", 8)]
        public void Add_WhenNonNumberValue_SkipNonNumberValues(string input, int expected)
        {
            //Arrange
            var stringCalculator = new StringCalculator();

            //Act
            var result = stringCalculator.Add(input);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}
