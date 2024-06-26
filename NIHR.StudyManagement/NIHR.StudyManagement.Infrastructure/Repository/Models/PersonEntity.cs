﻿using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{
    public partial class PersonEntity : DbEntity
    {
        public PersonEntity()
        {
            PersonNames = new HashSet<PersonNameEntity>();
            Practitioners = new HashSet<PractitionerEntity>();
        }

        public int Id { get; set; }

        public virtual ICollection<PersonNameEntity> PersonNames { get; set; }
        public virtual ICollection<PractitionerEntity> Practitioners { get; set; }
    }
}
