using System.ComponentModel.DataAnnotations;

namespace FrisbeeApp.Logic.Common
{
    public class SearchCriteria
    {
        public string? PlayerName { get; set; }
        public RequestStatus? Status { get; set; }
        public TimeOffRequestType? RequestType { get; set; }
    }
}
