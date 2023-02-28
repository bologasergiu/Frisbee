using FrisbeeApp.EmailSender.Common;

namespace FrisbeeApp.EmailSender.Abstractions
{
    public interface IEmailService
    {
        bool SendEmail(Message message, EmailTemplateType emailTemplateType, ConfirmEmailModel model);
        Task<bool> ConfirmAccount(string email, string userToken);
        Task<string> GenerateRegistrationToken(string email);
        void TemplateType(EmailTemplateType templateType, string email, ConfirmEmailModel model);
    }
}
