public interface ISubscriptionService
{
    Task<Subscription> CreateSubscription(User user);
    Task<Subscription> GetUserSubscription(long userId);
    Task<Subscription> UpgradeSubscription(long userId, PlanType planType);
    bool IsValid(Subscription subscription);
}