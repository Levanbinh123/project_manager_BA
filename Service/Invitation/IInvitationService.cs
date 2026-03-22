public interface IInvitationService
{
Task SendInvitation(string email, long projectId);
    Task<Invitation> AcceptInvitation(string token, long userId);
    Task<string> GetTokenByUserEmail(string email);
    Task DeleteToken(string token);
}