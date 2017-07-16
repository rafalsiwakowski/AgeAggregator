using AgeAggregator.Logic.Models;
using AgeAggregator.Logic.Serialization;
using NUnit.Framework;
using System.Collections.Generic;

namespace AgeAggregator.Logic.Tests.Serialization
{
    [TestFixture]
    public class AveragePeopleAgePerCountrySerializerTests
    {
        AveragePeopleAgePerCountrySerializer _objectUnderTest;

        [SetUp]
        public void SetUp()
        {
            _objectUnderTest = new AveragePeopleAgePerCountrySerializer();
        }

        [Test]
        public void Should_SerializeToStringWithTwoJsonObjects_When_ProvidedWithTwoElementsInput()
        {
            var expectedResult = @"[
  {
    ""country"": ""Country1"",
    ""average_age"": 500.5
  },
  {
    ""country"": ""Country2"",
    ""average_age"": 123.0
  }
]";
            var averages = new List<AveragePeopleAgePerCountry>
            {
                new AveragePeopleAgePerCountry
                {
                    Country = "Country1",
                    AverageAge = 500.5f
                },
                new AveragePeopleAgePerCountry
                {
                    Country = "Country2",
                    AverageAge = 123f
                },
            };

            var result = _objectUnderTest.SerializeArray(averages);

            Assert.AreEqual(expectedResult, result);
        }
    }
}
