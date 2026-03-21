using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailWithToken(string userEmail, string link)
    {
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress("Project System", _emailSettings.Email));
        email.To.Add(MailboxAddress.Parse(userEmail));
        email.Subject = "Join Project Team Invitation";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $"<p>Click link to join project:</p>" +
                       $"<a href='{link}'>Join Project</a>"
        };

        email.Body = bodyBuilder.ToMessageBody();

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);

        await smtp.SendAsync(email);

        await smtp.DisconnectAsync(true);
    }
}