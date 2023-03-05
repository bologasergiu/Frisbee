using FrisbeeApp.EmailSender.Abstractions;
using RazorEngineCore;

namespace FrisbeeApp.EmailSender.Common
{
    public class TemplateFillerService : ITemplateFillerService
    {
        private readonly IRazorEngine _razorEngine;

        public TemplateFillerService(IRazorEngine razorEngine)
        {
            _razorEngine = razorEngine;
        }

        public string FillTemplate(string path, object model)
        {
            string template = File.ReadAllText(path);
            IRazorEngineCompiledTemplate compiledTemplate = _razorEngine.Compile(template);
            string body = compiledTemplate.Run(model);

            return body;
        }
    }
}