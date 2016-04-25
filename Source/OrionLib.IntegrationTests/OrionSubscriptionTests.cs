using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OrionLib.Subscribe.SelfHosting;

namespace OrionLib.IntegrationTests
{
    // Warning: these tests are not 100% deterministic because of Orion's chaotic nature. They also can take very long.

    [Timeout(10000)]
    public class OrionSubscriptionTests
    {
        class Fixture
        {
            private readonly IOrion _sut;
            private readonly BlockingCollection<IEnumerable<OrionEntity>> _orionEvents;
            
            public Fixture()
            {
                var config = TestConfiguration.GetOrionConfiguration();

                _sut = new OrionFactory().Create(config);
                _orionEvents = new BlockingCollection<IEnumerable<OrionEntity>>();
            }

            public async Task CreateEntityAsync(OrionEntity orionEntity)
            {
                await _sut.CreateOrUpdateEntityAsync(orionEntity);
            }

            public ISelfHostedOrionSubscription StartSubscription(string type, string[] attributes)
            {
                var subscription = _sut.CreateSelfHostedSubscription(TestConfiguration.SubscriptionAddress, TestConfiguration.SubscriptionAddress, type, attributes)
                    .WithSubscriptionTiming(duration: TimeSpan.FromSeconds(3), renewingTimeout: TimeSpan.FromSeconds(2))
                    .OnEntityChanged(_orionEvents.Add)
                    .Create();

                subscription.Start();

                return subscription;
            }

            public void WaitForSubscriptionToExpire()
            {
                Thread.Sleep(TimeSpan.FromSeconds(4));
            }

            public async Task UpdateAttributeAsync(string type, string id, string attribute, string newValue)
            {
                await _sut.UpdateAttributeAsync(type, id, attribute, newValue);
            }

            public void WaitUntilAttributeChangesTo(string expectedType, string expectedId, string expectedAttribute, string expectedValue)
            {
                while (true)
                {
                    var entities = _orionEvents.Take();

                    if (ExpectedChange(entities, expectedType, expectedId, expectedAttribute, expectedValue))
                    {
                        return;
                    }
                }
            }

            private static bool ExpectedChange(IEnumerable<OrionEntity> entities, string type, string id, string attribute, string newValue)
            {
                var entity = entities.FirstOrDefault();

                return entity != null
                    && entity.Type == type
                    && entity.Id == id
                    && entity.GetAttributeValueByName(attribute) == newValue;
            }
        }

        [Test]
        public async Task Can_Subscribe_To_Entity_Attribute_Changes()
        {
            var fixture = new Fixture();

            await fixture.CreateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("title", "string", "Solaris")
            }));

            using (fixture.StartSubscription("book", new[] { "title" }))
            {
                await fixture.UpdateAttributeAsync("book", "book_1", "title", "Fiasco");

                fixture.WaitUntilAttributeChangesTo(
                    expectedType: "book",
                    expectedId: "book_1",
                    expectedAttribute: "title",
                    expectedValue: "Fiasco");
            }

            Assert.Pass("Update notification was successfully received.");
        }

        [Test]
        public async Task Can_Update_Subscription_When_It_Expires()
        {
            var fixture = new Fixture();

            await fixture.CreateEntityAsync(new OrionEntity("book", "book_1", new[]
            {
                new OrionAttribute("title", "string", "Solaris")
            }));

            using (fixture.StartSubscription("book", new[] { "title" }))
            {
                fixture.WaitForSubscriptionToExpire();

                await fixture.UpdateAttributeAsync("book", "book_1", "title", "Fiasco");

                fixture.WaitUntilAttributeChangesTo(
                    expectedType: "book",
                    expectedId: "book_1",
                    expectedAttribute: "title",
                    expectedValue: "Fiasco");
            }

            Assert.Pass("Subscription was successfully renewed and notification was received.");
        }
    }
}