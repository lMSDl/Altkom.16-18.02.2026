using AutoFixture;
using Moq;

namespace Shop.Tests
{
    public class OrderProcessorTests
    {
        [Fact]
        public void ProcessOrder_StockIsInsufficient_False()
        {
            //Arrange
            var inventoryService = new Mock<IInventoryService>();
            var orderRepository = new Mock<IOrderRepository>();
            var paymentService = new Mock<IPaymentService>();

            var orderProcessor = new OrderProcessor(orderRepository.Object, inventoryService.Object, paymentService.Object);

            var fixture = new Fixture();
            var order = fixture.Create<Order>();
            var cardNumber = fixture.Create<string>();

            var lastItem = order.Items.Last();

            //inventoryService.Setup(x => x.CheckStock(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            //var otherItems = order.Items.Take(order.Items.Count - 1).ToList();
            //otherItems.ForEach(item => inventoryService.Setup(x => x.CheckStock(item.ProductId, item.Quantity)).Returns(true)); //dokładne dopasowanie parametrów
            
            inventoryService.Setup(x => x.CheckStock(It.IsAny<int>(), It.IsAny<int>())).Returns(true); //ustawienie domyślne dla wszystkich wywołań, które nie mają dokładnego dopasowania parametrów
            inventoryService.Setup(x => x.CheckStock(lastItem.ProductId, lastItem.Quantity)).Returns(false);

            //zapis używamy jeśli chcemy w assert tylko wywołać Verify()
            //otherItems.ForEach(item => inventoryService.Setup(x => x.CheckStock(item.ProductId, item.Quantity)).Returns(true).Verifiable(Times.Once));
            //inventoryService.Setup(x => x.CheckStock(lastItem.ProductId, lastItem.Quantity)).Returns(false).Verifiable(Times.Once);

            //Act
            var result = orderProcessor.ProcessOrder(order, cardNumber);

            //Assert
            inventoryService.Verify(x => x.CheckStock(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(order.Items.Count));
            //inventoryService.Verify(); //veryfikacja mniej czytelna - nie wiemy które wywołania są sprawdzane, póki nie sprawdzimy arrange

            //zapewniamy, że niechciane akcje nie zostały wykonane
            orderRepository.Verify(x => x.SaveOrder(It.IsAny<Order>()), Times.Never);
            inventoryService.Verify(x => x.ReserveStock(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            paymentService.Verify(x => x.ProcessPayment(It.IsAny<string>(), It.IsAny<decimal>()), Times.Never);

            Assert.False(result);
        }

        [Fact]
        public void ProcessOrder_PaymentFailed_False()
        {
            //Arrange
            var inventoryService = new Mock<IInventoryService>();
            var orderRepository = new Mock<IOrderRepository>();
            var paymentService = new Mock<IPaymentService>();

            var orderProcessor = new OrderProcessor(orderRepository.Object, inventoryService.Object, paymentService.Object);

            var fixture = new Fixture();
            var order = fixture.Create<Order>();
            var cardNumber = fixture.Create<string>();

            inventoryService.Setup(x => x.CheckStock(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            paymentService.Setup(x => x.ProcessPayment(cardNumber, It.IsAny<decimal>())).Returns(false);

            //Act
            var result = orderProcessor.ProcessOrder(order, cardNumber);

            //Assert
            paymentService.Verify(x => x.ProcessPayment(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once);
            orderRepository.Verify(x => x.SaveOrder(It.IsAny<Order>()), Times.Never);
            inventoryService.Verify(x => x.ReserveStock(It.IsAny<int>(), It.IsAny<int>()), Times.Never);

            Assert.False(result);
        }

        [Fact]
        public void ProcessOrder_ConditionsMet_True()
        {
            //Arrange
            var inventoryService = new Mock<IInventoryService>();
            var orderRepository = new Mock<IOrderRepository>();
            var paymentService = new Mock<IPaymentService>();

            var orderProcessor = new OrderProcessor(orderRepository.Object, inventoryService.Object, paymentService.Object);

            var fixture = new Fixture();
            var order = fixture.Create<Order>();
            var cardNumber = fixture.Create<string>();

            inventoryService.Setup(x => x.CheckStock(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            paymentService.Setup(x => x.ProcessPayment(It.IsAny<string>(), It.IsAny<decimal>())).Returns(true);

            //Act
            var result = orderProcessor.ProcessOrder(order, cardNumber);

            //Assert

            inventoryService.Verify(x => x.ReserveStock(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(order.Items.Count));
            //inventoryService.Verify(x => x.ReserveStock(It.IsIn(order.Items.Select(i => i.ProductId)), It.IsIn(order.Items.Select(i => i.Quantity))), Times.Exactly(order.Items.Count));
            //order.Items.ForEach(x => inventoryService.Verify(s => s.ReserveStock(x.ProductId, x.Quantity), Times.Once));

            paymentService.Verify(x => x.ProcessPayment(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once);
            orderRepository.Verify(x => x.SaveOrder(order), Times.Once);

            Assert.True(result);
        }

        public static IEnumerable<object[]> OrderItemsData =>
       [
           [new OrderItem[] { new() { Quantity = 1, UnitPrice = 30 }, new() { Quantity = 10, UnitPrice = 20 }, new() { Quantity = 2, UnitPrice = 5 } }, 240m],
            [new OrderItem[] { new() { Quantity = 2, UnitPrice = 30 }, new() { Quantity = 10, UnitPrice = 20 }, new() { Quantity = 2, UnitPrice = 5 } }, 270m],
            [new OrderItem[] { new() { Quantity = 1, UnitPrice = 30 }, new() { Quantity = 2, UnitPrice = 5 } }, 40m]
       ];

        /* [Theory]
         [MemberData(nameof(OrderItemsData))]
         public void ProcessOrder_CorrectTotalPaymentAmount(IEnumerable<OrderItem> items, decimal expectedTotal)
         {
             //Arrange
             var inventoryService = new Mock<IInventoryService>();
             var orderRepository = new Mock<IOrderRepository>();
             var paymentService = new Mock<IPaymentService>();

             var orderProcessor = new OrderProcessor(orderRepository.Object, inventoryService.Object, paymentService.Object);

             var fixture = new Fixture();
             var order = fixture.Build<Order>().With(x => x.Items, [.. items]).Create();
             var cardNumber = fixture.Create<string>();

             inventoryService.Setup(x => x.CheckStock(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

             //Act
             orderProcessor.ProcessOrder(order, cardNumber);

             //Assert
             paymentService.Verify(x => x.ProcessPayment(cardNumber, expectedTotal));

         }*/

        [Theory]
        [MemberData(nameof(OrderItemsData))]
        public void CalculateTotal_CorrectSum(IEnumerable<OrderItem> items, decimal expectedTotal)
        {
            //Act
            var result = OrderProcessor.CalculateTotal(items);

            //Assert
            Assert.Equal(expectedTotal, result);
        }
    }
}
