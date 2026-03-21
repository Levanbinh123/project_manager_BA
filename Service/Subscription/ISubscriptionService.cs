public interface ISubscriptionService
{
    Task<Subscription> CreateSubscription(User user);
    Task<Subscription> GetUserSubscription(int userId);
    Task<Subscription> UpgradeSubscription(int userId, PlanType planType);
    bool IsValid(Subscription subscription);
}