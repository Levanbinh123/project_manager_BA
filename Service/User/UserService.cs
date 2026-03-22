using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    //private readonly IJwtProvider _jwtProvider; // bạn cần inject JwtProvider

    public UserService(AppDbContext context)
    {
        _context = context;
      
    }

    // Lấy user từ JWT
    // public async Task<User> FindUserProfileByJwt(string jwt)
    // {
    //     var email = _jwtProvider.GetEmailFromToken(jwt);
    //     return await FindUserByEmail(email);
    // }

    // Tìm user theo email
    public async Task<User> FindUserByEmail(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) throw new Exception("User not found");
        return user;
    }

    // Tìm user theo Id
    public async Task<User> FindUserById(long userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    // Cập nhật project size
    public async Task<User> UpdateUsersProjectSize(User user, int number)
    {
        user.ProjectSize += number;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    // Xóa user và xử lý liên kết
    public async Task DeleteUser(long userId)
    {
        var user = await _context.Users
            .Include(u => u.AssignedIssues)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) throw new Exception("User not found");

        //  Xóa user khỏi chat_users (nếu là many-to-many)
        var chats = await _context.Chats
            .Include(c => c.Users)
            .Where(c => c.Users.Contains(user))
            .ToListAsync();

        foreach (var chat in chats)
        {
            chat.Users.Remove(user);
        }

        //  Xóa user khỏi các team project
        var projects = await _context.Projects
            .Include(p => p.Team)
            .Where(p => p.Team.Contains(user))
            .ToListAsync();

        foreach (var project in projects)
        {
            project.Team.Remove(user);
        }

        //  Xử lý project mà user là owner
        var ownedProjects = await _context.Projects
            .Where(p => p.Owner.Id == userId)
            .ToListAsync();

        foreach (var project in ownedProjects)
        {
            project.Owner = null; // hoặc xóa project nếu muốn
        }

        //  Xử lý issue mà user là assignee
        var issues = await _context.Issues
            .Where(i => i.Assignee.Id == userId)
            .ToListAsync();

        foreach (var issue in issues)
        {
            issue.Assignee = null;
        }

        //  Xóa subscription
        var subscriptions = await _context.Subscriptions
            .Where(s => s.UserId == userId)
            .ToListAsync();
        _context.Subscriptions.RemoveRange(subscriptions);

        //  Xóa user
        _context.Users.Remove(user);

        await _context.SaveChangesAsync();
    }

    // Cập nhật user
    public async Task<User> UpdateUser(long userId, User userUpdate)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) throw new Exception("User not found");

        if (!string.IsNullOrEmpty(userUpdate.FullName)) user.FullName = userUpdate.FullName;
        if (!string.IsNullOrEmpty(userUpdate.Email)) user.Email = userUpdate.Email;
        if (userUpdate.Role != null) user.Role = userUpdate.Role;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    // Lấy tất cả user
    public async Task<List<User>> FindAllUsers()
    {
        return await _context.Users.ToListAsync();
    }
}