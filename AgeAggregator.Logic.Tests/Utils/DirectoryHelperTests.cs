using AgeAggregator.Logic.Utils;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Threading;

namespace AgeAggregator.Logic.Tests.Utils
{
    [TestFixture]
    public class DirectoryHelperTests
    {
        DirectoryHelper _objectUnderTest;

        [SetUp]
        public void SetUp()
        {
            _objectUnderTest = new DirectoryHelper();
        }

        [Test]
        public void Should_ReturnFalse_When_NotExistingPathProvided()
        {
            Assert.IsFalse(_objectUnderTest.ValidateDirectoryPath("pathThatDoesNotExist"));
        }

        [Test]
        public void Should_ReturnTrue_When_ExistingPathProvided()
        {
            var relativeSolutionPath = @"..\..\..";
            Assert.IsTrue(_objectUnderTest.ValidateDirectoryPath(relativeSolutionPath));
        }

        [Test]
        public void Should_ReturnDirectoryFiles_When_DirectoryContainsFiles()
        {
            string tempFileName = "tempFile1.txt";
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            var tempFilePath = Path.Combine(tempDirectory, tempFileName);
            using (File.Create(tempFilePath)) ;

            var results = _objectUnderTest.GetDirectoryFiles(tempDirectory);

            Assert.AreEqual(1, results.Length);
            Assert.AreEqual(tempFilePath, results[0]);

            Directory.Delete(tempDirectory, true);
        }
    }
}
