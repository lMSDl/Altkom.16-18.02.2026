using AutoFixture;

namespace ConsoleApp.Test.NUnit
{
    //[Ignore("sth")]
    public class GardenTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }



        [Test]
        public void Plant_ValidName_True()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 1;
            const string NAME = "a"; 
            Garden garden = new Garden(MINIMAL_VALID_SIZE);

            //Act
            var result = garden.Plant(NAME);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
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
            Assert.That(result, Is.False);
        }

        //[Theory]
        [TestCase("sunflower", "sun")]
        [TestCase("róża czerwona", "róża")]
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
            Assert.That(garden.GetItems(), Does.Contain(expectedName));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
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
            Assert.That(garden.GetItems(), Does.Contain(expectedName));
        }

        [Test]
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
            Assert.That(garden.GetItems(), Does.Contain(expectedName));
        }

        [Test]
        public void Plant_NullName_ArgumentNullException()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            string? nullName = null;
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            string expectedParameter = "item";

            //Act
            TestDelegate testDelegate = () => garden.Plant(nullName!);

            //Assert
            var argumentNullException = Assert.Throws<ArgumentNullException>(testDelegate);
            //Assert.That(argumentNullException.ParamName, Is.EqualTo("item"));
            AssertException<ArgumentNullException>(testDelegate, expectedParameter);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\r")]
        [TestCase("\n")]
        [TestCase("\t")]
        [TestCase("   \n  \t")]
        //[Ignore("")]
        public void Plant_EmptyOrWhitespaceName_ArgumentException(string invalidName)
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            string expectedMessage = ConsoleApp.Properties.Resources.emptyStringException;
            string expectedParameter = "item";

            //Act
            TestDelegate testDelegate = () => garden.Plant(invalidName);

            //Assert
            AssertException<ArgumentException>(testDelegate, expectedParameter, expectedMessage);
        }

        private static void AssertException<T>(TestDelegate testDelegate, string expectedParameter, string? expectedMessage = null) where T : ArgumentException
        {
            var argumentException = Assert.Throws<T>(testDelegate);
            Assert.That(argumentException.ParamName, Is.EqualTo(expectedParameter));
            if (expectedMessage is not null)
                Assert.That(argumentException.Message, Does.Contain(expectedMessage));
        }

        [Test]
        public void GetItems_ReturnsCopyOfPlantsCollection()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            var items1 = garden.GetItems();

            //Act
            var items2 = garden.GetItems();

            //Arrange
            Assert.That(items1, Is.Not.SameAs(items2));
        }
    }
}
