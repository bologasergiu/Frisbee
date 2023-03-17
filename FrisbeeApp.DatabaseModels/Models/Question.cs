namespace FrisbeeApp.DatabaseModels.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool CorrectAnswer { get; set; }
        public bool IncorrectAnswer { get; set; }
    }
}
