public interface IInvitationRepository
{
     Task<Invitation?> FindByTokenAsync(string token);
    Task<Invitation?> FindByEmailAsync(string email);
}