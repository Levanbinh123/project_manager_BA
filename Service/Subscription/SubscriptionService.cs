using Microsoft.EntityFrameworkCore;

public class SubscriptionService : ISubscriptionService
{
    private readonly AppDbContext _context;
    private readonly IUserService _userService;

    public SubscriptionService(AppDbContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    // Tạo subscription mới
    public async Task<Subscription> CreateSubscription(User user)
    {
        var subscription = new Subscription
        {
            User = user,
            SubscriptionDateStart = DateTime.Now,
            SubscriptionDateEnd = DateTime.Now.AddMonths(12),
            IsValid = true,
            PlanType = PlanType.FREE
        };

        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();
        return subscription;
    }

    // Lấy subscription của user và đảm bảo valid
    public async Task<Subscription> GetUserSubscription(long userId)
    {
        var subscription = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.UserId == userId);

        if (!IsValid(subscription))
        {
            subscription.PlanType = PlanType.FREE;
            subscription.SubscriptionDateStart = DateTime.Now;
            subscription.SubscriptionDateEnd = DateTime.Now.AddMonths(12);
        }

        _context.Subscriptions.Update(subscription);
        await _context.SaveChangesAsync();
        return subscription;
    }

    //  Nâng cấp subscription
    public async Task<Subscription> UpgradeSubscription(long userId, PlanType planType)
    {
        var subscription = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.UserId == userId);

        if (subscription == null)
            throw new Exception("Subscription not found");

        subscription.PlanType = planType;
        subscription.SubscriptionDateStart = DateTime.Now;

        if (planType == PlanType.ANNUALLY)
            subscription.SubscriptionDateEnd = DateTime.Now.AddMonths(12);
        else
            subscription.SubscriptionDateEnd = DateTime.Now.AddMonths(1);

        _context.Subscriptions.Update(subscription);
        await _context.SaveChangesAsync();
        return subscription;
    }

    //  Check subscription còn hiệu lực
    public bool IsValid(Subscription subscription)
    {
        if (subscription.PlanType == PlanType.FREE) return true;

        var endDate = subscription.SubscriptionDateEnd;
        var currentDate = DateTime.Now;
        return endDate.Date >= currentDate.Date;
    }
}