using NUnit.Framework;

namespace OrionLib.Tests
{
    public class EntityAttributeTests
    {
        [Test]
        public void Can_Create_Entity_Attribute()
        {
            var entityAttribute = new OrionAttribute("author", "string", "Lem");

            Assert.That(entityAttribute.Name, Is.EqualTo("author"));
            Assert.That(entityAttribute.Type, Is.EqualTo("string"));
            Assert.That(entityAttribute.Value, Is.EqualTo("Lem"));
        }
    }
}