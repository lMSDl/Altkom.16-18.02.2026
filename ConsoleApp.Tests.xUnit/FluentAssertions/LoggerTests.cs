using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;

namespace ConsoleApp.Tests.xUnit.FluentAssertions
{
    public class LoggerTests
    {
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
            logger.Log(log);

            //Assert
            using (new AssertionScope())
            {
                sender.Should().NotBeNull().And.Be(logger);
                eventArgs.Should().NotBeNull();
                eventArgs.Message.Should().Be(log);
                eventArgs.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            }
        }

        [Fact]
        public void Log_AnyInput_ValidLoggerEventArgs_FA()
        {
            //Arrange
            var logger = new Logger();
            var log = new Fixture().Create<string>();
            using var monitor = logger.Monitor();

            //Act
            logger.Log(log);

            //Assert
            using (new AssertionScope())
            {
                monitor.Should().Raise(nameof(logger.MessageLogged))
                    .WithSender(logger)
                    .WithArgs<Logger.LoggerEventArgs>(e => e.Message == log);
            }
        }
    }
}
