using NUnit.Framework;
using OrionLib.CommonContracts;

namespace OrionLib.Tests.CommonContracts
{
    public class OrionStatusTests
    {
        [Test]
        public void Can_Serialize_Orion_Status()
        {
            var sut = new OrionStatusDto
            {
                Code = "404",
                ReasonPhrase = "No context element found",
                Details = "Entity id: /book_1/"
            };

            Assert.That(sut.Serialize(), Is.EqualTo("OrionStatus(code: '404', reasonPhrase: 'No context element found', details: 'Entity id: /book_1/')"));
        }
    }
}