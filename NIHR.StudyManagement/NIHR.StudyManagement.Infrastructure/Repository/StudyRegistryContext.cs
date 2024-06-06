using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NIHR.StudyManagement.Infrastructure.Repository.EnumsAndConstants;
using NIHR.StudyManagement.Infrastructure.Repository.Models;


/// Changes
/// griResearchStudyStatus (Rename to researchStudyStatus)
///     Drop GriMappingId and reference ResearchStudyId
/// griResearchStudy (Rename to researchStudy)
///     Drop researchInitiativeId & reference
/// griMapping (rename to researchStudyIdentifiers)
/// researchInitiative (drop table)     
/// researchInitiativeIdentifier (drop table, migrate to researchStudyIdentifiers)
/// researchInitiativeType (drop table)
namespace NIHR.StudyManagement.Infrastructure.Repository
{
    public partial class StudyRegistryContext : DbContext
    {
        public StudyRegistryContext()
        {
        }

        public StudyRegistryContext(DbContextOptions<StudyRegistryContext> options)
            : base(options)
        {
        }

        //public virtual DbSet<GriMapping> GriMappings { get; set; } = null!;
        public virtual DbSet<ResearchStudyEntity> ResearchStudies { get; set; } = null!;
        public virtual DbSet<PersonEntity> People { get; set; } = null!;
        public virtual DbSet<PersonNameEntity> PersonNames { get; set; } = null!;
        public virtual DbSet<RoleTypeEntity> PersonRoles { get; set; } = null!;

        public virtual DbSet<OrganisationEntity> OrganisationEntities { get; set; } = null!;

        public virtual DbSet<ResearchStudyIdentifierTypeEntity> ResearchInitiativeIdentifierTypes { get; set; } = null!;

        public virtual DbSet<ResearchStudyTeamMember> ResearchStudyTeamMembers { get; set; } = null!;
        public virtual DbSet<Researcher> Researchers { get; set; } = null!;
        public virtual DbSet<SourceSystem> SourceSystems { get; set; } = null!;
        public virtual DbSet<ResearchStudyIdentifierStatusEntity> GriResearchStudyStatuses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResearchStudyIdentifierEntity>(entity =>
            {
                entity.ToTable("researchStudyIdentifier");

                entity.HasIndex(e => e.GriResearchStudyId, "fk_griMapping_griResearchStudy_idx");

                //entity.HasIndex(e => e.IdentifierId, "fk_griMapping_researchInitiativeIdentifier_idx");

                entity.Property(e => e.Value)
                    .HasMaxLength(150)
                    .HasColumnName("value");

                entity.HasIndex(e => e.SourceSystemId, "fk_griMapping_sourceSystem_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GriResearchStudyId).HasColumnName("griResearchStudy_id");

                //entity.Property(e => e.IdentifierId).HasColumnName("researchInitiativeIdentifier_id");

                entity.Property(e => e.SourceSystemId).HasColumnName("sourceSystem_id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.HasOne(d => d.GriResearchStudy)
                    .WithMany(p => p.ResearchStudyIdentifiers)
                    .HasForeignKey(d => d.GriResearchStudyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_griMapping_griResearchStudy");

                //entity.HasOne(d => d.Identifier)
                //    .WithMany(p => p.Identifiers)
                //    .HasForeignKey(d => d.IdentifierId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("fk_griMapping_researchInitiativeIdentifier");

                entity.HasOne(d => d.SourceSystem)
                    .WithMany(p => p.GriMappings)
                    .HasForeignKey(d => d.SourceSystemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_griMapping_sourceSystem");
            });

            modelBuilder.Entity<ResearchStudyIdentifierStatusEntity>(entity =>
            {
                entity.ToTable("researchStudyIdentifierStatus");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.Code)
                    .HasMaxLength(100)
                    .HasColumnName("code");

                entity.HasOne(d => d.ResearchStudyIdentifier)
                    .WithMany(p => p.IdentifierStatuses)
                    .HasForeignKey(d => d.ResearchStudyIdentifierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_griResearchStudyStatus_griMapping");
            });

            modelBuilder.Entity<ResearchStudyEntity>(entity =>
            {
                entity.ToTable("researchStudy");

                entity.HasIndex(e => e.RequestSourceSystemId, "fk_griResearchStudy_sourceSystem_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.Gri)
                    .HasMaxLength(100)
                    .HasColumnName("gri");

                entity.Property(e => e.RequestSourceSystemId).HasColumnName("requestSourceSystem_id");

                entity.Property(e => e.ShortTitle)
                    .HasMaxLength(150)
                    .HasColumnName("shortTitle");

                entity.HasOne(d => d.RequestSourceSystem)
                    .WithMany(p => p.GriResearchStudies)
                    .HasForeignKey(d => d.RequestSourceSystemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_griResearchStudy_sourceSystem");
            });

            modelBuilder.Entity<PersonEntity>(entity =>
            {
                entity.ToTable("person");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created).HasColumnName("created");
            });

            modelBuilder.Entity<PersonNameEntity>(entity =>
            {
                entity.ToTable("personName");

                entity.HasIndex(e => e.PersonId, "fk_personName_person_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.Family)
                    .HasMaxLength(100)
                    .HasColumnName("family");

                entity.Property(e => e.Given)
                    .HasMaxLength(100)
                    .HasColumnName("given");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .HasColumnName("email");

                entity.Property(e => e.PersonId).HasColumnName("person_id");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.PersonNames)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_personName_person");
            });

            modelBuilder.Entity<RoleTypeEntity>(entity =>
            {
                entity.ToTable("roleType");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasColumnName("description");

                entity.Property(e => e.Code)
                    .HasMaxLength(150)
                    .HasColumnName("code");

                entity.HasData(
                    new RoleTypeEntity {
                        Id = 1,
                        Description = "Chief Investigator",
                        Code = "CHF_INV@2.16.840.1.113883.2.1.3.8.5.2.3.5",
                    },
                    new RoleTypeEntity
                    {
                        Id = 2,
                        Description = "Study Coordinator",
                        Code = "STDY_CRDNTR@2.16.840.1.113883.2.1.3.8.5.2.3.5",
                    },
                    new RoleTypeEntity
                    {
                        Id = 3,
                        Description = "Research Activity Coordinator",
                        Code = "RSRCH_ACT_CRDNTR@2.16.840.1.113883.2.1.3.8.5.2.3.5",
                    },
                    new RoleTypeEntity
                    {
                        Id = 4,
                        Description = "Principal Investigator",
                        Code = "PRNCPL_INV@2.16.840.1.113883.2.1.3.8.5.2.3.5",
                    },
                    new RoleTypeEntity
                    {
                        Id = 5,
                        Description = "Company Representative",
                        Code = "CMPNY_RP@2.16.840.1.113883.2.1.3.8.5.2.3.5",
                    }
                    );
            });

            modelBuilder.Entity<ResearchStudyIdentifierTypeEntity>(entity =>
            {
                entity.ToTable("researchStudyIdentifierType");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(45)
                    .HasColumnName("description");

                entity.HasData(
                    new ResearchStudyIdentifierTypeEntity { Id = 1, Description = Domain.EnumsAndConstants.ResearchInitiativeIdentifierTypes.Project },
                    new ResearchStudyIdentifierTypeEntity { Id = 2, Description = Domain.EnumsAndConstants.ResearchInitiativeIdentifierTypes.Protocol },
                    new ResearchStudyIdentifierTypeEntity { Id = 3, Description = Domain.EnumsAndConstants.ResearchInitiativeIdentifierTypes.Bundle },
                    new ResearchStudyIdentifierTypeEntity { Id = 4, Description = Domain.EnumsAndConstants.ResearchInitiativeIdentifierTypes.GrisId }
                    );
            });

            modelBuilder.Entity<OrganisationEntity>(entity =>
            {
                entity.ToTable("organisation");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasColumnName("description");

                entity.Property(e => e.Code)
                    .HasMaxLength(150)
                    .HasColumnName("code");

                entity.HasData(
                    new OrganisationEntity { Id = 1, Description = "Development organisation", Code = "org01"}
                    );
            });

            modelBuilder.Entity<ResearchStudyTeamMember>(entity =>
            {
                entity.ToTable("practitionerRole");

                entity.HasIndex(e => e.GriMappingId, "fk_researchStudyTeamMember_griResearch_idx");

                entity.HasIndex(e => e.RoleTypeId, "fk_researchStudyTeamMember_personRol_idx");

                entity.HasIndex(e => e.PractitionerId, "researchStudyTeamMember_researcher_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GriMappingId).HasColumnName("researchStudy_id");

                entity.Property(e => e.RoleTypeId).HasColumnName("roleType_id");

                entity.Property(e => e.PractitionerId).HasColumnName("practitioner_id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.EffectiveFrom).HasColumnName("effective_from").IsRequired();

                entity.Property(e => e.EffectiveTo).HasColumnName("effective_to").IsRequired(false);

                entity.HasOne(d => d.GriMapping)
                    .WithMany(p => p.ResearchStudyTeamMembers)
                    .HasForeignKey(d => d.GriMappingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_researchStudyTeamMember_griResearch");

                entity.HasOne(d => d.PersonRole)
                    .WithMany(p => p.ResearchStudyTeamMembers)
                    .HasForeignKey(d => d.RoleTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_researchStudyTeamMember_personRol");

                entity.HasOne(d => d.Researcher)
                    .WithMany(p => p.ResearchStudyTeamMembers)
                    .HasForeignKey(d => d.PractitionerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("researchStudyTeamMember_researcher");
            });

            modelBuilder.Entity<Researcher>(entity =>
            {
                entity.ToTable("practitioner");

                entity.HasIndex(e => e.PersonId, "fk_researcher_person_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PersonId).HasColumnName("person_id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Researchers)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_researcher_person");
            });

            modelBuilder.Entity<SourceSystem>(entity =>
            {
                entity.ToTable("sourceSystem");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.Code)
                    .HasMaxLength(45)
                    .HasColumnName("code");

                entity.Property(e => e.Description)
                    .HasMaxLength(45)
                    .HasColumnName("description");

                entity.HasData(
                    new SourceSystem { Id = 1, Code = SourceSystemNames.Edge, Description = "Edge system" },
                    new SourceSystem { Id = 2, Code = SourceSystemNames.Iras, Description = "IRAS system" }
                    );
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
