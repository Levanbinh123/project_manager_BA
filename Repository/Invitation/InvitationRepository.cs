using Microsoft.EntityFrameworkCore;

public class InvitationRepository : IInvitationRepository
{
    private readonly AppDbContext _context;
    public InvitationRepository(AppDbContext context)
    {
        _context=context;
    }
  public async Task<Invitation?> FindByTokenAsync(string token)
    {
        return await _context.Invitations
            .FirstOrDefaultAsync(i => i.Token == token);
    }
 public async Task<Invitation?> FindByEmailAsync(string email)
    {
        return await _context.Invitations
            .FirstOrDefaultAsync(i => i.Email == email);
    }
}