namespace FrisbeeApp.EmailSender
{
    public interface IEmailService
    {
        void SendEmail(Message message, EmailTemplateType emailTemplateType);
    }
}
