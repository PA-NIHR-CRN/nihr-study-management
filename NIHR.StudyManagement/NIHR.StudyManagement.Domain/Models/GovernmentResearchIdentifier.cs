
namespace NIHR.StudyManagement.Domain.Models
{
    public class GovernmentResearchIdentifier : DomainEntity
    {
        public string Identifier { get; set; }

        public string ShortTitle { get; set; }

        public string Sponsor { get; set; }

        public List<TeamMember> TeamMembers { get; set; }

        public List<LinkedSystemIdentifier> LinkedSystemIdentifiers { get; set; }
        
        public GovernmentResearchIdentifier()
        {
            Identifier = "";
            ShortTitle = "";
            Sponsor = "";
            TeamMembers = new List<TeamMember>();
            LinkedSystemIdentifiers = new List<LinkedSystemIdentifier>();
        }
    }
}
