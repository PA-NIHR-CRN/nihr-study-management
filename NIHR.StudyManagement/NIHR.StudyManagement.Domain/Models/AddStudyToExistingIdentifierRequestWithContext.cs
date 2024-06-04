namespace NIHR.StudyManagement.Domain.Models
{
    public class AddStudyToExistingIdentifierRequestWithContext
    {
        public RegisterStudyToExistingIdentifierRequest RequestContext { get; set; }

        public List<LinkedSystemIdentifier> LinkedSystemIdentifiersToAdd { get; set; }

        public List<TeamMember> TeamMembersToAdd { get; set; }

        public AddStudyToExistingIdentifierRequestWithContext()
        {
            RequestContext = new RegisterStudyToExistingIdentifierRequest();
            LinkedSystemIdentifiersToAdd = new List<LinkedSystemIdentifier>();
            TeamMembersToAdd = new List<TeamMember>();
        }
    }
}
