using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class InvitationService : IInvitationService
{
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;

    public InvitationService(AppDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    // Gửi invitation
    public async Task SendInvitation(string email, long projectId)
    {
        // Tạo token
        string token = Guid.NewGuid().ToString();

        var invitation = new Invitation
        {
            Email = email,
            ProjectId = projectId,
            Token = token
        };

        _context.Invitations.Add(invitation);
        await _context.SaveChangesAsync();

        // Link frontend
        string link = $"https://project-managerment-react12.vercel.app/accept_invitation?token={token}";

        await _emailService.SendEmailWithToken(email, link);
    }

    // Accept invitation
    public async Task<Invitation> AcceptInvitation(string token, long userId)
    {
        var invitation = await _context.Invitations
            .FirstOrDefaultAsync(i => i.Token == token);

        if (invitation == null)
            throw new Exception("Invitation not found!");

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            throw new Exception("User not found!");

        // Check email match
        if (user.Email != invitation.Email)
            throw new Exception("Email does not match invitation!");

        var project = await _context.Projects
            .Include(p => p.Team)
            .FirstOrDefaultAsync(p => p.Id == invitation.ProjectId);

        if (project == null)
            throw new Exception("Project not found!");

        // Add user vào team
        project.Team.Add(user);

        await _context.SaveChangesAsync();

        // Xóa token
        _context.Invitations.Remove(invitation);
        await _context.SaveChangesAsync();

        return invitation;
    }

    // Lấy token theo email
    public async Task<string> GetTokenByUserEmail(string email)
    {
        var invitation = await _context.Invitations
            .FirstOrDefaultAsync(i => i.Email == email);

        return invitation?.Token;
    }

    //Xóa token
    public async Task DeleteToken(string token)
    {
        var invitation = await _context.Invitations
            .FirstOrDefaultAsync(i => i.Token == token);

        if (invitation != null)
        {
            _context.Invitations.Remove(invitation);
            await _context.SaveChangesAsync();
        }
    }
}