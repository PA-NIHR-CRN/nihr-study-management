namespace NIHR.StudyManagement.Api.Models.Dto
{
    public class LinkedSystemIdentifierDto
    {
        public string Identifier { get; set; } = "";

        public string SystemName { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Status { get; set; } = "";
    }
}