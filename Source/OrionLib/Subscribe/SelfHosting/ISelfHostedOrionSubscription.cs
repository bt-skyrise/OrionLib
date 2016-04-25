using System;

namespace OrionLib.Subscribe.SelfHosting
{
    public interface ISelfHostedOrionSubscription : IDisposable
    {
        void Start();
    }
}