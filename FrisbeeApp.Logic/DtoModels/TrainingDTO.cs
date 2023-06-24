using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrisbeeApp.Logic.DtoModels
{
    public class TrainingDTO
    {
        public DateTime Date { get; set; }
        public float Duration { get; set; }
        public TrainingField Field { get; set; }
        public string Team { get; set; }
        public string CoachName { get; set; }
    }
}
