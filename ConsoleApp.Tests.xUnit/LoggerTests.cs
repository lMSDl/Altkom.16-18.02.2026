using AutoFixture;
using System.Globalization;

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


        [Fact]
        public async Task GetLogsAsync_ValidateDateRange_LoggedMessage()
        {
            //Arrange
            const int LOG_SPLIT_COUNT = 2;
            const string LOG_SPLIT = ": ";
            const string EXPECTED_DATE_FORMAT = "dd.MM.yyyy HH:mm";
            var logger = new Logger();
            var fixture = new Fixture();
            var log = fixture.Create<string>();

            logger.Log(fixture.Create<string>());
            var dateTimeStart = DateTime.Now;
            logger.Log(log);
            var dateTimeEnd = DateTime.Now;
            logger.Log(fixture.Create<string>());

            //Act
            var result = await logger.GetLogsAsync(dateTimeStart, dateTimeEnd);

            //Assert
            var splittedResult = result.Split(LOG_SPLIT);
            Assert.Equal(LOG_SPLIT_COUNT, splittedResult.Length);
            Assert.Equal(log, splittedResult[1]);
            Assert.True(DateTime.TryParseExact(splittedResult[0], EXPECTED_DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
        }

    }
}
