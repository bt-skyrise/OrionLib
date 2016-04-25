using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OrionLib.CommonContracts;

namespace OrionLib.Tests.CommonContracts
{
    public class ContextResponseTests
    {
        [Test]
        public void Can_Check_If_Context_Response_Is_Valid()
        {
            var contextResponses = new List<ContextResponseDto>
            {
                new ContextResponseDto
                {
                    ContextElement = CreateSomeContextElement("book_1"),
                    StatusCode = new OrionStatusDto
                    {
                        Code = "200"
                    }
                },
                new ContextResponseDto
                {
                    ContextElement = CreateSomeContextElement("book_2"),
                    StatusCode = new OrionStatusDto
                    {
                        Code = "500"
                    }
                }
            };

            var result = contextResponses.GetValidEntities();

            Assert.That(result.Single().Id, Is.EqualTo("book_1"));
        }

        private ContextElementDto CreateSomeContextElement(string id)
        {
            return new ContextElementDto
            {
                Id = id,
                Type = "book",
                Attributes = new List<GetAttributeDto>
                {
                    new GetAttributeDto
                    {
                        Name = "title",
                        Type = "string",
                        Value = "Solaris"
                    }
                }
            };
        }

    }
}