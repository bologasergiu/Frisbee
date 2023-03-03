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
        public bool AccountConfirmationEmail(Message message, EmailTemplateType emailTemplateType, ConfirmEmailModel model)
        {
            var emailMessage = CreateEmailConfirmationMessage(message, emailTemplateType, model);
            return Send(emailMessage);
        }

        public bool TrainingEmail(Message message, EmailTemplateType emailTemplateType, TrainingModel model)
        {
            var emailMessage = CreateTrainingMessage(message, emailTemplateType, model);
            return Send(emailMessage);
        }
        
        public bool TimeOffRequestEmail(Message message, EmailTemplateType emailTemplateType, TimeOffRequestModel model)
        {
            var emailMessage = CreateTimeOffRequestMessage(message, emailTemplateType, model);
            return Send(emailMessage);
        }
        
        public void SendConfirmationEmail(EmailTemplateType templateType, string email, ConfirmEmailModel model)
        {
            var message = new Message(new string[] { email }, "FrisbeeApp Confirm Registration", "");
            if (templateType == EmailTemplateType.ConfirmAccountPlayer)
            {
                AccountConfirmationEmail(message, EmailTemplateType.ConfirmAccountPlayer, model);
            }
        }

        public void SendNewTrainingNotification(EmailTemplateType templateType, List<string> emailList, TrainingModel model)
        {
            var message = new Message(emailList, "New Training added", "");
            if (templateType == EmailTemplateType.Training)
            {
                TrainingEmail(message, EmailTemplateType.Training, model);
            }
        }
        
        public async void SendTimeOffRequestEmail(EmailTemplateType templateType, List<string> emailList, TimeOffRequestModel model)
        {
            var message = new Message(emailList, "New Timeoff request added", "");
            if (templateType == EmailTemplateType.TimeOffRequest)
            {
                TimeOffRequestEmail(message,EmailTemplateType.TimeOffRequest, model);
            }
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
        private MimeMessage CreateTrainingMessage(Message message, EmailTemplateType emailTemplateType, TrainingModel model)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("bologasergiu22@gmail.com", "bologasergiu22@gmail.com"));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            if (emailTemplateType == EmailTemplateType.Training)
            {
                var pathToFile = Path.GetDirectoryName(Directory.GetCurrentDirectory())
                + Path.DirectorySeparatorChar.ToString()
                + "FrisbeeApp.EmailSender"
                + Path.DirectorySeparatorChar.ToString()
                + "EmailTemplates"
                + Path.DirectorySeparatorChar.ToString()
                + "AddTrainingTemplate.cshtml";

                var body = _templateFiller.FillTemplate(pathToFile, model);
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            }
            return emailMessage;
        }

        private MimeMessage CreateTimeOffRequestMessage(Message message, EmailTemplateType emailTemplateType, TimeOffRequestModel model)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("bologasergiu22@gmail.com", "bologasergiu22@gmail.com"));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            if (emailTemplateType == EmailTemplateType.TimeOffRequest)
            {
                var pathToFile = Path.GetDirectoryName(Directory.GetCurrentDirectory())
                + Path.DirectorySeparatorChar.ToString()
                + "FrisbeeApp.EmailSender"
                + Path.DirectorySeparatorChar.ToString()
                + "EmailTemplates"
                + Path.DirectorySeparatorChar.ToString()
                + "AddTimeOffRequestTemplate.cshtml";

                var body = _templateFiller.FillTemplate(pathToFile, model);
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            }
            return emailMessage;
        }

        private MimeMessage CreateEmailConfirmationMessage(Message message, EmailTemplateType emailTemplateType, ConfirmEmailModel model)
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
    }
}
