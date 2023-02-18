﻿using RazorEngineCore;

namespace FrisbeeApp.EmailSender 
{
    public class TemplateFillerService : ITemplateFillerService
    {
        private IRazorEngine _razorEngine;

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