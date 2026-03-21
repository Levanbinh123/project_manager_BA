using Microsoft.EntityFrameworkCore;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;
    private readonly IUserService _userService;

    public ProjectService(AppDbContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    // Tạo project mới
    public async Task<Project> CreateProject(ProjectDTO projectDto, User user)
    {
        var project = new Project
        {
            Name = projectDto.Name,
            Description = projectDto.Description,
            Tags = projectDto.Tags ?? new List<string>(),
            Owner = user,
            Category = projectDto.Category,
            Team = new List<User> { user }
        };

        var chat = new Chat
        {
           // Name = $"Chat for project: {projectDto.Name}",
            Project = project,
            Users = new List<User> { user }
        };

        project.Chat = chat;

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return project;
    }

    // Lấy project theo team + lọc category/tag
    public async Task<List<ProjectDTO>> GetProjectByTeam(User user, string category, string tag)
    {
        var projects = await _context.Projects
            .Include(p => p.Team)
            .Where(p => p.Team.Contains(user) || p.Owner.Id == user.Id)
            .ToListAsync();

        if (!string.IsNullOrEmpty(category))
            projects = projects.Where(p => p.Category == category).ToList();

        if (!string.IsNullOrEmpty(tag))
            projects = projects.Where(p => p.Tags != null && p.Tags.Contains(tag)).ToList();

        return projects.Select(p => new ProjectDTO
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Tags = p.Tags ?? new List<string>(),
            Category = p.Category ?? "No Category",
            Owner = p.Owner,
            Team = p.Team,
            Chat = p.Chat
        }).ToList();
    }

    // Lấy project theo Id
    public async Task<ProjectDTO> GetProjectById(int id)
    {
        var project = await _context.Projects
            .Include(p => p.Team)
            .Include(p => p.Chat)
            .Include(p => p.Owner)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
            throw new Exception("Project not found");

        return new ProjectDTO
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            Tags = project.Tags,
            Category = project.Category,
            Owner = project.Owner,
            Team = project.Team,
            Chat = project.Chat
        };
    }

    //  Xóa project
    public async Task DeleteProject(int projectId, int userId)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project == null)
            throw new Exception("Project not found");

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }

    //Cập nhật project
    public async Task<Project> UpdateProject(int id, UpdateProject projectDto)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
            throw new Exception("Project not found");

        project.Name = projectDto.Name;
        project.Description = projectDto.Description;
        project.Tags = projectDto.Tags ?? new List<string>();

        await _context.SaveChangesAsync();
        return project;
    }

    //Thêm user vào project
    public async Task AddUserToProject(int projectId, int userId)
    {
        var project = await _context.Projects
            .Include(p => p.Team)
            .Include(p => p.Chat)
            .ThenInclude(c => c.Users)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null) throw new Exception("Project not found");

        var user = await _userService.FindUserById(userId);
        if (!project.Team.Contains(user))
        {
            project.Team.Add(user);
            project.Chat.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }

    //  Xóa user khỏi project
    public async Task RemoveUserFromProject(int projectId, int userId)
    {
        var project = await _context.Projects
            .Include(p => p.Team)
            .Include(p => p.Chat)
            .ThenInclude(c => c.Users)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null) throw new Exception("Project not found");

        var user = await _userService.FindUserById(userId);

        if (project.Team.Contains(user))
        {
            project.Team.Remove(user);
            project.Chat.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    // Lấy chat theo projectId
    public async Task<Chat> GetChatByProjectId(int projectId)
    {
        var project = await _context.Projects
            .Include(p => p.Chat)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null || project.Chat == null)
            throw new Exception("Chat not found");

        return project.Chat;
    }

    //Search project theo keyword
    public async Task<List<ProjectDTO>> SearchProjects(string keyword, User user)
    {
        var projects = await _context.Projects
            .Include(p => p.Team)
            .Where(p => p.Team.Contains(user) && p.Name.Contains(keyword))
            .ToListAsync();

        return projects.Select(p => new ProjectDTO
        {
            Name = p.Name,
            Description = p.Description,
            Tags = p.Tags,
            Category = p.Category
        }).ToList();
    }
}