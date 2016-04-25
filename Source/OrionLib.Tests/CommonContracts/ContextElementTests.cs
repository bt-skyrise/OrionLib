using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using OrionLib.CommonContracts;

namespace OrionLib.Tests.CommonContracts
{
    public class ContextElementTests
    {
        [Test]
        public void Can_Convert_Context_Element_To_Entity()
        {
            // arrange

            var contextElement = new ContextElementDto
            {
                Id = "book_1",
                Type = "book",
                Attributes = new List<GetAttributeDto>
                {
                    new GetAttributeDto
                    {
                        Name = "author",
                        Type = "string",
                        Value = "Lem"
                    },
                    new GetAttributeDto
                    {
                        Name = "title",
                        Type = "string",
                        Value = "Solaris"
                    },
                }
            };

            // act

            var result = contextElement.ToEntity();

            // assert

            var expectedEntity = new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("author", "string", "Lem"),
                new OrionAttribute("title", "string", "Solaris")
            });

            result.ShouldBeEquivalentTo(expectedEntity);
        }

    }
}