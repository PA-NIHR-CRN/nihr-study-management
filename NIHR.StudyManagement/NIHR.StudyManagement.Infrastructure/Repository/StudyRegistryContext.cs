using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NIHR.StudyManagement.Infrastructure.Repository.EnumsAndConstants;
using NIHR.StudyManagement.Infrastructure.Repository.Models;

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

        public virtual DbSet<GriMapping> GriMappings { get; set; } = null!;
        public virtual DbSet<GriResearchStudy> GriResearchStudies { get; set; } = null!;
        public virtual DbSet<Person> People { get; set; } = null!;
        public virtual DbSet<PersonName> PersonNames { get; set; } = null!;
        public virtual DbSet<PersonRole> PersonRoles { get; set; } = null!;
        public virtual DbSet<PersonType> PersonTypes { get; set; } = null!;
        public virtual DbSet<ResearchInitiative> ResearchInitiatives { get; set; } = null!;
        public virtual DbSet<ResearchInitiativeIdentifier> ResearchInitiativeIdentifiers { get; set; } = null!;
        public virtual DbSet<ResearchInitiativeIdentifierType> ResearchInitiativeIdentifierTypes { get; set; } = null!;
        public virtual DbSet<ResearchInitiativeType> ResearchInitiativeTypes { get; set; } = null!;
        public virtual DbSet<ResearchStudyTeamMember> ResearchStudyTeamMembers { get; set; } = null!;
        public virtual DbSet<Researcher> Researchers { get; set; } = null!;
        public virtual DbSet<SourceSystem> SourceSystems { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GriMapping>(entity =>
            {
                entity.ToTable("griMapping");

                entity.HasIndex(e => e.GriResearchStudyId, "fk_griMapping_griResearchStudy_idx");

                entity.HasIndex(e => e.ResearchInitiativeIdentifierId, "fk_griMapping_researchInitiativeIdentifier_idx");

                entity.HasIndex(e => e.SourceSystemId, "fk_griMapping_sourceSystem_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GriResearchStudyId).HasColumnName("griResearchStudy_id");

                entity.Property(e => e.ResearchInitiativeIdentifierId).HasColumnName("researchInitiativeIdentifier_id");

                entity.Property(e => e.SourceSystemId).HasColumnName("sourceSystem_id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.HasOne(d => d.GriResearchStudy)
                    .WithMany(p => p.GriMappings)
                    .HasForeignKey(d => d.GriResearchStudyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_griMapping_griResearchStudy");

                entity.HasOne(d => d.ResearchInitiativeIdentifier)
                    .WithMany(p => p.GriMappings)
                    .HasForeignKey(d => d.ResearchInitiativeIdentifierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_griMapping_researchInitiativeIdentifier");

                entity.HasOne(d => d.SourceSystem)
                    .WithMany(p => p.GriMappings)
                    .HasForeignKey(d => d.SourceSystemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_griMapping_sourceSystem");
            });

            modelBuilder.Entity<GriResearchStudy>(entity =>
            {
                entity.ToTable("griResearchStudy");

                entity.HasIndex(e => e.ResearchInitiativeId, "fk_griResearchStudy_researchInitiative_idx");

                entity.HasIndex(e => e.RequestSourceSystemId, "fk_griResearchStudy_sourceSystem_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.Gri)
                    .HasMaxLength(100)
                    .HasColumnName("gri");

                entity.Property(e => e.RequestSourceSystemId).HasColumnName("requestSourceSystem_id");

                entity.Property(e => e.ResearchInitiativeId).HasColumnName("researchInitiative_id");

                entity.Property(e => e.ShortTitle)
                    .HasMaxLength(150)
                    .HasColumnName("shortTitle");

                entity.HasOne(d => d.RequestSourceSystem)
                    .WithMany(p => p.GriResearchStudies)
                    .HasForeignKey(d => d.RequestSourceSystemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_griResearchStudy_sourceSystem");

                entity.HasOne(d => d.ResearchInitiative)
                    .WithMany(p => p.GriResearchStudies)
                    .HasForeignKey(d => d.ResearchInitiativeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_griResearchStudy_researchInitiative");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("person");

                entity.HasIndex(e => e.PersonTypeId, "fk_person_type_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PersonTypeId).HasColumnName("personType_id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.HasOne(d => d.PersonType)
                    .WithMany(p => p.People)
                    .HasForeignKey(d => d.PersonTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_person_type");
            });

            modelBuilder.Entity<PersonName>(entity =>
            {
                entity.ToTable("personName");

                entity.HasIndex(e => e.PersonId, "fk_personName_person_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.Family)
                    .HasMaxLength(100)
                    .HasColumnName("family");

                entity.Property(e => e.Given)
                    .HasMaxLength(10)
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

            modelBuilder.Entity<PersonRole>(entity =>
            {
                entity.ToTable("personRole");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.Description)
                    .HasMaxLength(150)
                    .HasColumnName("description");

                entity.Property(e => e.Type)
                    .HasMaxLength(45)
                    .HasColumnName("type");

                entity.HasData(
                    new PersonRole
                    {
                        Id = 1,
                        Description = "A Chief investigator role",
                        Type = EnumsAndConstants.PersonRoles.ChiefInvestigator,
                    });
            });

            modelBuilder.Entity<PersonType>(entity =>
            {
                entity.ToTable("personType");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.Description)
                    .HasMaxLength(45)
                    .HasColumnName("description");

                entity.HasData(
                    new PersonType
                    {
                        Id = 1,
                        Description = EnumsAndConstants.PersonTypes.Researcher
                    });
            });

            modelBuilder.Entity<ResearchInitiative>(entity =>
            {
                entity.ToTable("researchInitiative");

                entity.HasIndex(e => e.ResearchInitiativeTypeId, "fk_researchInitiative_type_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.ResearchInitiativeTypeId).HasColumnName("researchInitiativeType_id");

                entity.HasOne(d => d.ResearchInitiativeType)
                    .WithMany(p => p.ResearchInitiatives)
                    .HasForeignKey(d => d.ResearchInitiativeTypeId)
                    .HasConstraintName("fk_researchInitiative_type");
            });

            modelBuilder.Entity<ResearchInitiativeIdentifier>(entity =>
            {
                entity.HasKey(e => e.Int)
                    .HasName("PRIMARY");

                entity.ToTable("researchInitiativeIdentifier");

                entity.HasIndex(e => e.SourceSystemId, "fk_researchInitiativeIdentifier_sourceSystem_idx");

                entity.HasIndex(e => e.ResearchInitiativeIdentifierTypeId, "fk_researchInitiativeIdentifier_type_idx");

                entity.Property(e => e.Int).HasColumnName("int");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.ResearchInitiativeIdentifierTypeId).HasColumnName("researchInitiativeIdentifierType_id");

                entity.Property(e => e.SourceSystemId).HasColumnName("sourceSystem_id");

                entity.Property(e => e.Value)
                    .HasMaxLength(150)
                    .HasColumnName("value");

                entity.HasOne(d => d.ResearchInitiativeIdentifierType)
                    .WithMany(p => p.ResearchInitiativeIdentifiers)
                    .HasForeignKey(d => d.ResearchInitiativeIdentifierTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_researchInitiativeIdentifier_type");

                entity.HasOne(d => d.SourceSystem)
                    .WithMany(p => p.ResearchInitiativeIdentifiers)
                    .HasForeignKey(d => d.SourceSystemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_researchInitiativeIdentifier_sourceSystem");
            });

            modelBuilder.Entity<ResearchInitiativeIdentifierType>(entity =>
            {
                entity.ToTable("researchInitiativeIdentifierType");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(45)
                    .HasColumnName("description");

                entity.HasData(
                    new ResearchInitiativeIdentifierType { Id = 1, Description = EnumsAndConstants.ResearchInitiativeIdentifierTypes.Project },
                    new ResearchInitiativeIdentifierType { Id = 2, Description = EnumsAndConstants.ResearchInitiativeIdentifierTypes.Protocol }
                    );
            });

            modelBuilder.Entity<ResearchInitiativeType>(entity =>
            {
                entity.ToTable("researchInitiativeType");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(45)
                    .HasColumnName("description");

                entity.HasData(new ResearchInitiativeType { Id = 1, Description = EnumsAndConstants.ResearchInitiativeTypes.Study });
            });

            modelBuilder.Entity<ResearchStudyTeamMember>(entity =>
            {
                entity.ToTable("researchStudyTeamMember");

                entity.HasIndex(e => e.GriMappingId, "fk_researchStudyTeamMember_griResearch_idx");

                entity.HasIndex(e => e.PersonRoleId, "fk_researchStudyTeamMember_personRol_idx");

                entity.HasIndex(e => e.ResearcherId, "researchStudyTeamMember_researcher_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GriMappingId).HasColumnName("griMapping_id");

                entity.Property(e => e.PersonRoleId).HasColumnName("personRole_id");

                entity.Property(e => e.ResearcherId).HasColumnName("researcher_id");

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
                    .HasForeignKey(d => d.PersonRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_researchStudyTeamMember_personRol");

                entity.HasOne(d => d.Researcher)
                    .WithMany(p => p.ResearchStudyTeamMembers)
                    .HasForeignKey(d => d.ResearcherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("researchStudyTeamMember_researcher");
            });

            modelBuilder.Entity<Researcher>(entity =>
            {
                entity.ToTable("researcher");

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
