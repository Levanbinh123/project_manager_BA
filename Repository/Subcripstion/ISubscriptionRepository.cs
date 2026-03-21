public interface ISubscriptionRepository
{
    Task<Subscription?> FindByUserIdAsync(long userId);
    Task DeleteByUserIdAsync(long userId);
}