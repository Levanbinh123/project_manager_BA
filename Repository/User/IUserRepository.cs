public interface IUserRepository
{
    Task<User?> FindByEmailAsync(string email);
    Task<User?> GetByIdAsync(long id);
    Task<User> CreateAsync(User user);
}