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
        private IConfiguration _configuration;

        public  SenGridMailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


            public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var apiKey = _configuration["API_KEY"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ayaconsultorestributarios@gmail.com");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
        }

    }
}
