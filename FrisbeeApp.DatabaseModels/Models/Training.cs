using System.ComponentModel.DataAnnotations.Schema;

namespace FrisbeeApp.DatabaseModels.Models
{
    public class Training
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public float Duration { get; set; }    
        public TrainingField Field { get; set; }
        public string Team { get; set; }
        public string CoachName { get; set; }
        [NotMapped]
        public List<string> GoingPlayers { get; set; }
        [NotMapped]
        public List<string> NoNGoingPlayers { get; set; }
    }
}
