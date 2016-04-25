using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using OrionLib.Communication;
using OrionLib.Subscribe;
using OrionLib.Subscribe.SelfHosting;

namespace OrionLib.IntegrationTests
{
    public class OwinNotificationListenerTests
    {
        [Test]
        public async Task Can_Get_Notification_About_Event()
        {
            // arrange

            var orionSubscriptionMock = new Mock<IOrionSubscription>();

            var sut = new OwinSubscriptionListener(TestConfiguration.SubscriptionAddress, orionSubscriptionMock.Object);

            // act

            sut.Start();

            await SendNotification("hello listener");

            // assert
            
            orionSubscriptionMock.Verify(subscription => subscription.NotifyAboutOrionEvent("hello listener"));
        }

        public async Task SendNotification(string content)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(TestConfiguration.SubscriptionAddress.Value)
            };

            var response = await client.PostAsync("/", new StringContent(content, Encoding.UTF8, "text/plain"));
            response.EnsureSuccessStatusCode();
        }
    }
}