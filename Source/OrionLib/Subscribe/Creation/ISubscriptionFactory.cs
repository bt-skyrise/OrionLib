namespace OrionLib.Subscribe.Creation
{
    public interface ISubscriptionFactory<TSubscription>
    {
        TSubscription Create(OrionSubscriptionConfiguration configuration);
    }
}