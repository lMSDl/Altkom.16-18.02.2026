using AutoFixture;

namespace ConsoleApp.Tests.xUnit
{
    public class LoggerTests
    {

        [Fact]
        public void Log_AnyInput_EventInvokedOnce()
        {
            //Arrange
            var logger = new Logger();
            var log = new Fixture().Create<string>();
            var eventInvoked = 0;
            logger.MessageLogged += (_, _) => eventInvoked++;
            const int EXPECTED_RESULT = 1;

            //Act
            logger.Log(log);

            //Assert
            Assert.Equal(EXPECTED_RESULT, eventInvoked);
        }

        [Fact]
        public void Log_AnyInput_ValidLoggerEventArgs()
        {
            //Arrange
            var logger = new Logger();
            var log = new Fixture().Create<string>();
            object? sender = null;
            Logger.LoggerEventArgs? eventArgs = null;
            logger.MessageLogged += (s, e) => { sender = s; eventArgs = e; };


            //Act
            var dateTimeStart = DateTime.Now;
            logger.Log(log);
            var dateTimeEnd = DateTime.Now;

            //Assert
            Assert.NotNull(sender);
            Assert.NotNull(eventArgs);
            Assert.Equal(logger, sender);
            Assert.Equal(log, eventArgs.Message);
            Assert.InRange(eventArgs.Timestamp, dateTimeStart, dateTimeEnd);
        }

    }
}
