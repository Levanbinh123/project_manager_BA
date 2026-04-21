using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Issue> Issues { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Invitation> Invitations { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
          base.OnModelCreating(modelBuilder);

    // =========================
    // MANY-TO-MANY
    // =========================

    modelBuilder.Entity<Chat>()
        .HasMany(c => c.Users)
        .WithMany();

    modelBuilder.Entity<Project>()
        .HasMany(p => p.Team)
        .WithMany();

    // =========================
    // ONE-TO-ONE
    // =========================

    modelBuilder.Entity<Project>()
        .HasOne(p => p.Chat)
        .WithOne(c => c.Project)
        .HasForeignKey<Chat>(c => c.ProjectId)
        .OnDelete(DeleteBehavior.Cascade);

    // =========================
    // ONE-TO-MANY
    // =========================

    // Project -> Issue
    modelBuilder.Entity<Issue>()
        .HasOne(i => i.Project)
        .WithMany(p => p.Issues)
        .HasForeignKey(i => i.ProjectId)
        .OnDelete(DeleteBehavior.Cascade);

    // Issue -> Comment
    modelBuilder.Entity<Comment>()
        .HasOne(c => c.Issue)
        .WithMany(i => i.Comments)
        .HasForeignKey(c => c.IssueId)
        .OnDelete(DeleteBehavior.Cascade);

    // Chat -> Message
    modelBuilder.Entity<Message>()
        .HasOne(m => m.chat)
        .WithMany(c => c.Messages)
        .HasForeignKey(m => m.ChatId)
        .OnDelete(DeleteBehavior.Cascade);

    // Message -> User (Sender)
    modelBuilder.Entity<Message>()
        .HasOne(m => m.Sender)
        .WithMany()
        .HasForeignKey(m => m.SenderId)
        .OnDelete(DeleteBehavior.SetNull);

    // Issue -> Assignee
    modelBuilder.Entity<Issue>()
        .HasOne(i => i.Assignee)
        .WithMany(u => u.AssignedIssues)
        .HasForeignKey(i => i.AssigneeId)
        .OnDelete(DeleteBehavior.SetNull);

    // Project -> Invitation
    modelBuilder.Entity<Invitation>()
        .HasOne(i => i.Project)
        .WithMany()
        .HasForeignKey(i => i.ProjectId)
        .OnDelete(DeleteBehavior.Cascade);
}}