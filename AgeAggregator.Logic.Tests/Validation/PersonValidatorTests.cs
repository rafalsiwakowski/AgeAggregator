using AgeAggregator.Logic.Models;
using AgeAggregator.Logic.Validation;
using NUnit.Framework;

namespace AgeAggregator.Logic.Tests.Validation
{
    [TestFixture]
    public class PersonValidatorTests
    {
        PersonValidator _objectUnderTest;

        [SetUp]
        public void SetUp()
        {
            _objectUnderTest = new PersonValidator();
        }

        [Test]
        public void Should_ReturnTrue_When_ValidPersonProvided()
        {
            var person = new Person
            {
                FirstName = "ValidName",
                LastName = "ValidLastName",
                Country = "ValidCountry",
                Age = 25
            };

            Assert.IsTrue(_objectUnderTest.IsValid(person));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Should_ReturnFalse_When_PersonHasInvalidFirstName(string invalidFirstName)
        {
            var person = new Person
            {
                FirstName = invalidFirstName,
                LastName = "ValidLastName",
                Country = "ValidCountry",
                Age = 25
            };

            Assert.IsFalse(_objectUnderTest.IsValid(person));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Should_ReturnFalse_When_PersonHasInvalidLastName(string invalidLastName)
        {
            var person = new Person
            {
                FirstName = "ValidName",
                LastName = invalidLastName,
                Country = "ValidCountry",
                Age = 25
            };

            Assert.IsFalse(_objectUnderTest.IsValid(person));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Should_ReturnFalse_When_PersonHasInvalidCountry(string invalidCountry)
        {
            var person = new Person
            {
                FirstName = "ValidName",
                LastName = "ValidLastName",
                Country = invalidCountry,
                Age = 25
            };

            Assert.IsFalse(_objectUnderTest.IsValid(person));
        }

        [Test]
        public void Should_ReturnFalse_When_PersonHasNegativeAge()
        {
            var person = new Person
            {
                FirstName = "ValidName",
                LastName = "ValidLastName",
                Country = "ValidCountry",
                Age = -1
            };

            Assert.IsFalse(_objectUnderTest.IsValid(person));
        }
    }
}
