public interface IUserService
{
   // Task<User> FindUserProfileByJwt(string jwt);
    Task<User> FindUserByEmail(string email);
    Task<User> FindUserById(long userId);
    Task<User> UpdateUsersProjectSize(User user, int number);
    Task DeleteUser(long userId);
    Task<User> UpdateUser(long userId, User user);
    Task<List<User>> FindAllUsers();
}