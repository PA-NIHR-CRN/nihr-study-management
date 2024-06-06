using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{
    public partial class ResearchInitiativeIdentifierType : DbEntity
    {
        public ResearchInitiativeIdentifierType()
        {
            Identifiers = new HashSet<GriMapping>();
        }

        public int Id { get; set; }
        public string Description { get; set; } = null!;

        public virtual ICollection<GriMapping> Identifiers { get; set; }
    }
}
