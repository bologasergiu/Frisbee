using MailKit.Net.Smtp;
using MimeKit;
using Org.BouncyCastle.Asn1.Ocsp;

namespace FrisbeeApp.EmailSender
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly ITemplateFillerService _templateFiller;
        public EmailService(EmailConfiguration emailConfig, ITemplateFillerService templateFiller)
        {
            _emailConfig = emailConfig;
            _templateFiller = templateFiller;

        }
        public void SendEmail(Message message, EmailTemplateType emailTemplateType)
        {
            var emailMessage = CreateEmailMessage(message, emailTemplateType);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message, EmailTemplateType emailTemplateType)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("bologasergiu22@gmail.com","bologasergiu22@gmail.com"));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            if(emailTemplateType == EmailTemplateType.ConfirmAccountPlayer)
            {
                var pathToFile = Path.GetDirectoryName(Directory.GetCurrentDirectory())
            + Path.DirectorySeparatorChar.ToString()
            + "FrisbeeApp.EmailSender"
            + Path.DirectorySeparatorChar.ToString()
            + "EmailTemplates"
            + Path.DirectorySeparatorChar.ToString()
            + "ConfirmAccountTemplate.cshtml";

                object model = new
                {
                    FirstName = "Sergiu",
                    LastName = "Bologa",
                    Link = "blank"
                };

                var body = _templateFiller.FillTemplate(pathToFile, model);
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            }
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
