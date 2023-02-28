using FrisbeeApp.EmailSender.Common;

namespace FrisbeeApp.EmailSender.Abstractions
{
    public interface IEmailService
    {
        bool SendEmail(Message message, EmailTemplateType emailTemplateType, ConfirmEmailModel model);
    }
}
