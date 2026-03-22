public interface IProjectService
{
    Task<Project> CreateProject(ProjectDTO project, User user);
    Task<List<ProjectDTO>> GetProjectByTeam(User user, string category, string tag);
    Task<ProjectDTO> GetProjectById(long id);
    Task DeleteProject(long projectId, long userId);
    Task<Project> UpdateProject(long id, UpdateProject project);
    Task AddUserToProject(long projectId, long userId);
    Task RemoveUserFromProject(long projectId, long userId);
    Task<Chat> GetChatByProjectId(long projectId);
    Task<List<ProjectDTO>> SearchProjects(string keyword, User user);
}