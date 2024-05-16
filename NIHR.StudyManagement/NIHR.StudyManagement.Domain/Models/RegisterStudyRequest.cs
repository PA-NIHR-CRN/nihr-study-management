namespace NIHR.StudyManagement.Domain.Models
{

    public class RegisterStudyRequest
    {
        public string ProjectId { get; set; }

        public PersonWithPrimaryEmail ChiefInvestigator { get; set; }

        public string ShortTitle { get; set; }

        public string Sponsor { get; set; }

        public string ProtocolId { get; set; }

        public string StatusCode { get; set; }

        public List<ResearchInitiativeIdentifierItem> Identifiers { get; set; }

        public RegisterStudyRequest()
        {
            ProjectId = "";
            ChiefInvestigator = new PersonWithPrimaryEmail();
            ShortTitle = "";
            Sponsor = "";
            ProtocolId = "";
            StatusCode = "";
            Identifiers = new List<ResearchInitiativeIdentifierItem>();
        }
    }

    public class ResearchInitiativeIdentifierItem
    {
        public string Value { get; set; }

        public string Type { get; set; }

        public DateTime Created { get; set; }

        public ResearchInitiativeIdentifierItem()
        {
            Value = "";
            Type = "";
            Created = DateTime.Now;
        }
    }


}
