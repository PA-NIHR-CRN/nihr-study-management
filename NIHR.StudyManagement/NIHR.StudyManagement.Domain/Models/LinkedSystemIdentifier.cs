namespace NIHR.StudyManagement.Domain.Models
{
    public class LinkedSystemIdentifier
    {
        public string Identifier { get; set; }
        public string SystemName { get; set; }

        public DateTime CreatedAt { get; set; }

        public LinkedSystemIdentifier()
        {
            Identifier = "";
            SystemName = "";
            CreatedAt = DateTime.Now;
        }
    }
}
