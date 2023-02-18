namespace FrisbeeApp.EmailSender
{
    public interface ITemplateFillerService
    {
        string FillTemplate(string path, object model);
    }
}