using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.EmailSender.Common;

namespace FrisbeeApp.EmailSender.Abstractions
{
    public interface IEmailService
    {
        //bool SendEmail(Message message, EmailTemplateType emailTemplateType, ConfirmEmailModel model);
       
        void SendConfirmationEmail(EmailTemplateType templateType, string email, ConfirmEmailModel model);
        
        void SendNewTrainingNotification(EmailTemplateType training, List<string> emailList, TrainingModel trainingModel);
        void SendTimeOffRequestEmail(EmailTemplateType templateType, List<string> email, TimeOffRequestModel model);
    }
}
