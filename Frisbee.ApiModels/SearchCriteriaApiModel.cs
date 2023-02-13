using System.ComponentModel.DataAnnotations;

namespace Frisbee.ApiModels
{
    public class SearchCriteriaApiModel : IValidatableObject
    {
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "LastName can only contain letters.")]
        public string? PlayerName { get; set; }
        public RequestStatus? Status { get; set; }
        public TimeOffRequestType? RequestType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Status != null)
            {
                if (!Enum.IsDefined(typeof(RequestStatus), Status))
                {
                    yield return new ValidationResult(errorMessage: "Invalid status.", memberNames: new[] { "Status" });
                }
            }
            if (RequestType != null)
            {
                if (!Enum.IsDefined(typeof(TimeOffRequestType), RequestType))
                {
                    yield return new ValidationResult(errorMessage: "Invalid request type.", memberNames: new[] { "RequestType" });
                }
            }
        }
    }
}
