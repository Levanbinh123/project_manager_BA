public interface IProjectService
{
    Task<Project> CreateProject(ProjectDTO project, User user);
    Task<List<ProjectDTO>> GetProjectByTeam(User user, string category, string tag);
    Task<ProjectDTO> GetProjectById(int id);
    Task DeleteProject(int projectId, int userId);
    Task<Project> UpdateProject(int id, UpdateProject project);
    Task AddUserToProject(int projectId, int userId);
    Task RemoveUserFromProject(int projectId, int userId);
    Task<Chat> GetChatByProjectId(int projectId);
    Task<List<ProjectDTO>> SearchProjects(string keyword, User user);
}