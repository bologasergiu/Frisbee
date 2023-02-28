using FrisbeeApp.Context;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.EmailSender.Abstractions;
using FrisbeeApp.EmailSender.Common;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace FrisbeeApp.EmailSender.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly FrisbeeAppContext _context;
        private readonly UserManager<User> _userManager;
        private readonly EmailConfiguration _emailConfig;
        private readonly ITemplateFillerService _templateFiller;

        public EmailService(FrisbeeAppContext context, UserManager<User> userManager, EmailConfiguration emailConfig, ITemplateFillerService templateFiller)
        {
            _context = context;
            _userManager = userManager;
            _emailConfig = emailConfig;
            _templateFiller = templateFiller;
        }
        public bool SendEmail(Message message, EmailTemplateType emailTemplateType, ConfirmEmailModel model)
        {
            var emailMessage = CreateEmailMessage(message, emailTemplateType, model);
            return Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message, EmailTemplateType emailTemplateType, ConfirmEmailModel model)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("bologasergiu22@gmail.com", "bologasergiu22@gmail.com"));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            if (emailTemplateType == EmailTemplateType.ConfirmAccountPlayer)
            {
                var pathToFile = Path.GetDirectoryName(Directory.GetCurrentDirectory())
                + Path.DirectorySeparatorChar.ToString()
                + "FrisbeeApp.EmailSender"
                + Path.DirectorySeparatorChar.ToString()
                + "EmailTemplates"
                + Path.DirectorySeparatorChar.ToString()
                + "ConfirmAccountTemplate.cshtml";

                var body = _templateFiller.FillTemplate(pathToFile, model);
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            }
            return emailMessage;
        }

        private bool Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                    client.Send(mailMessage);
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
        public void TemplateType(EmailTemplateType templateType, string email, ConfirmEmailModel model)
        {
            var message = new Message(new string[] { email }, "FrisbeeApp Confirm Registration", "");
            if (templateType == EmailTemplateType.ConfirmAccountPlayer)
            {
                SendEmail(message, EmailTemplateType.ConfirmAccountPlayer, model);
            }

        }

        public async Task<bool> ConfirmAccount(string email, string userToken)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (dbUser != null && userToken != null && dbUser.EmailConfirmed != true)
            {
                var result = await _userManager.ConfirmEmailAsync(dbUser, userToken);
                return result == IdentityResult.Success ? true : false;
            }
            return false;
        }

        public async Task<string> GenerateRegistrationToken(string email)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            string token = null;
            if (dbUser != null)
            {
                token = await _userManager.GenerateEmailConfirmationTokenAsync(dbUser);
            }
            return token;
        }
    }
}
