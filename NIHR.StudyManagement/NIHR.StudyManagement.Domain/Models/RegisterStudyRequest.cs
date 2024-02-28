namespace NIHR.StudyManagement.Domain.Models
{

    public class RegisterStudyRequest
    {
        public string ProjectId { get; set; }

        public PersonWithPrimaryEmail ChiefInvestigator { get; set; }

        public string ShortTitle { get; set; }

        public string Sponsor { get; set; }

        public string ProtocolId { get; set; }

        public RegisterStudyRequest()
        {
            ProjectId = "";
            ChiefInvestigator = new PersonWithPrimaryEmail();
            ShortTitle = "";
            Sponsor = "";
            ProtocolId = "";
        }
    }
}
