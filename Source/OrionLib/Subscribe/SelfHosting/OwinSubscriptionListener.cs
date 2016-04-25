using System;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using OrionLib.Utils;
using Owin;

namespace OrionLib.Subscribe.SelfHosting
{
    public class OwinSubscriptionListener : IDisposable
    {
        private readonly Url _url;
        private readonly IOrionSubscription _subscription;

        private IDisposable _webServer;

        public static OwinSubscriptionListener StartListening(Url url, IOrionSubscription subscription)
        {
            var listener = new OwinSubscriptionListener(url, subscription);

            listener.Start();

            return listener;
        }

        public OwinSubscriptionListener(Url url, IOrionSubscription subscription)
        {
            _url = url;
            _subscription = subscription;
        }

        public void Start()
        {
            _webServer = WebApp.Start(_url.Value, appBuilder =>
            {
                appBuilder.Run(context =>
                {
                    return Task.Run(() =>
                    {
                        var content = context.Request.Body.ReadStringToEnd();

                        _subscription.NotifyAboutOrionEvent(content);
                    });
                });
            });
        }

        public void Dispose()
        {
            _webServer?.Dispose();
        }
    }
}