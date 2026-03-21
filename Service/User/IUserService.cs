public interface IUserService
{
   // Task<User> FindUserProfileByJwt(string jwt);
    Task<User> FindUserByEmail(string email);
    Task<User> FindUserById(int userId);
    Task<User> UpdateUsersProjectSize(User user, int number);
    Task DeleteUser(int userId);
    Task<User> UpdateUser(int userId, User user);
    Task<List<User>> FindAllUsers();
}