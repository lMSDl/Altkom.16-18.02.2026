using AutoFixture;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Tests.xUnit
{
    public class ServiceUserTests
    {

        [Fact]
        public void SetServiceFriendlyName_ShouldSetName()
        {
            //Arrange
            var serviceMock = new Mock<IService>();
            var serviceUser = new ServiceUser(serviceMock.Object);

            string expectedName = new Fixture().Create<string>();
            //serviceMock.SetupProperty(s => s.Name);

            //Act
            serviceUser.SetServiceFriendlyName(expectedName);
        
            //Assert
            serviceMock.VerifySet(s => s.Name = expectedName, Times.Once);
            //Assert.Equal(expectedName, serviceMock.Object.Name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ValidateUniqueId_UniqueIdIsNullOrEmpty_False(string? input)
        {
            //Arrange
            var serviceMock = new Mock<IService>();
            var serviceUser = new ServiceUser(serviceMock.Object);

            serviceMock.SetupGet(s => s.UniqueId).Returns(input);

            //Act
            var result = serviceUser.ValidateUniqueId();

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateUniqueId_UniqueIdIsValidGuid_True()
        {
            //Arrange
            var serviceMock = new Mock<IService>();
            var serviceUser = new ServiceUser(serviceMock.Object);

            string validGuid = Guid.NewGuid().ToString();

            serviceMock.SetupGet(x => x.UniqueId).Returns(validGuid);

            //Act
            var result = serviceUser.ValidateUniqueId();

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void StartService_InvokesStartService()
        {

            //Arrange
            var serviceMock = new Mock<IService>();
            var serviceUser = new ServiceUser(serviceMock.Object);

            //Act
            serviceUser.StartService();

            //Assert
            serviceMock.Verify(s => s.StartService(), Times.Once);
        }

        [Fact]
        public void StartService_SetsIsServiceStarted()
        {
            //Arrange
            var serviceMock = new Mock<IService>();
            var serviceUser = new ServiceUser(serviceMock.Object);

            serviceMock.Raise(x => x.OnServiceStarted += default, EventArgs.Empty);

            //Act
            serviceUser.StartService();

            //Assert
            Assert.True(serviceUser.IsServiceStarted);

        }
    }
}
