public class Subscription
{
    public long Id { get; set; }

    public DateTime SubscriptionDateStart { get; set; }

    public DateTime SubscriptionDateEnd { get; set; }

    public PlanType PlanType { get; set; }

    public bool IsValid { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }
}
public enum PlanType
{
     FREE,
    MONTHLY,
    ANNUALLY,
}