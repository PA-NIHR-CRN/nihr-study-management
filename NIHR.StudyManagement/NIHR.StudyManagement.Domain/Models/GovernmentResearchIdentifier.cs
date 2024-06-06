
using NIHR.StudyManagement.Domain.EnumsAndConstants;

namespace NIHR.StudyManagement.Domain.Models
{
    public class GovernmentResearchIdentifier : DomainEntity
    {
        public string ShortTitle { get; set; }

        public string Sponsor { get; set; }

        public List<TeamMember> TeamMembers { get; set; }

        public List<LinkedSystemIdentifier> LinkedSystemIdentifiers { get; set; }
        
        public GovernmentResearchIdentifier()
        {
            ShortTitle = "";
            Sponsor = "";
            TeamMembers = new List<TeamMember>();
            LinkedSystemIdentifiers = new List<LinkedSystemIdentifier>();
        }

        public string GrisId
        {
            get
            {
                foreach (var identifier in LinkedSystemIdentifiers)
                {
                    if(identifier.IdentifierType == ResearchInitiativeIdentifierTypes.GrisId)
                    {
                        return identifier.Identifier;
                    }
                }

                return "";
            }
        }
    }
}
