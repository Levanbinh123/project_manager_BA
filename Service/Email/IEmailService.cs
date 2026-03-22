public interface IEmailService
{
    Task SendEmailWithToken(string userEmail, string link);
}