using FrisbeeApp.Context;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.EmailSender.Abstractions;
using FrisbeeApp.EmailSender.Common;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
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
        
        public void SendEmail(EmailTemplateType templateType, List<string> emailList, EmailInfo model)
        {
            Message message = new Message(emailList, null, null);

            if (templateType == EmailTemplateType.ConfirmAccountPlayer)
            {
                message.Subject = "FrisbeeApp Confirm Registration";
            }
            else if (templateType == EmailTemplateType.Training)
            {
                message.Subject = "New frisbee training added.";
            }
            else if(templateType == EmailTemplateType.TimeOffRequest)
            {
                message.Subject = "New timeoff request";
            }
            else if (templateType == EmailTemplateType.ApproveAccount)
            {
                message.Subject = "New user registration";
            }
            else if (templateType == EmailTemplateType.TimeOffRequestChangeStatus)
            {
                message.Subject = "Timeoff request status updated";
            }
            else if (templateType == EmailTemplateType.PasswordChanged)
            {
                message.Subject = "Password updated";
            }

            var emailMessage = CreateMessage(message, templateType, model);
            Send(emailMessage);
        }

        private MimeMessage CreateMessage(Message message, EmailTemplateType emailTemplateType, EmailInfo model)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("adm.frisbee.app@gmail.com", "adm.frisbee.app@gmail.com"));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            string pathToFile = Path.GetDirectoryName(Directory.GetCurrentDirectory())
                + Path.DirectorySeparatorChar.ToString()
                + "FrisbeeApp.EmailSender"
                + Path.DirectorySeparatorChar.ToString()
                + "EmailTemplates";

            if (emailTemplateType == EmailTemplateType.Training)
            {
                pathToFile = pathToFile
                    + Path.DirectorySeparatorChar.ToString()
                    + "AddTrainingTemplate.cshtml";
            }
            else if (emailTemplateType == EmailTemplateType.ConfirmAccountPlayer)
            {
                pathToFile = pathToFile
                    + Path.DirectorySeparatorChar.ToString()
                    + "ConfirmAccountTemplate.cshtml";           
            }
            else if (emailTemplateType == EmailTemplateType.TimeOffRequest)
            {
                pathToFile = pathToFile
                    + Path.DirectorySeparatorChar.ToString()
                    + "AddTimeOffRequestTemplate.cshtml";              
            }else if (emailTemplateType == EmailTemplateType.ApproveAccount)
            {
                pathToFile = pathToFile
                    + Path.DirectorySeparatorChar.ToString()
                    + "ApproveAccountTemplate.cshtml";
            }else if (emailTemplateType == EmailTemplateType.TimeOffRequestChangeStatus)
            {
                pathToFile = pathToFile
                    + Path.DirectorySeparatorChar.ToString()
                    + "TimeOffRequestChangeStatusTemplate.cshtml";
            }
            else if (emailTemplateType == EmailTemplateType.PasswordChanged)
            {
                pathToFile = pathToFile
                    + Path.DirectorySeparatorChar.ToString()
                    + "PasswordChanged.cshtml";
            }

            var body = _templateFiller.FillTemplate(pathToFile, model);
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

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
    }
}
