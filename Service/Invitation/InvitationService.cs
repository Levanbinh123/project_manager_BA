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
       try
    {
        string token = Guid.NewGuid().ToString();

        var invitation = new Invitation
        {
            Email = email,
            ProjectId = projectId,
            Token = token,
            ExpiredAt = DateTime.Now.AddHours(24),
            IsUsed = false
        };

        _context.Invitations.Add(invitation);
        await _context.SaveChangesAsync();

        string message = $@"
<h3>Bạn được mời vào project</h3>
<p>Token của bạn:</p>
<h2>{token}</h2>
<p>Hãy copy token này vào ứng dụng để tham gia.</p>
";

await _emailService.SendEmail(
    email,
    "Project Invitation",
    message
);
    }
    catch (Exception ex)
    {
        Console.WriteLine("ERROR INVITE: " + ex.Message);
        throw;
    }
    }

    // Accept invitation
    public async Task<Invitation> AcceptInvitation(string token, long userId)
    {
        var invitation = await _context.Invitations
            .FirstOrDefaultAsync(i => i.Token == token);

        if (invitation == null)
            throw new Exception("Invitation not found!");
        if(invitation.ExpiredAt<DateTime.Now)
        throw new Exception("Invitation expired");
        if(invitation.IsUsed)
        throw new Exception("Invitation already used!");
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
            if(project.Team.Any(u=>u.Id==user.Id))
            throw new Exception("User already in project");

        // Add user vào team
        project.Team.Add(user);
        invitation.IsUsed=true;
    
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