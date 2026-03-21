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
        // Many-to-Many Chat - User
        modelBuilder.Entity<Chat>()
            .HasMany(c => c.Users)
            .WithMany();

        // Many-to-Many Project - User
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Team)
            .WithMany();

        // One-to-One Project - Chat
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Chat)
            .WithOne(c => c.Project)
            .HasForeignKey<Chat>(c => c.ProjectId);
    }
}