namespace FrisbeeApp.DatabaseModels.Models
{
    public class TimeOffRequest
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string TeamName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RequestStatus Status { get; set; }
        public TimeOffRequestType RequestType { get; set; }
    }
}
