namespace NIHR.StudyManagement.Domain.Models
{
    public class RegisterStudyRequest
    {
        public List<TeamMember> TeamMembers { get; set; }

        public string ShortTitle { get; set; }

        public string ApiSystemName { get; set; }

        public List<ResearchInitiativeIdentifierItem> Identifiers { get; set; }

        public RegisterStudyRequest()
        {
            ShortTitle = "";
            ApiSystemName = "";
            TeamMembers = new List<TeamMember>();
            Identifiers = new List<ResearchInitiativeIdentifierItem>();
        }
    }

    public class ResearchInitiativeIdentifierItem
    {
        public string Value { get; set; }

        public string Type { get; set; }

        public DateTime Created { get; set; }

        public string StatusCode { get; set; }

        public ResearchInitiativeIdentifierItem()
        {
            Value = "";
            Type = "";
            Created = DateTime.Now;
            StatusCode = "";
        }
    }


}
