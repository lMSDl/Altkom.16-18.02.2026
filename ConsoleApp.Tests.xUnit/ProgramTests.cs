using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ConsoleApp.Tests.xUnit
{
    public class ProgramTests
    {
        [Fact]
        public void Main_ConsoleOutput_HelloWorld()
        {
            //Arrange
            const string EXPECTED_OUTPUT = "Hello, World!";
            var main = typeof(Program).Assembly.EntryPoint;

            TextWriter consoleWriter = Console.Out;
            TextWriter textWriter = new StringWriter();
            Console.SetOut(textWriter);

            //Act
            main.Invoke(null, [Array.Empty<string>()]);

            //Assert
            Console.SetOut(consoleWriter);
            Assert.Equal(EXPECTED_OUTPUT, textWriter.ToString().Trim());
        }
    }
}
