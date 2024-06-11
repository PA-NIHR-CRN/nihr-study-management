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

        public virtual DbSet<ResearchStudyEntity> ResearchStudies { get; set; } = null!;

        public virtual DbSet<StudyRecordOutboxEntity> StudyRecordOutboxEntries { get; set; } = null!;

        public virtual DbSet<PersonNameEntity> PersonNames { get; set; } = null!;
        public virtual DbSet<RoleTypeEntity> PersonRoles { get; set; } = null!;

        public virtual DbSet<OrganisationEntity> OrganisationEntities { get; set; } = null!;

        public virtual DbSet<ResearchStudyIdentifierTypeEntity> ResearchInitiativeIdentifierTypes { get; set; } = null!;

        public virtual DbSet<ResearchStudyTeamMemberEntity> ResearchStudyTeamMembers { get; set; } = null!;

        public virtual DbSet<ResearchStudyIdentifierEntity> ResearchStudyIdentifiers { get; set; } = null!;

        public virtual DbSet<SourceSystemEntity> SourceSystems { get; set; } = null!;

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

                entity.HasIndex(e => e.ResearchStudyId, "idx_researchStudyIdentifier_researchStudyId");

                entity.Property(e => e.Value)
                    .HasMaxLength(250)
                    .HasColumnName("value");

                entity.HasIndex(e => e.SourceSystemId, "idx_researchStudyIdentifier_sourceSystemId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ResearchStudyId).HasColumnName("researchStudy_id");

                entity.Property(e => e.SourceSystemId).HasColumnName("sourceSystem_id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.HasOne(d => d.ResearchStudy)
                    .WithMany(p => p.ResearchStudyIdentifiers)
                    .HasForeignKey(d => d.ResearchStudyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_researchStudyIdentifier_researchStudy");

                entity.HasOne(d => d.SourceSystem)
                    .WithMany(p => p.ResearchStudyIdentifiers)
                    .HasForeignKey(d => d.SourceSystemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_researchStudyIdentifier_sourceSystem");
            });

            modelBuilder.Entity<ResearchStudyIdentifierStatusEntity>(entity =>
            {
                entity.ToTable("researchStudyIdentifierStatus");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.Code)
                    .HasMaxLength(250)
                    .HasColumnName("code");

                entity.HasOne(d => d.ResearchStudyIdentifier)
                    .WithMany(p => p.IdentifierStatuses)
                    .HasForeignKey(d => d.ResearchStudyIdentifierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_researchStudyIdentifierStatus_researchStudy");
            });

            modelBuilder.Entity<ResearchStudyEntity>(entity =>
            {
                entity.ToTable("researchStudy");

                entity.HasIndex(e => e.RequestSourceSystemId, "idx_researchStudy_sourceSystem");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.RequestSourceSystemId).HasColumnName("sourceSystem_id");

                entity.Property(e => e.ShortTitle)
                    .HasMaxLength(250)
                    .HasColumnName("shortTitle");

                entity.HasOne(d => d.RequestSourceSystem)
                    .WithMany(p => p.ResearchStudies)
                    .HasForeignKey(d => d.RequestSourceSystemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_researchStudy_sourceSystem");
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

                entity.HasIndex(e => e.PersonId, "idx_personName_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.Family)
                    .HasMaxLength(250)
                    .HasColumnName("family");

                entity.Property(e => e.Given)
                    .HasMaxLength(250)
                    .HasColumnName("given");

                entity.Property(e => e.Email)
                    .HasMaxLength(250)
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
                    .HasMaxLength(250)
                    .HasColumnName("description");

                entity.Property(e => e.Code)
                    .HasMaxLength(250)
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
                    .HasMaxLength(250)
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
                    .HasMaxLength(250)
                    .HasColumnName("description");

                entity.Property(e => e.Code)
                    .HasMaxLength(250)
                    .HasColumnName("code");

                entity.HasData(
                    new OrganisationEntity { Id = 1, Description = "Development organisation", Code = "org01"}
                    );
            });

            modelBuilder.Entity<ResearchStudyTeamMemberEntity>(entity =>
            {
                entity.ToTable("practitionerRole");

                entity.HasIndex(e => e.ResearchStudyId, "idx_researchStudyTeamMember_researchStudyId");

                entity.HasIndex(e => e.RoleTypeId, "idx_researchStudyTeamMember_roleTypeId");

                entity.HasIndex(e => e.PractitionerId, "idx_researchStudyTeamMember_PractitionerId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ResearchStudyId).HasColumnName("researchStudy_id");

                entity.Property(e => e.RoleTypeId).HasColumnName("roleType_id");

                entity.Property(e => e.PractitionerId).HasColumnName("practitioner_id");

                entity.Property(e => e.OrganizationId).HasColumnName("organization_id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.EffectiveFrom).HasColumnName("effective_from").IsRequired();

                entity.Property(e => e.EffectiveTo).HasColumnName("effective_to").IsRequired(false);

                entity.HasOne(d => d.ResearchStudy)
                    .WithMany(p => p.ResearchStudyTeamMembers)
                    .HasForeignKey(d => d.ResearchStudyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_researchStudyTeamMember_researchStudy");

                entity.HasOne(d => d.PersonRole)
                    .WithMany(p => p.ResearchStudyTeamMembers)
                    .HasForeignKey(d => d.RoleTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_researchStudyTeamMember_personRole");

                entity.HasOne(d => d.Practitioner)
                    .WithMany(p => p.ResearchStudyTeamMembers)
                    .HasForeignKey(d => d.PractitionerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_researchStudyTeamMember_practitioner");

                entity.HasOne(d => d.Organitation)
                    .WithMany(p => p.ResearchStudyTeamMembers)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_researchStudyTeamMember_organization");
            });

            modelBuilder.Entity<PractitionerEntity>(entity =>
            {
                entity.ToTable("practitioner");

                entity.HasIndex(e => e.PersonId, "fk_researcher_person_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PersonId).HasColumnName("person_id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Practitioners)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_researcher_person");
            });

            modelBuilder.Entity<SourceSystemEntity>(entity =>
            {
                entity.ToTable("sourceSystem");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.Code)
                    .HasMaxLength(250)
                    .HasColumnName("code");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .HasColumnName("description");

                entity.HasData(
                    new SourceSystemEntity { Id = 1, Code = SourceSystemNames.Edge, Description = "Edge system" },
                    new SourceSystemEntity { Id = 2, Code = SourceSystemNames.Iras, Description = "IRAS system" }
                    );
            });

            modelBuilder.Entity<StudyRecordOutboxEntity>(entity =>
            {
                entity.ToTable("studyRecordOutboxEntry");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.EventType).HasColumnName("eventtype");

                entity.Property(e => e.SourceSystem).HasColumnName("sourcesystem");

                entity.Property(e => e.ProcessingStartDate).HasColumnName("processingStartDate").IsRequired(false);

                entity.Property(e => e.ProcessingCompletedDate).HasColumnName("processingCompletedDate").IsRequired(false);

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Payload)
                    .HasColumnType("json")
                    .HasColumnName("payload");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
