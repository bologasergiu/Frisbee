using System.ComponentModel.DataAnnotations;

namespace Frisbee.ApiModels
{
    public class TimeOffRequestApiModel : IValidatableObject
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeOffRequestType RequestType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate < DateTime.Now)
            {
                yield return new ValidationResult(errorMessage: "Date can't be in the past!", memberNames: new[] { "StartDate" });
            } 
            if(EndDate.Day - StartDate.Day > 14)
            {
                yield return new ValidationResult(errorMessage: "TimeOff request should be less than 14 days", memberNames: new[] { "EndDate" });
            }
            if (RequestType != 0)
            {
                if (!Enum.IsDefined(typeof(TimeOffRequestType), RequestType))
                {
                    yield return new ValidationResult(errorMessage: "Invalid request type.", memberNames: new[] { "RequestType" });
                }
            }
        }
    }
}
