using System.Collections.Generic;
using AgeAggregator.Logic.Models;
using AgeAggregator.Logic.Validation;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

namespace AgeAggregator.Logic.Serialization
{
    class XmlPeopleDeserializer : PeopleDeserializerBase
    {
        public XmlPeopleDeserializer(IValidator<Person> personValidator) : base(personValidator)
        {
        }

        protected override IEnumerable<Person> DeserializeInternal(string serializedArray)
        {
            using (var reader = new StringReader(serializedArray))
            {
                var xmlSerializer = new XmlSerializer(typeof(List<PersonXmlWrapper>), new XmlRootAttribute("dataset"));
                var people = (List<PersonXmlWrapper>)xmlSerializer.Deserialize(reader);
                return people.Select(x => x.ToPerson());
            }
        }
    }

    [XmlType("record")]
    public class PersonXmlWrapper
    {
        [XmlElement("first_name")]
        public string FirstName { get; set; }
        [XmlElement("last_name")]
        public string LastName { get; set; }
        [XmlElement("country")]
        public string Country { get; set; }
        [XmlElement("age")]        
        public string  Age { get; set; }
        public Person ToPerson()
        {
            int ageAsNumber;
            return new Person
            {
                FirstName = FirstName,
                LastName = LastName,
                Country = Country,
                Age = int.TryParse(Age, out ageAsNumber) ? ageAsNumber : -1
            };
        }
    }
}
