using FluentAssertions;
using NUnit.Framework;
using OrionLib.CommonContracts;

namespace OrionLib.Tests.CommonContracts
{
    public class OrionResponseTests
    {
        [Test]
        public void Response_Is_Valid_When_Response_Is_Valid()
        {
            var sut = new OrionResponseDto
            {
                StatusCode = new OrionStatusDto
                {
                    Code = "200"
                }
            };

            Assert.That(sut.TryGetError(), Is.Null);
        }

        [Test]
        public void Response_Is_Valid_When_Status_Code_Is_Not_Present()
        {
            var sut = new OrionResponseDto();

            Assert.That(sut.TryGetError(), Is.Null);
        }

        [Test]
        public void Can_Get_Orion_Error()
        {
            var sut = new OrionResponseDto
            {
                OrionError = new OrionStatusDto
                {
                    Code = "404"
                }
            };

            sut.TryGetError().ShouldBeEquivalentTo(new OrionStatusDto
            {
                Code = "404"
            });
        }

        [Test]
        public void Can_Get_Error_Code()
        {
            var sut = new OrionResponseDto
            {
                ErrorCode = new OrionStatusDto
                {
                    Code = "404"
                }
            };

            sut.TryGetError().ShouldBeEquivalentTo(new OrionStatusDto
            {
                Code = "404"
            });
        }

        [Test]
        public void Can_Get_Invalid_Status_Code()
        {
            var sut = new OrionResponseDto
            {
                StatusCode = new OrionStatusDto
                {
                    Code = "404"
                }
            };

            sut.TryGetError().ShouldBeEquivalentTo(new OrionStatusDto
            {
                Code = "404"
            });
        }
    }
}