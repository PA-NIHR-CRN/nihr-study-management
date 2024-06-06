﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NIHR.StudyManagement.Infrastructure.Repository;

#nullable disable

namespace NIHR.StudyManagement.Infrastructure.Migrations
{
    [DbContext(typeof(StudyRegistryContext))]
    partial class StudyRegistryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.GriMapping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created");

                    b.Property<int>("GriResearchStudyId")
                        .HasColumnType("int")
                        .HasColumnName("griResearchStudy_id");

                    b.Property<int>("IdentifierTypeId")
                        .HasColumnType("int");

                    b.Property<int>("SourceSystemId")
                        .HasColumnType("int")
                        .HasColumnName("sourceSystem_id");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.HasIndex("IdentifierTypeId");

                    b.HasIndex(new[] { "GriResearchStudyId" }, "fk_griMapping_griResearchStudy_idx");

                    b.HasIndex(new[] { "SourceSystemId" }, "fk_griMapping_sourceSystem_idx");

                    b.ToTable("researchStudyIdentifier", (string)null);
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.GriResearchStudy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created");

                    b.Property<string>("Gri")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("gri");

                    b.Property<int>("RequestSourceSystemId")
                        .HasColumnType("int")
                        .HasColumnName("requestSourceSystem_id");

                    b.Property<string>("ShortTitle")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)")
                        .HasColumnName("shortTitle");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "RequestSourceSystemId" }, "fk_griResearchStudy_sourceSystem_idx");

                    b.ToTable("researchStudy", (string)null);
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.GriResearchStudyStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("code");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created");

                    b.Property<DateTime>("FromDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ResearchStudyIdentifierId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ToDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("ResearchStudyIdentifierId");

                    b.ToTable("researchStudyIdentifierStatus", (string)null);
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created");

                    b.Property<int>("PersonTypeId")
                        .HasColumnType("int")
                        .HasColumnName("personType_id");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "PersonTypeId" }, "fk_person_type_idx");

                    b.ToTable("person", (string)null);
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.PersonName", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)")
                        .HasColumnName("email");

                    b.Property<string>("Family")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("family");

                    b.Property<string>("Given")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("given");

                    b.Property<int>("PersonId")
                        .HasColumnType("int")
                        .HasColumnName("person_id");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "PersonId" }, "fk_personName_person_idx");

                    b.ToTable("personName", (string)null);
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.PersonRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)")
                        .HasColumnName("code");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)")
                        .HasColumnName("description");

                    b.HasKey("Id");

                    b.ToTable("roleType", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = "CHF_INV@2.16.840.1.113883.2.1.3.8.5.2.3.5",
                            Created = new DateTime(2024, 6, 6, 10, 19, 20, 316, DateTimeKind.Local).AddTicks(7616),
                            Description = "Chief Investigator"
                        },
                        new
                        {
                            Id = 2,
                            Code = "STDY_CRDNTR@2.16.840.1.113883.2.1.3.8.5.2.3.5",
                            Created = new DateTime(2024, 6, 6, 10, 19, 20, 316, DateTimeKind.Local).AddTicks(7667),
                            Description = "Study Coordinator"
                        },
                        new
                        {
                            Id = 3,
                            Code = "RSRCH_ACT_CRDNTR@2.16.840.1.113883.2.1.3.8.5.2.3.5",
                            Created = new DateTime(2024, 6, 6, 10, 19, 20, 316, DateTimeKind.Local).AddTicks(7671),
                            Description = "Research Activity Coordinator"
                        },
                        new
                        {
                            Id = 4,
                            Code = "PRNCPL_INV@2.16.840.1.113883.2.1.3.8.5.2.3.5",
                            Created = new DateTime(2024, 6, 6, 10, 19, 20, 316, DateTimeKind.Local).AddTicks(7673),
                            Description = "Principal Investigator"
                        },
                        new
                        {
                            Id = 5,
                            Code = "CMPNY_RP@2.16.840.1.113883.2.1.3.8.5.2.3.5",
                            Created = new DateTime(2024, 6, 6, 10, 19, 20, 316, DateTimeKind.Local).AddTicks(7676),
                            Description = "Company Representative"
                        });
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.Researcher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created");

                    b.Property<int>("PersonId")
                        .HasColumnType("int")
                        .HasColumnName("person_id");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "PersonId" }, "fk_researcher_person_idx");

                    b.ToTable("practitioner", (string)null);
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.ResearchInitiativeIdentifierType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("description");

                    b.HasKey("Id");

                    b.ToTable("researchStudyIdentifierType", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Created = new DateTime(2024, 6, 6, 10, 19, 20, 316, DateTimeKind.Local).AddTicks(8604),
                            Description = "PROJECT"
                        },
                        new
                        {
                            Id = 2,
                            Created = new DateTime(2024, 6, 6, 10, 19, 20, 316, DateTimeKind.Local).AddTicks(8621),
                            Description = "PROTOCOL"
                        },
                        new
                        {
                            Id = 3,
                            Created = new DateTime(2024, 6, 6, 10, 19, 20, 316, DateTimeKind.Local).AddTicks(8624),
                            Description = "BUNDLE"
                        });
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.ResearchStudyTeamMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created");

                    b.Property<DateTime>("EffectiveFrom")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("effective_from");

                    b.Property<DateTime?>("EffectiveTo")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("effective_to");

                    b.Property<int>("GriMappingId")
                        .HasColumnType("int")
                        .HasColumnName("researchStudy_id");

                    b.Property<int>("PractitionerId")
                        .HasColumnType("int")
                        .HasColumnName("practitioner_id");

                    b.Property<int>("RoleTypeId")
                        .HasColumnType("int")
                        .HasColumnName("roleType_id");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "GriMappingId" }, "fk_researchStudyTeamMember_griResearch_idx");

                    b.HasIndex(new[] { "RoleTypeId" }, "fk_researchStudyTeamMember_personRol_idx");

                    b.HasIndex(new[] { "PractitionerId" }, "researchStudyTeamMember_researcher_idx");

                    b.ToTable("practitionerRole", (string)null);
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.SourceSystem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("code");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("description");

                    b.HasKey("Id");

                    b.ToTable("sourceSystem", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = "EDGE",
                            Created = new DateTime(2024, 6, 6, 10, 19, 20, 320, DateTimeKind.Local).AddTicks(1457),
                            Description = "Edge system"
                        },
                        new
                        {
                            Id = 2,
                            Code = "IRAS",
                            Created = new DateTime(2024, 6, 6, 10, 19, 20, 320, DateTimeKind.Local).AddTicks(1507),
                            Description = "IRAS system"
                        });
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.GriMapping", b =>
                {
                    b.HasOne("NIHR.StudyManagement.Infrastructure.Repository.Models.GriResearchStudy", "GriResearchStudy")
                        .WithMany("ResearchStudyIdentifiers")
                        .HasForeignKey("GriResearchStudyId")
                        .IsRequired()
                        .HasConstraintName("fk_griMapping_griResearchStudy");

                    b.HasOne("NIHR.StudyManagement.Infrastructure.Repository.Models.ResearchInitiativeIdentifierType", "IdentifierType")
                        .WithMany("Identifiers")
                        .HasForeignKey("IdentifierTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NIHR.StudyManagement.Infrastructure.Repository.Models.SourceSystem", "SourceSystem")
                        .WithMany("GriMappings")
                        .HasForeignKey("SourceSystemId")
                        .IsRequired()
                        .HasConstraintName("fk_griMapping_sourceSystem");

                    b.Navigation("GriResearchStudy");

                    b.Navigation("IdentifierType");

                    b.Navigation("SourceSystem");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.GriResearchStudy", b =>
                {
                    b.HasOne("NIHR.StudyManagement.Infrastructure.Repository.Models.SourceSystem", "RequestSourceSystem")
                        .WithMany("GriResearchStudies")
                        .HasForeignKey("RequestSourceSystemId")
                        .IsRequired()
                        .HasConstraintName("fk_griResearchStudy_sourceSystem");

                    b.Navigation("RequestSourceSystem");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.GriResearchStudyStatus", b =>
                {
                    b.HasOne("NIHR.StudyManagement.Infrastructure.Repository.Models.GriMapping", "ResearchStudyIdentifier")
                        .WithMany("IdentifierStatuses")
                        .HasForeignKey("ResearchStudyIdentifierId")
                        .IsRequired()
                        .HasConstraintName("fk_griResearchStudyStatus_griMapping");

                    b.Navigation("ResearchStudyIdentifier");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.PersonName", b =>
                {
                    b.HasOne("NIHR.StudyManagement.Infrastructure.Repository.Models.Person", "Person")
                        .WithMany("PersonNames")
                        .HasForeignKey("PersonId")
                        .IsRequired()
                        .HasConstraintName("fk_personName_person");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.Researcher", b =>
                {
                    b.HasOne("NIHR.StudyManagement.Infrastructure.Repository.Models.Person", "Person")
                        .WithMany("Researchers")
                        .HasForeignKey("PersonId")
                        .IsRequired()
                        .HasConstraintName("fk_researcher_person");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.ResearchStudyTeamMember", b =>
                {
                    b.HasOne("NIHR.StudyManagement.Infrastructure.Repository.Models.GriResearchStudy", "GriMapping")
                        .WithMany("ResearchStudyTeamMembers")
                        .HasForeignKey("GriMappingId")
                        .IsRequired()
                        .HasConstraintName("fk_researchStudyTeamMember_griResearch");

                    b.HasOne("NIHR.StudyManagement.Infrastructure.Repository.Models.Researcher", "Researcher")
                        .WithMany("ResearchStudyTeamMembers")
                        .HasForeignKey("PractitionerId")
                        .IsRequired()
                        .HasConstraintName("researchStudyTeamMember_researcher");

                    b.HasOne("NIHR.StudyManagement.Infrastructure.Repository.Models.PersonRole", "PersonRole")
                        .WithMany("ResearchStudyTeamMembers")
                        .HasForeignKey("RoleTypeId")
                        .IsRequired()
                        .HasConstraintName("fk_researchStudyTeamMember_personRol");

                    b.Navigation("GriMapping");

                    b.Navigation("PersonRole");

                    b.Navigation("Researcher");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.GriMapping", b =>
                {
                    b.Navigation("IdentifierStatuses");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.GriResearchStudy", b =>
                {
                    b.Navigation("ResearchStudyIdentifiers");

                    b.Navigation("ResearchStudyTeamMembers");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.Person", b =>
                {
                    b.Navigation("PersonNames");

                    b.Navigation("Researchers");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.PersonRole", b =>
                {
                    b.Navigation("ResearchStudyTeamMembers");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.Researcher", b =>
                {
                    b.Navigation("ResearchStudyTeamMembers");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.ResearchInitiativeIdentifierType", b =>
                {
                    b.Navigation("Identifiers");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.SourceSystem", b =>
                {
                    b.Navigation("GriMappings");

                    b.Navigation("GriResearchStudies");
                });
#pragma warning restore 612, 618
        }
    }
}
