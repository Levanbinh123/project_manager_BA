using Microsoft.EntityFrameworkCore;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly AppDbContext _context;

    public SubscriptionRepository(AppDbContext context)
    {
        _context = context;
    }

    // findByUserId
    public async Task<Subscription?> FindByUserIdAsync(long userId)
    {
        return await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.UserId == userId);
    }

    //  deleteByUserId (cách thường)
    public async Task DeleteByUserIdAsync(long userId)
    {
        var subs = await _context.Subscriptions
            .Where(s => s.UserId == userId)
            .ToListAsync();

        _context.Subscriptions.RemoveRange(subs);
        await _context.SaveChangesAsync();
    }
}