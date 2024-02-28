using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{
    public partial class ResearchInitiativeIdentifierType : DbEntity
    {
        public ResearchInitiativeIdentifierType()
        {
            ResearchInitiativeIdentifiers = new HashSet<ResearchInitiativeIdentifier>();
        }

        public int Id { get; set; }
        public string Description { get; set; } = null!;

        public virtual ICollection<ResearchInitiativeIdentifier> ResearchInitiativeIdentifiers { get; set; }
    }
}
