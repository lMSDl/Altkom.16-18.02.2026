namespace ConsoleApp.Test.NUnit
{
    internal class SentenceUtilsTests
    {
        [Test]
        public void Character()
        {
            Assert.That(SentenceUtils.ToTitleCase("a"),
                        Is.EqualTo("A"),
                        "Your function should convert a single character.");
            Assert.That(SentenceUtils.ToTitleCase("A"),
                        Is.EqualTo("A"),
                        "Your function should convert a single character.");
        }

        [Test]
        public void Word()
        {
            Assert.That(SentenceUtils.ToTitleCase("abc"),
                        Is.EqualTo("Abc"),
                        "Your function should convert a single word.");
            Assert.That(SentenceUtils.ToTitleCase("ABC"),
                        Is.EqualTo("Abc"),
                        "Your function should convert a single word.");
            Assert.That(SentenceUtils.ToTitleCase("aBC"),
                        Is.EqualTo("Abc"),
                        "Your function should convert a single word.");
            Assert.That(SentenceUtils.ToTitleCase("Abc"),
                        Is.EqualTo("Abc"),
                        "Your function should convert a single word.");
        }

        [Test]
        public void Sentence()
        {
            Assert.That(SentenceUtils.ToTitleCase("abc def ghi"),
                        Is.EqualTo("Abc Def Ghi"),
                        "Your function should convert a sentence.");
        }

        [Test]
        public void MultipleWhitespace()
        {
            Assert.That(SentenceUtils.ToTitleCase("  abc  def ghi "),
                        Is.EqualTo("  Abc  Def Ghi "),
                        "Your function should keep multiple whitespaces.");
        }


        [TestCase("a", "A")]
        [TestCase("A", "A")]
        [TestCase("abc", "Abc")]
        [TestCase("ABC", "Abc")]
        [TestCase("aBc", "Abc")]
        [TestCase("Abc", "Abc")]
        [TestCase("abC", "Abc")]
        [TestCase("abc de fgh", "Abc De Fgh")]
        [TestCase("    abc   de\t fgh", "    Abc   De\t Fgh")]
        public void ToTitleCase_AnyInput_TitleCaseOuput(string input, string output)
        {
            // Act
            var result = SentenceUtils.ToTitleCase(input);
            // Assert
            Assert.That(result, Is.EqualTo(output));
        }
    }
}
