using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{
    public partial class ResearchInitiativeIdentifier : DbEntity
    {
        public ResearchInitiativeIdentifier()
        {
            GriMappings = new HashSet<GriMapping>();
        }

        public int Int { get; set; }
        public int SourceSystemId { get; set; }
        public string Value { get; set; } = null!;
        public int ResearchInitiativeIdentifierTypeId { get; set; }

        public virtual ResearchInitiativeIdentifierType ResearchInitiativeIdentifierType { get; set; } = null!;
        public virtual SourceSystem SourceSystem { get; set; } = null!;
        public virtual ICollection<GriMapping> GriMappings { get; set; }
    }
}
