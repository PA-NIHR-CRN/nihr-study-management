﻿using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{
    public partial class GriResearchStudy : DbEntity
    {
        public GriResearchStudy()
        {
            ResearchStudyIdentifiers = new HashSet<GriMapping>();
            ResearchStudyTeamMembers = new HashSet<ResearchStudyTeamMember>();
        }

        public int Id { get; set; }
        public string Gri { get; set; } = null!;
        public string ShortTitle { get; set; } = null!;
        public int RequestSourceSystemId { get; set; }

        public virtual SourceSystem RequestSourceSystem { get; set; } = null!;
        public virtual ICollection<GriMapping> ResearchStudyIdentifiers { get; set; }
        public virtual ICollection<ResearchStudyTeamMember> ResearchStudyTeamMembers { get; set; }
    }
}
