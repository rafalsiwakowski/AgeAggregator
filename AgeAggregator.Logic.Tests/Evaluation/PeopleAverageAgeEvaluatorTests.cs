using AgeAggregator.Logic.Evaluation;
using AgeAggregator.Logic.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgeAggregator.Logic.Tests.Evaluation
{
    [TestFixture]
    public class PeopleAverageAgeEvaluatorTests
    {
        PeopleAverageAgeEvaluator _objectUnderTest;        

        [SetUp]
        public void SetUp()
        {
            _objectUnderTest = new PeopleAverageAgeEvaluator();
        }

        [Test]
        public void Should_ThrowArgumentNullException_When_ProvidedWithNullInput()
        {
            Assert.Throws<ArgumentNullException>(() => _objectUnderTest.ComputePeopleAverageAge(null));
        }

        [Test]
        public void Should_ComputeValidResult_When_ProvidedWithInputData()
        {
            var people = new List<Person>
            {
                new Person
                {
                    FirstName = "PersonName1",
                    LastName = "PersonLastName1",
                    Country = "Country1",
                    Age = 20
                },
                new Person
                {
                    FirstName = "PersonName2",
                    LastName = "PersonLastName2",
                    Country = "Country1",
                    Age = 60
                },
                new Person
                {
                    FirstName = "PersonName3",
                    LastName = "PersonLastName3",
                    Country = "Country1",
                    Age = 40
                },
                new Person
                {
                    FirstName = "PersonName4",
                    LastName = "PersonLastName4",
                    Country = "Country2",
                    Age = 3
                },
                new Person
                {
                    FirstName = "PersonName5",
                    LastName = "PersonLastName5",
                    Country = "Country2",
                    Age = 11
                },
                new Person
                {
                    FirstName = "PersonName6",
                    LastName = "PersonLastName6",
                    Country = "Country2",
                    Age = 16
                }
            };

            var result = _objectUnderTest.ComputePeopleAverageAge(people);

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Country1", result.First().Country);
            Assert.AreEqual(40, result.First().AverageAge);
            Assert.AreEqual("Country2", result.Last().Country);
            Assert.AreEqual(10, result.Last().AverageAge);
        }
    }
}
