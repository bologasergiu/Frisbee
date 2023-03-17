using FrisbeeApp.EmailSender.Common;

namespace FrisbeeApp.EmailSender.Abstractions
{
    public interface IEmailService
    {
        void SendEmail(EmailTemplateType templateType, List<string> emailList, EmailInfo model);
    }
}
