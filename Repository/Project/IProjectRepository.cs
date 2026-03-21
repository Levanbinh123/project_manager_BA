public interface IProjectRepository
{
    Task<List<Project>> FindByOwnerAsync(long userId);

    Task<List<Project>> FindByNameContainingAndTeamContainingAsync(string partialName, long userId);

    Task<Project?> FindByIdWithChatAsync(long id);

    Task<List<Project>> FindByTeamContainingOrOwnerAsync(long userId);

    Task UnassignProjectsAsync(long userId);

    Task<List<Project>> FindProjectsByTeamContainingAsync(long userId);
}