using AgeAggregator.Logic.Utils;
using NUnit.Framework;
using System.IO;

namespace AgeAggregator.Logic.Tests.Utils
{
    [TestFixture]
    public class PathHelperTests
    {
        PathHelper _objectUnderTest;

        [SetUp]
        public void SetUp()
        {
            _objectUnderTest = new PathHelper();
        }

        [Test]
        public void Should_GetFullFilePath_When_FileExists()
        {
            var testFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Utils", "testFile.txt");

            Assert.AreEqual(Path.GetFullPath(testFilePath), _objectUnderTest.GetFullPath(testFilePath));
        }

        [Test]
        public void Should_ReturnTrue_When_FileExists()
        {
            var testFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Utils", "testFile.txt");

            Assert.IsTrue(_objectUnderTest.FileExists(testFilePath));
        }

        [Test]
        public void Should_GetFileExtensions_When_FileHasExtension()
        {
            var testFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Utils", "testFile.txt");

            Assert.AreEqual(".txt", _objectUnderTest.GetExtension(testFilePath));
        }

        [Test]
        public void Should_ReturnTrue_When_CanWriteToFile()
        {
            var testFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Utils", "testFile.txt");

            Assert.IsTrue(_objectUnderTest.CanWrite(testFilePath));
        }
    }
}
