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
    }
}
