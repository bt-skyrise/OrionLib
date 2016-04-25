using System;
using FluentAssertions;
using NUnit.Framework;

namespace OrionLib.Tests
{
    public class EntityTests
    {
        [Test]
        public void Can_Get_Attribute_By_Name()
        {
            // arrange

            var entity = new OrionEntity("person", "person_1", new[]
            {
                new OrionAttribute("name", "string", "Jan"),
                new OrionAttribute("surname", "string", "Kowalski")
            });

            // act

            var result = entity.GetAttributeByName("name");

            // assert
            result.ShouldBeEquivalentTo(new OrionAttribute("name", "string", "Jan"));
        }

        [Test]
        public void Can_Get_Attribute_Value_By_Name()
        {
            // arrange

            var entity = new OrionEntity("person", "person_1", new[]
            {
                new OrionAttribute("name", "string", "Jan"),
                new OrionAttribute("surname", "string", "Kowalski")
            });

            // act

            var result = entity.GetAttributeValueByName("name");

            // assert

            Assert.That(result, Is.EqualTo("Jan"));
        }

        [Test]
        public void Entity_Cannot_Have_Duplicated_Attributes()
        {
            // arrange

            var attributes = new[]
            {
                new OrionAttribute("name", "string", "Jan"),
                new OrionAttribute("name", "string", "Janusz")
            };

            // act, assert

            Assert.That(() => new OrionEntity("person", "person_1", attributes),
                Throws.TypeOf<ArgumentException>());
        }
    }
}