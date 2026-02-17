using AutoFixture;

namespace ConsoleApp.Tests.MSTest
{
    [TestClass]
    public sealed class GardenTests
    {
        [TestInitialize]
        public void Setup()
        {
        }

        [TestCleanup]
        public void Teardown()
        {
        }

        [TestMethod]
        public void Plant_ValidName_True()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 1; 
            const string NAME = "a"; 
            Garden garden = new Garden(MINIMAL_VALID_SIZE);

            //Act
            var result = garden.Plant(NAME);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
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
            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow("sunflower", "sun")]
        [DataRow("róża czerwona", "róża")]
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
            //Assert.IsTrue(garden.GetItems().Contains(actName));
            //CollectionAssert.Contains(garden.GetItems().ToArray(), actName);
            Assert.Contains(actName, garden.GetItems());
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(4)]
        public void Plant_WhenNameIsDuplicated_DuplicationCounterAddedToName(int numberOfCopies)
        {
            //Arrange
            int expectedCounter = numberOfCopies + 1; //oczekujemy, że licznik duplikacji będzie o 1 większy niż liczba istniejących już kopii
            string name = new Fixture().Create<string>();
            string expectedName = name + expectedCounter;
            Garden garden = new Garden(expectedCounter);

            Enumerable.Repeat(name, numberOfCopies)
                .ToList()
                .ForEach(x => garden.Plant(x));

            //Act
            garden.Plant(name);

            //Assert
            Assert.Contains(expectedName, garden.GetItems());
        }

        [TestMethod]
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
            Assert.Contains(expectedName, garden.GetItems());
        }

        [TestMethod]
        //w poprzednich wersjach MSTest można było użyć atrybutów ExpectedException, ale nie pozwalały one na sprawdzenie dodatkowych informacji o wyjątku, takich jak nazwa parametru czy wiadomość wyjątku, dlatego lepszym rozwiązaniem jest użycie Assert.Throws i sprawdzenie tych informacji w osobnych asercjach
        //[ExpectedException(typeof(ArgumentNullException))]
        //[ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        public void Plant_NullName_ArgumentNullException()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            string? nullName = null;
            Garden garden = new Garden(MINIMAL_VALID_SIZE);

            //Act
            Action action = () => garden.Plant(nullName!);

            //Assert
            var argumentNullException = Assert.Throws<ArgumentNullException>(action);
            Assert.AreEqual("item", argumentNullException.ParamName);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\r")]
        [DataRow("\n")]
        [DataRow("\t")]
        [DataRow("   \n  \t")]
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
            var argumentException = Assert.Throws<T>(action);
            Assert.AreEqual(expectedParameter, argumentException.ParamName);
            if (expectedMessage is not null)
                //StringAssert.Contains(expectedMessage, argumentException.Message);
                Assert.Contains(expectedMessage, argumentException.Message);
        }

        [TestMethod]
        public void GetItems_ReturnsCopyOfPlantsCollection()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            var items1 = garden.GetItems();

            //Act
            var items2 = garden.GetItems();

            //Arrange
            Assert.AreNotSame(items1, items2); //sprawdzamy, czy GetItems zwraca kopię kolekcji, a nie referencję do tej samej kolekcji
        }
    }
}
