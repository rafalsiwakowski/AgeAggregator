using AgeAggregator.Logic.Cache;
using AgeAggregator.Logic.Models;
using AgeAggregator.Logic.Serialization;
using AgeAggregator.Logic.Utils;
using FakeItEasy;
using NUnit.Framework;
using System.Collections.Generic;

namespace AgeAggregator.Logic.Tests.Cache
{
    [TestFixture]
    public class PeopleCacheTests
    {
        PeopleCache _objectUnderTest;

        IPersonDeserializerFactory _personDeserializerFactory;
        IDirectoryHelper _directoryHelper;
        IFileHelper _fileHelper;
        IPathHelper _pathHelper;

        [SetUp]
        public void SetUp()
        {
            _personDeserializerFactory = A.Fake<IPersonDeserializerFactory>();
            _directoryHelper = A.Fake<IDirectoryHelper>();
            _fileHelper = A.Fake<IFileHelper>();
            _pathHelper = A.Fake<IPathHelper>();
            _objectUnderTest = new PeopleCache(_personDeserializerFactory, _directoryHelper, _fileHelper, _pathHelper);
        }

        [Test]
        public void Should_AddItemsToCache_When_InitializedWithDirectoryPathWhereSomeItemsExist()
        {
            var deserializedPeople = new List<Person>()
            {
                new Person
                {
                    FirstName = "PersonName1",
                    LastName = "PersonLastName1",
                    Country = "Country1",
                    Age = 30
                },
                new Person
                {
                    FirstName = "PersonName2",
                    LastName = "PersonLastName2",
                    Country = "Country2",
                    Age = 60
                }
            };
            var peopleDeserializer = A.Fake<IDeserializer<Person>>();
            string directoryPath = "directoryPath";
            string[] allowedExtensions = new[] { ".xml", "json", ".csv" };
            string[] files = new[] { "file1" };
            A.CallTo(() => _directoryHelper.GetDirectoryFiles(directoryPath, A<string[]>.That.IsSameSequenceAs(allowedExtensions))).Returns(files);
            A.CallTo(() => _pathHelper.GetExtension("file1")).Returns(".extension");
            A.CallTo(() => _personDeserializerFactory.CreateFor(".extension")).Returns(peopleDeserializer);
            A.CallTo(() => _fileHelper.ReadFile("file1")).Returns("file1Content");
            A.CallTo(() => peopleDeserializer.DeserializeArray("file1Content")).Returns(deserializedPeople);

            _objectUnderTest.Initialize(directoryPath);
            var takenFromCache = _objectUnderTest.GetPeople();

            CollectionAssert.AreEquivalent(deserializedPeople, takenFromCache);
        }

        [Test]
        public void Should_RemoveItemsFromCache_When_ItemsFromSpecifiedFileExistInCache()
        {
            var deserializedPeople = new List<Person>()
            {
                new Person
                {
                    FirstName = "PersonName1",
                    LastName = "PersonLastName1",
                    Country = "Country1",
                    Age = 30
                },
                new Person
                {
                    FirstName = "PersonName2",
                    LastName = "PersonLastName2",
                    Country = "Country2",
                    Age = 60
                }
            };
            var peopleDeserializer = A.Fake<IDeserializer<Person>>();
            string directoryPath = "directoryPath";
            string[] allowedExtensions = new[] { ".xml", "json", ".csv" };
            string[] files = new[] { "file1" };
            A.CallTo(() => _directoryHelper.GetDirectoryFiles(directoryPath, A<string[]>.That.IsSameSequenceAs(allowedExtensions))).Returns(files);
            A.CallTo(() => _pathHelper.GetExtension("file1")).Returns(".extension");
            A.CallTo(() => _personDeserializerFactory.CreateFor(".extension")).Returns(peopleDeserializer);
            A.CallTo(() => _fileHelper.ReadFile("file1")).Returns("file1Content");
            A.CallTo(() => peopleDeserializer.DeserializeArray("file1Content")).Returns(deserializedPeople);

            _objectUnderTest.Initialize(directoryPath);
            _objectUnderTest.ExcludeFiles("file1");

            Assert.IsEmpty(_objectUnderTest.GetPeople());
        }
    }
}
