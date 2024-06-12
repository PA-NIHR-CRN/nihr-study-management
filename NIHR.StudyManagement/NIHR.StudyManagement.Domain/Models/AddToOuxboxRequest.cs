namespace NIHR.StudyManagement.Domain.Models
{
    public class AddToOuxboxRequest
    {
        public string Payload { get; set; } = "";

        public string SourceSystem { get; set; } = "";

        public string EventType { get; set; } = "";
    }
}
