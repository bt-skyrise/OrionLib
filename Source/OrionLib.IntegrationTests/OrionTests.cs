using System;
using NUnit.Framework;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using FluentAssertions;
using OrionLib.QueryContext;

namespace OrionLib.IntegrationTests
{
    public class OrionTests
    {
        public IOrion Sut;

        [SetUp]
        public async Task SetUp()
        {
            var config = TestConfiguration.GetOrionConfiguration();
            Sut = new OrionFactory().Create(config);

            foreach (var orionEntity in await Sut.QueryEntities().GetAllAsync())
            {
                await Sut.RemoveEntityAsync(orionEntity.Type, orionEntity.Id);
            }
        }

        
        [Test]
        public async Task Can_Create_Entity()
        {
            //act
            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new []
            {
                new OrionAttribute("author", "string", "Lem"),
                new OrionAttribute("title", "string", "Solaris")
            }));

            //assert
            var foundEntity = await Sut.GetEntityAsync("book", "book_1");

            foundEntity.ShouldBeEquivalentTo(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("author", "string", "Lem"),
                new OrionAttribute("title", "string", "Solaris")
            }));
        }

        [Test]
        public async Task Can_Add_Attribute_To_Entity()
        {
            //arrange
            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("author", "string", "Lem"),
            }));

            //act
            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("title", "string", "Solaris")
            }));

            //assert
            var foundEntity = await Sut.GetEntityAsync("book", "book_1");
            
            foundEntity.ShouldBeEquivalentTo(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("author", "string", "Lem"),
                new OrionAttribute("title", "string", "Solaris")
            }));
        }

        [Test]
        public async Task Can_Remove_Attributes_From_Entity()
        {
            //arrange
            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("author", "string", "Lem"),
                new OrionAttribute("title", "string", "Solaris"),
                new OrionAttribute("year", "int", "1991")
            }));

            //act
            await Sut.RemoveAttributesAsync("book", "book_1", "title", "author");

            //assert
            var foundEntity = await Sut.GetEntityAsync("book", "book_1");

            foundEntity.ShouldBeEquivalentTo(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("year", "int", "1991")
            }));
        }

        [Test]
        public async Task Shold_Throw_On_Removing_Not_Existing_Attrubite()
        {
            //arrange
            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("author", "string", "Lem"),
            }));

            //act
            OrionException expectedException = null;

            try
            {
                await Sut.RemoveAttributesAsync("book", "book_1", "title");
            }
            catch (OrionException exception)
            {
                expectedException = exception;
            }

            //assert
            expectedException.Should().NotBe(null);
        }

        [Test]
        public async Task Can_Create_Entity_With_Attributes_Metadata()
        {
            //act
            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("author", "string", "Lem", new[]
                {
                    new OrionAttributeMetadata("nationality", "string", "Polish"),
                    new OrionAttributeMetadata("born_year", "year", "1921")
                }),
                new OrionAttribute("title", "string", "Solaris", new[]
                {
                    new OrionAttributeMetadata("pages", "int", "100")
                })
            }));

            //assert
            var foundEntity = await Sut.GetEntityAsync("book", "book_1");

            foundEntity.ShouldBeEquivalentTo(new OrionEntity("book", "book_1", new[]
            {
               new OrionAttribute("author", "string", "Lem", new[]
                {
                    new OrionAttributeMetadata("nationality", "string", "Polish"),
                    new OrionAttributeMetadata("born_year", "year", "1921")
                }),
                new OrionAttribute("title", "string", "Solaris", new[]
                {
                    new OrionAttributeMetadata("pages", "int", "100")
                })
            }));
        }

        [Test]
        public async Task Can_Add_Metadata_To_Entity_Attribute()
        {
            //arrange
            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("author", "string", "Lem", new[]
                {
                    new OrionAttributeMetadata("nationality", "string", "Polish"),
                }),
            }));

            //act
            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("author", "string", "Lem", new[]
                {
                    new OrionAttributeMetadata("born_year", "year", "1921")
                }),
            }));

            //assert
            var foundEntity = await Sut.GetEntityAsync("book", "book_1");

            foundEntity.ShouldBeEquivalentTo(new OrionEntity("book", "book_1", new[]
            {
               new OrionAttribute("author", "string", "Lem", new[]
                {
                    new OrionAttributeMetadata("nationality", "string", "Polish"),
                    new OrionAttributeMetadata("born_year", "year", "1921")
                })
            }));
        }

        [Test]
        public async Task Can_Remove_Entity()
        {
            //Arrange
            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("author", "string", "Lem"),
                new OrionAttribute("title", "string", "Solaris")
            }));

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_2", new[]
            {
                new OrionAttribute("author", "string", "Lem"),
                new OrionAttribute("title", "string", "Bajki robotów")
            }));

            //Act
            await Sut.RemoveEntityAsync("book", "book_1");

            //Assert
            var foundEntities = await Sut.QueryEntities("book").GetAllAsync();

            foundEntities.ShouldAllBeEquivalentTo(new[]
            {
                new OrionEntity("book", "book_2", new[]
                {
                    new OrionAttribute("author", "string", "Lem"),
                    new OrionAttribute("title", "string", "Bajki robotów")
                })
            });
        }

        [Test]
        public async Task Can_Update_Entity()
        {
            //Arrange
            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new[]
           {
                new OrionAttribute("author", "string", "Lem"),
                new OrionAttribute("title", "string", "Solaris")
            }));

            //Act
            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("author", "string", "Lem Stanisław"),
                new OrionAttribute("year", "integer", "1961")
            }));


            //Assert
            var foundEntity = await Sut.GetEntityAsync("book", "book_1");

            foundEntity.ShouldBeEquivalentTo(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("author", "string", "Lem Stanisław"),
                new OrionAttribute("title", "string", "Solaris"),
                new OrionAttribute("year", "integer", "1961")
            }));
        }

        [Test]
        public async Task Can_Try_To_Get_Not_Present_Entity()
        {
            var result = await Sut.TryGetEntityAsync("book", "book_1");

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Can_Set_Entity_Attribute()
        {
            // arrange

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new []
            {
                new OrionAttribute("title", "string", "Solaris")
            }));

            // act

            await Sut.UpdateAttributeAsync("book", "book_1", "title", "Fiasco");

            // assert

            var changedEntity = await Sut.GetEntityAsync("book", "book_1");

            Assert.That(changedEntity.GetAttributeValueByName("title"), Is.EqualTo("Fiasco"));
        }

        [TestCase(1001)]
        [TestCase(0)]
        [TestCase(-1)]
        public async Task Fails_When_Query_Limit_Is_Out_Of_Range(int limit)
        {
            ArgumentOutOfRangeException expectedException = null;

            try
            {
                await Sut.QueryEntities("bay").GetPageAsync(0, limit);
            }
            catch (ArgumentOutOfRangeException exception)
            {
                expectedException = exception;
            }

            expectedException.Should().NotBe(null);
        }

        [Test]
        public async Task Can_Query_Many_Entities()
        {
            // arrange

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("title", "string", "Solaris")
            }));

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_2", new[]
            {
                new OrionAttribute("title", "string", "Fiasco")
            }));

            // act

            var result = await Sut.QueryEntities("book").GetAllAsync();

            // assert
            result.ShouldAllBeEquivalentTo(new[]
            {
                new OrionEntity("book", "book_1", new[]
                {
                    new OrionAttribute("title", "string", "Solaris")
                }),
                new OrionEntity("book", "book_2", new[]
                {
                    new OrionAttribute("title", "string", "Fiasco")
                })
            });
        }

        [Test]
        public async Task Can_Query_Many_Entities_Of_Different_Types()
        {
            // arrange

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("title", "string", "Solaris")
            }));

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("bookshelf", "bookshelf_1", new[]
            {
                new OrionAttribute("capacity", "string", "10 books")
            }));

            // act

            var result = await Sut.QueryEntities().GetAllAsync();

            // assert

            result.ShouldAllBeEquivalentTo(new[]
            {
                new OrionEntity("book", "book_1", new[]
                {
                    new OrionAttribute("title", "string", "Solaris")
                }),
                new OrionEntity("bookshelf", "bookshelf_1", new[]
                {
                    new OrionAttribute("capacity", "string", "10 books")
                })
            });
        }

        [Test]
        public async Task Can_Query_All_Entities()
        {
            // arrange

            var entityIds = Enumerable
                .Range(0, 2000)
                .Select(id => id.ToString())
                .ToArray();

            foreach (var entityId in entityIds)
            {
                await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", entityId, new[]
                {
                    new OrionAttribute("title", "string", "some book")
                }));
            }

            // act

            var results = await Sut.QueryEntities("book")
                .GetAllAsync();

            // assert

            var allIds = results.Select(entity => entity.Id).ToArray();

            Assert.That(allIds, Is.EquivalentTo(entityIds));
        }

        [Test]
        public async Task Can_Count_Many_Entities()
        {
            // arrange

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("title", "string", "Solaris")
            }));

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_2", new[]
            {
                new OrionAttribute("title", "string", "Fiasco")
            }));

            // act

            var result = await Sut.QueryEntities("book").CountAsync();

            // assert

            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public async Task Can_Count_Many_Entities_Of_Different_Types()
        {
            // arrange

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("title", "string", "Solaris")
            }));

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("bookshelf", "bookshelf_1", new[]
            {
                new OrionAttribute("capacity", "string", "10 books")
            }));

            // act

            var result = await Sut.QueryEntities().CountAsync();

            // assert

            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public async Task Can_Query_Empty_Context()
        {
            // act

            var result = await Sut.QueryEntities("book").GetAllAsync();

            // assert

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task Can_Query_Many_Entities_With_Equal_Filter()
        {
            // arrange

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("title", "string", "Solaris")
            }));

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_2", new[]
            {
                new OrionAttribute("title", "string", "Fiasco")
            }));

            // act

            var result = await Sut.QueryEntities("book")
                .WithAttributeEqualTo("title","Solaris")
                .GetAllAsync();

            // assert
            result.ShouldAllBeEquivalentTo(new[]
            {
                new OrionEntity("book", "book_1", new[]
                {
                    new OrionAttribute("title", "string", "Solaris")
                })
            });
        }

        [Test]
        public async Task Can_Query_Many_Entities_Of_Different_Types_With_Equal_Filter()
        {
            // arrange
            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("owner", "person", "Peter Pan"),
            }));

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("bookshelf", "bookshelf_1", new[]
            {
                new OrionAttribute("owner", "person", "Peter Pan"),
            }));

            // act

            var result = await Sut.QueryEntities()
                .WithAttributeEqualTo("owner", "Peter Pan")
                .GetAllAsync();

            // assert
            result.ShouldAllBeEquivalentTo(new[]
            {
                new OrionEntity("book", "book_1", new[]
                {
                    new OrionAttribute("owner", "person", "Peter Pan"),
                }),
                new OrionEntity("bookshelf", "bookshelf_1", new[]
                {
                    new OrionAttribute("owner", "person", "Peter Pan"),
                })
            });
        }

        [Test]
        public async Task Can_Count_Many_Entities_With_Equal_Filter()
        {
            // arrange

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("title", "string", "Solaris")
            }));

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_2", new[]
            {
                new OrionAttribute("title", "string", "Fiasco")
            }));

            // act

            var result = await Sut.QueryEntities("book")
                .WithAttributeEqualTo("title", "Solaris")
                .CountAsync();

            // assert

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task Can_Count_Many_Entities_Of_Different_Types_With_Equal_Filter()
        {
            // arrange

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("title", "string", "Solaris")
            }));

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("bookshelf", "bookshelf_1", new[]
            {
                new OrionAttribute("capacity", "string", "10 books")
            }));

            // act

            var result = await Sut.QueryEntities()
                .WithAttributeEqualTo("title", "Solaris")
                .CountAsync();

            // assert

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task Can_Query_Many_Entities_With_Not_Equal_Filter()
        {
            // arrange

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("title", "string", "Solaris")
            }));

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_2", new[]
            {
                new OrionAttribute("title", "string", "Dune")
            }));

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_3", new[]
            {
                new OrionAttribute("title", "string", "Fiasco")
            }));

            // act

            var result = await Sut.QueryEntities("book")
                .WithAttributeNotEqualTo("title", "Fiasco")
                .GetAllAsync();

            // assert
            result.ShouldAllBeEquivalentTo(new[]
            {
                new OrionEntity("book", "book_1", new[]
                {
                    new OrionAttribute("title", "string", "Solaris")
                }),
                new OrionEntity("book", "book_2", new[]
                {
                    new OrionAttribute("title", "string", "Dune")
                })
            });
        }

        [Test]
        public async Task Can_Query_Many_Entities_With_Filter_And_Pagination()
        {
            // arrange

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("title", "string", "Solaris")
            }));

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_2", new[]
            {
                new OrionAttribute("title", "string", "Dune")
            }));

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_3", new[]
            {
                new OrionAttribute("title", "string", "Fiasco")
            }));

            // act

            var result = await Sut.QueryEntities("book")
                .WithAttributeNotEqualTo("title", "Fiasco")
                .GetPageAsync(1,1);

            // assert
            result.Entities.ShouldAllBeEquivalentTo(new[]
            {
                new OrionEntity("book", "book_1", new[]
                {
                    new OrionAttribute("title", "string", "Solaris")
                })
            });

            Assert.That(result.QueryLimit, Is.EqualTo(1));
            Assert.That(result.QueryOffset, Is.EqualTo(1));
        }

        [Test]
        public async Task Can_Query_Many_Entities_With_Many_Filters()
        {
            // arrange

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("title", "string", "Solaris"),
                new OrionAttribute("author", "string", "Lem")
            }));

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_2", new[]
            {
                new OrionAttribute("title", "string", "Dune"),
                new OrionAttribute("author", "string", "Herbert")
            }));

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_3", new[]
            {
                new OrionAttribute("title", "string", "Dune Messiah"),
                new OrionAttribute("author", "string", "Herbert")
            }));
            // act

            var result = await Sut.QueryEntities("book")
                .WithAttributeEqualTo("author", "Herbert")
                .WithAttributeNotEqualTo("title", "Dune")
                .GetAllAsync();

            // assert
            result.ShouldAllBeEquivalentTo(new[]
            {
                new OrionEntity("book", "book_3", new[]
                {
                    new OrionAttribute("author", "string", "Herbert"),
                    new OrionAttribute("title", "string", "Dune Messiah")
                })
            });
        }

        [Test]
        public async Task Can_Count_Many_Entities_With_Many_Filters()
        {
            // arrange

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("title", "string", "Solaris"),
                new OrionAttribute("author", "string", "Lem")
            }));

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_2", new[]
            {
                new OrionAttribute("title", "string", "Dune"),
                new OrionAttribute("author", "string", "Herbert")
            }));

            await Sut.CreateOrUpdateEntityAsync(new OrionEntity("book", "book_3", new[]
            {
                new OrionAttribute("title", "string", "Dune Messiah"),
                new OrionAttribute("author", "string", "Herbert")
            }));
            // act

            var result = await Sut.QueryEntities("book")
                .WithAttributeEqualTo("author", "Herbert")
                .WithAttributeNotEqualTo("title", "Dune")
                .CountAsync();

            // assert

            Assert.That(result, Is.EqualTo(1));
        }
    }
}