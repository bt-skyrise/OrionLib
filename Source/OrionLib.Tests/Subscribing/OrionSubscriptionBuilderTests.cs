using System;
using System.Security.Policy;
using Moq;
using NUnit.Framework;
using OrionLib.Subscribe;
using OrionLib.Subscribe.Creation;

namespace OrionLib.Tests.Subscribing
{
    public class OrionSubscriptionBuilderTests
    {
        private OrionSubscriptionBuilder<IOrionSubscription> BuildSubscription(Url notificationAddress, string entityType, string[] attributes)
        {
            var subscriptionFactoryMock = new Mock<ISubscriptionFactory<IOrionSubscription>>();

            return new OrionSubscriptionBuilder<IOrionSubscription>(subscriptionFactoryMock.Object, notificationAddress, entityType, attributes);
        }

        [Test]
        public void Cannot_Subscribe_To_Zero_Attributes()
        {
            var sut = BuildSubscription(new Url("http://test.com/"), "type", new string[0]);

            Assert.That(() => sut.Create(), Throws.ArgumentException);
        }

        [Test]
        public void Cannot_Create_Subscription_Time_Configuration_With_Duration_Shorter_Than_Renewing_Timeout()
        {
            var sut = BuildSubscription(new Url("http://test.com/"), "type", new[] { "attribute" })
                .WithSubscriptionTiming(TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(60));

            Assert.That(() => sut.Create(), Throws.ArgumentException);
        }
    }
}