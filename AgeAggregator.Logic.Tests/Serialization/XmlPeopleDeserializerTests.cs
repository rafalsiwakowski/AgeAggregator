using AgeAggregator.Logic.Serialization;
using System.Linq;
using AgeAggregator.Logic.Models;
using AgeAggregator.Logic.Validation;
using NUnit.Framework;

namespace AgeAggregator.Logic.Tests.Serialization
{
    class XmlPeopleDeserializerTests : PeopleDeserializationTestsBase<XmlPeopleDeserializer>
    {
        protected override XmlPeopleDeserializer CreateTestObject(IValidator<Person> personValidator)
        {
            return new XmlPeopleDeserializer(personValidator);
        }

        protected override void TestIgnoringInvalidEntries()
        {
            var xml = @"<?xml version='1.0' encoding='UTF-8'?>
<dataset>
<record><first_name></first_name><last_name>Borsnall</last_name><country>Portugal</country><age>55</age></record>
<record><first_name>Teador</first_name><last_name>Holcroft</last_name><country>Ukraine</country><age>invalid</age></record>
<record><first_name>Tiffy</first_name><last_name>Reggio</last_name><country>China</country><age>65</age></record></dataset>";

            var people = ObjectUnderTest.DeserializeArray(xml);

            Assert.AreEqual(1, people.Count());
            Assert.AreEqual("Tiffy", people.First().FirstName);
            Assert.AreEqual("Reggio", people.First().LastName);
            Assert.AreEqual("China", people.First().Country);
            Assert.AreEqual(65, people.First().Age);
        }
    }
}
