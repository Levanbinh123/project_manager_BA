public interface IEmailService
{
    Task SendEmailWithToken(string userEmail, string link);
     Task SendEmail(string userEmail, string subject, string content);
}