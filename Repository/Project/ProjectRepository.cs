using Microsoft.EntityFrameworkCore;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    // findByOwner
    public async Task<List<Project>> FindByOwnerAsync(long userId)
    {
        return await _context.Projects
            .Where(p => p.OwnerId == userId)
            .ToListAsync();
    }

    //  findByNameContainingAndTeamContaining
    public async Task<List<Project>> FindByNameContainingAndTeamContainingAsync(string partialName, long userId)
    {
        return await _context.Projects
            .Where(p => p.Name.Contains(partialName)
                     && p.Team.Any(u => u.Id == userId))
            .ToListAsync();
    }

    //  findByIdWithChat (LEFT JOIN FETCH)
    public async Task<Project?> FindByIdWithChatAsync(long id)
    {
        return await _context.Projects
            .Include(p => p.Chat)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    // findByTeamContainingOrOwner
    public async Task<List<Project>> FindByTeamContainingOrOwnerAsync(long userId)
    {
        return await _context.Projects
            .Where(p => p.OwnerId == userId 
                     || p.Team.Any(u => u.Id == userId))
            .ToListAsync();
    }

    //  UPDATE 
    public async Task UnassignProjectsAsync(long userId)
    {
        var projects = await _context.Projects
            .Where(p => p.OwnerId == userId)
            .ToListAsync();

        foreach (var p in projects)
        {
            p.OwnerId = null;
        }

        await _context.SaveChangesAsync();
    }

    //  MEMBER OF team
    public async Task<List<Project>> FindProjectsByTeamContainingAsync(long userId)
    {
        return await _context.Projects
            .Where(p => p.Team.Any(u => u.Id == userId))
            .ToListAsync();
    }
}