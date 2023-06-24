namespace FrisbeeApp.Logic.DtoModels
{
    public class TimeOffRequestPlayerDTO
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RequestStatus Status { get; set; }
        public TimeOffRequestType RequestType { get; set; }
    }
}
