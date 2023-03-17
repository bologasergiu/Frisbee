namespace FrisbeeApp.EmailSender.Abstractions
{
    public interface ITemplateFillerService
    {
        string FillTemplate(string path, object model);
    }
}