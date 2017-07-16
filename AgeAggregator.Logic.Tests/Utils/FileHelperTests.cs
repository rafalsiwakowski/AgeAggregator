using AgeAggregator.Logic.Utils;
using NUnit.Framework;
using System.IO;

namespace AgeAggregator.Logic.Tests.Utils
{
    [TestFixture]
    public class FileHelperTests
    {
        FileHelper _objectUnderTest;

        [SetUp]
        public void SetUp()
        {
            _objectUnderTest = new FileHelper();
        }

        [Test]
        public void Should_ReadFileContent_When_FilePathProvided()
        {
            var testFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Utils", "testFile.txt");
            
            var content = _objectUnderTest.ReadFile(testFilePath);

            Assert.AreEqual("testContent", content);
        }

        [Test]
        public void Should_WriteFileContent_When_PathAndContentProvided()
        {
            var testFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "newFile.txt");

            _objectUnderTest.SaveFile(testFilePath, "newFileContent");

            Assert.AreEqual("newFileContent", _objectUnderTest.ReadFile(testFilePath));

            File.Delete(testFilePath);
        }
    }
}
