using System;
using System.Threading.Tasks;

namespace OrionLib.Subscribe
{
    public interface IOrionSubscription : IDisposable
    {
        void NotifyAboutOrionEvent(string requestContent);
        Task Start();
    }
}