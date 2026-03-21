public interface IInvitationService
{
Task SendInvitation(string email, int projectId);
    Task<string> AcceptInvitation(string token, int userId);
    Task<string> GetTokenByUserEmail(string email);
    Task DeleteToken(string token);
}