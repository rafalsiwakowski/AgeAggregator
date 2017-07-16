using AgeAggregator.Logic.Models;
using AgeAggregator.Logic.Serialization;
using AgeAggregator.Logic.Validation;
using NUnit.Framework;

namespace AgeAggregator.Logic.Tests.Serialization
{
    [TestFixture]
    abstract class PeopleDeserializationTestsBase<T> where T : IDeserializer<Person>
    {
        protected T ObjectUnderTest;

        protected IValidator<Person> PersonValidator;

        [SetUp]
        public void SetUp()
        {
            PersonValidator = new PersonValidator();
            ObjectUnderTest = CreateTestObject(PersonValidator);
        }

        protected abstract T CreateTestObject(IValidator<Person> personValidator);

        [Test]
        public void Should_IgnoreInvalidEntries_When_InvalidEntriesExistInSerializedContent()
        {
            TestIgnoringInvalidEntries();
        }

        protected abstract void TestIgnoringInvalidEntries();

        [Test]
        public void Should_ReturnNull_When_DeserializationThrows()
        {
            Assert.IsNull(ObjectUnderTest.DeserializeArray(null));
        }
    }
}
