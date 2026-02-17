using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;

namespace ConsoleApp.Tests.xUnit.FluentAssertions
{
    public class GardenTests 
    {
        [Fact]
        public void Plant_ValidName_True()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 1; 
            const string NAME = "a"; 
            Garden garden = new Garden(MINIMAL_VALID_SIZE);

            //Act
            var result = garden.Plant(NAME);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Plant_GardenFull_False()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            //można wykorzysać AutoFixture do generowania danych testowych
            string name = new Fixture().Create<string>();
            Garden garden = new Garden(MINIMAL_VALID_SIZE);

            //Act
            var result = garden.Plant(name);

            //Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("sunflower", "sun")]
        [InlineData("róża czerwona", "róża")]
        public void Plant_WhenNameIsSubstringOfAnotherName_DuplicationCounterNotAddedToName(string arrangeName, string actName)
        {
            //Arrange
            const int MININAL_VALID_SIZE = 3;
            Garden garden = new Garden(MININAL_VALID_SIZE);
            garden.Plant(actName);
            garden.Plant(arrangeName);
            string expectedName = actName + 2;

            //Act
            garden.Plant(actName);

            //Assert
            garden.GetItems().Should().Contain(expectedName);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        public void Plant_WhenNameIsDuplicated_DuplicationCounterAddedToName(int numberOfCopies)
        {
            //Arrange
            int expectedCounter = numberOfCopies + 1;
            string name = new Fixture().Create<string>();
            string expectedName = name + expectedCounter;
            Garden garden = new Garden(expectedCounter);

            Enumerable.Repeat(name, numberOfCopies)
                .ToList()
                .ForEach(x => garden.Plant(x));

            //Act
            garden.Plant(name);

            //Assert
            garden.GetItems().Should().Contain(expectedName);
        }

        [Fact]
        public void Plant_DuplicatedName_NameChanged()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 2;
            string name = new Fixture().Create<string>();
            string expectedName = name + "2"; 
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            garden.Plant(name);

            //Act
            garden.Plant(name);

            //Assert
            garden.GetItems().Should().Contain(expectedName);
        }

        [Fact]
        public void Plant_NullName_ArgumentNullException()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            string? nullName = null;
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            string expectedParameter = "item";

            //Act
            Action action = () => garden.Plant(nullName!);

            //Assert
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName(expectedParameter);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\r")]
        [InlineData("\n")]
        [InlineData("\t")]
        [InlineData("   \n  \t")]
        public void Plant_EmptyOrWhitespaceName_ArgumentException(string invalidName)
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            string expectedMessage = ConsoleApp.Properties.Resources.emptyStringException;
            string expectedParameter = "item";

            //Act
            Action action = () => garden.Plant(invalidName);

            //Assert
            AssertException<ArgumentException>(action, expectedParameter, expectedMessage);
        }

        private static void AssertException<T>(Action action, string expectedParameter, string? expectedMessage = null) where T : ArgumentException
        {
            using (new AssertionScope())
            {
                action.Should().Throw<ArgumentException>()
                    .WithParameterName(expectedParameter)
                    .WithMessage(expectedMessage + "*" ?? "*") //wild card patterns
                    ;
            }
        }

        [Fact]
        public void GetItems_ReturnsCopyOfPlantsCollection()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            var items1 = garden.GetItems();

            //Act
            var items2 = garden.GetItems();
        
            //Arrange
            items1.Should().NotBeSameAs(items2);
        }

    }
}
