using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace alkemyapi.Data
{
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
    }

    public class SenGridMailService : IMailService
    {
       
        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("gerardojao@gmail.com", "@gerardojao");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
        }

    }
}
