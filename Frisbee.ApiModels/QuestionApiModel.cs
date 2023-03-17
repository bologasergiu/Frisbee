using System.ComponentModel.DataAnnotations;

namespace Frisbee.ApiModels
{
    public class QuestionApiModel 
    {
        public string Text { get; set; }
        public bool CorrectAnswer { get; set; }
        public bool IncorrectAnswer { get; set; }
    }
}
