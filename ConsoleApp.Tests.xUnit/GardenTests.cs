using AutoFixture;

namespace ConsoleApp.Tests.xUnit
{
    public class GardenTests : IDisposable
    {
        //używanie metod setup i teardown w testach jednostkowych jest niezalecane,
        //ponieważ testy powinny być niezależne i nie powinny mieć efektów ubocznych.
        //metody setup i teardown są wywoływane przed i po KAŻDYM teście
        //Zamiast tego możemy używać metod pomocniczych (np. "Garden CreateEmptyGarden()" lub "Garden CreateGarden(int minmalValidSize)")

        //xUnit nie posiada metod setup, ale możemy użyć konstruktora klasy testowej
        public GardenTests()
        {
        }

        // xUnit nie posiada metod teardown, ale możemy użyć metody Dispose
        public void Dispose()
        {
        }



        [Fact]
        //public void Plant_GivesTrueWhenNameIsValid()
        //public void Plant_PassValidName_ReturnsTrue()
        //<nazwa_metody>_<scenariusz>_<oczekiwany_wynik>
        public void Plant_ValidName_True()
        {
            //Arrange
            //opisujemy intencje
            //nie używamy "magicznych" wartości, tylko takie, które mają znaczenie dla testu
            const int MINIMAL_VALID_SIZE = 1; //używamy parametu, który daje minimalny ale wystarczający rozmiar ogrodu, aby test był ważny
            const string NAME = "a"; //używamy parametru, który daje minimalny przekaz (jak najmniejsze pole do interpretacji) 
            Garden garden = new Garden(MINIMAL_VALID_SIZE);

            //Act
            var result = garden.Plant(NAME);

            //Assert
            Assert.True(result);
        }

        [Fact]
        //public void Plant_GivesFalseWhenGardenIsFull()
        //public void Plant_PassValidNameButGardenIsFull_ReturnsFalse()
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
            Assert.False(result);
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
            Assert.Contains(expectedName, garden.GetItems());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        public void Plant_WhenNameIsDuplicated_DuplicationCounterAddedToName(int numberOfCopies)
        {
            //Arrange
            int expectedCounter = numberOfCopies + 1; //oczekujemy, że licznik duplikacji będzie o 1 większy niż liczba istniejących już kopii
            string name = new Fixture().Create<string>();
            string expectedName = name + expectedCounter;
            Garden garden = new Garden(expectedCounter);

            //staramy się unikać pętli, jeśli już to używamy LINQ (unikamy wtedy logiki)
            Enumerable.Repeat(name, numberOfCopies)
                .ToList()
                .ForEach(x => garden.Plant(x));

            //Act
            garden.Plant(name);

            //Assert
            Assert.Contains(expectedName, garden.GetItems());
        }

        [Fact]
        //public void Plant_ChangesNameWhenDuplicatedItem()
        //public void Plant_WhenNameIsDuplicated_DuplicationCounterAddedToName()
        public void Plant_DuplicatedName_NameChanged()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 2;
            string name = new Fixture().Create<string>();
            string expectedName = name + "2"; //oczekujemy, że zostanie dodany licznik duplikacji do nazwy, jeśli jest ona już zajęta
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            garden.Plant(name);

            //Act
            garden.Plant(name);

            //Assert
            Assert.Contains(expectedName, garden.GetItems());
            //powinniśmy unikać asercji wielu rzeczy w jednym teście
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
            //var argumentNullException = Assert.ThrowsAny<ArgumentException>(action); //uwzględnia hierarchię dziedziczenia wyjątku
            //var argumentNullException = Assert.Throws<ArgumentNullException>(action); //tylko konkretny wyjątek
            //Assert.Equal("item", argumentNullException.ParamName); //wiele asercji w jednym teście jest niezalecane, ale w tym przypadku sprawdzamy, czy wyjątek został rzucony z odpowiednim parametrem, więc jest to uzasadnione - sprawdzamy jedną rzecz pod różnymi kątami

            AssertException<ArgumentNullException>(action, expectedParameter);
        }

        public static IEnumerable<object[]> GetInvalidNames()
        {
            yield return new object[] { "" };
            yield return new object[] { " " };
            yield return new object[] { "\r" };
            yield return new object[] { "\n" };
            yield return new object[] { "\t" };
            yield return new object[] { "   \n  \t" };
        }

        //Theory - testy z parametrami, które pozwalają na uruchomienie tej samej metody z różnymi danymi sprawdzającymi różne warunki (w tym brzegowe)
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\r")]
        [InlineData("\n")]
        [InlineData("\t")]
        [InlineData("   \n  \t")]
        //[MemberData(nameof(GetInvalidNames))]
        //[ClassData(typeof(GardenInvalidNames))] // klasa musi implementować interfejs IEnumerable<object[]>
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


        [Fact(Skip = "Replaced by Plant_EmptyOrWhitespaceName_ArgumentException")]
        public void Plant_EmptyName_ArgumentException()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            string? emptyName = string.Empty;
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            string expectedMessage = ConsoleApp.Properties.Resources.emptyStringException; //oczekujemy, że zostanie rzucony wyjątek z odpowiednią wiadomością, jeśli nazwa jest pusta
            string expectedParameter = "item";

            //Act
            Action action = () => garden.Plant(emptyName!);

            //Assert
            AssertException<ArgumentException>(action, expectedParameter, expectedMessage);
        }

        [Fact(Skip = "Replaced by Plant_EmptyOrWhitespaceName_ArgumentException")]
        public void Plant_WhitespaceName_ArgumentException()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            string whitespace = "   ";
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            string expectedMessage = ConsoleApp.Properties.Resources.emptyStringException;
            string expectedParameter = "item";
            //Act
            Action action = () => garden.Plant(whitespace);

            //Assert
            AssertException<ArgumentException>(action, expectedParameter, expectedMessage);
            //var argumentNullException = Assert.Throws<ArgumentException>(action);
            //Assert.Equal("item", argumentNullException.ParamName);
            //Assert.Contains(expectedMessage, argumentNullException.Message);
        }


        private static void AssertException<T>(Action action, string expectedParameter, string? expectedMessage = null) where T : ArgumentException
        {
            var argumentException = Assert.Throws<T>(action);
            Assert.Equal(expectedParameter, argumentException.ParamName);
            if (expectedMessage is not null)
                Assert.Contains(expectedMessage, argumentException.Message);
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
            Assert.NotSame(items1, items2); //sprawdzamy, czy GetItems zwraca kopię kolekcji, a nie referencję do tej samej kolekcji
        }

    }
}
