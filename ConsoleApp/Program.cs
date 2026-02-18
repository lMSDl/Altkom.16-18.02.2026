using System.Runtime.CompilerServices;
//InternalsVisibleTo - udostępniamy internale dla projektu testowego
[assembly: InternalsVisibleTo("ConsoleApp.Tests.xUnit")]
[assembly: InternalsVisibleTo("ConsoleApp.Tests.MSTest")]
[assembly: InternalsVisibleTo("ConsoleApp.Test.NUnit")]

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//upublicznienie klasy Program, aby można było ją testować w projekcie testowym - alternatywa dla InternalsVisibleTo 
//public partial class Program;
