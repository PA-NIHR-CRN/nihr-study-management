using System;

namespace NIHR.StudyManagement.Api.Models.Dto
{
    public class GovernmentResearchIdentifierDto
    {
        public DateTime? CreatedAt { get; set; }

        public string Gri { get; set; }

        public string ShortTitle { get; set; }

        public List<LinkedSystemIdentifierDto> LinkedSystemIdentifiers { get; set; }

        public List<TeamMemberDto> TeamMembers { get; set; }

        public GovernmentResearchIdentifierDto()
        {
            CreatedAt = DateTime.Now;
            Gri = "";
            ShortTitle = "";
            TeamMembers = new List<TeamMemberDto>();
            LinkedSystemIdentifiers = new List<LinkedSystemIdentifierDto>();
        }
    }
}