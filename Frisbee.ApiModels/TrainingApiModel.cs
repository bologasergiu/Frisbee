using System.ComponentModel.DataAnnotations;

namespace Frisbee.ApiModels
{
    public class TrainingApiModel : IValidatableObject
    {
        public DateTime Date { get; set; }
        public float Duration { get; set; }
        public TrainingField Field { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Date < DateTime.Now)
            {
                yield return new ValidationResult(errorMessage: "Date can't be in the past!", memberNames: new[] { "Date" });
            }
            if(Duration < 1)
            {
                yield return new ValidationResult(errorMessage: "Training should last at least one hour!", memberNames: new[] { "Duration" });
            }
            if (Field != 0)
            {
                if (!Enum.IsDefined(typeof(TrainingField), Field))
                {
                    yield return new ValidationResult(errorMessage: "Invalid Field.", memberNames: new[] { "Field" });
                }
            }
        }
    }
}
