﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NIHR.StudyManagement.Infrastructure.Repository;

#nullable disable

namespace NIHR.StudyManagement.Infrastructure.Migrations
{
    [DbContext(typeof(StudyRegistryContext))]
    [Migration("20240607091645_01-Initial")]
    partial class _01Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.OrganisationEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("code");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("description");

                    b.HasKey("Id");

                    b.ToTable("organisation", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = "org01",
                            Created = new DateTime(2024, 6, 7, 10, 16, 44, 744, DateTimeKind.Local).AddTicks(4852),
                            Description = "Development organisation"
                        });
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.PersonEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created");

                    b.HasKey("Id");

                    b.ToTable("person", (string)null);
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.PersonNameEntity", b =>
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
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("email");

                    b.Property<string>("Family")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("family");

                    b.Property<string>("Given")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("given");

                    b.Property<int>("PersonId")
                        .HasColumnType("int")
                        .HasColumnName("person_id");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "PersonId" }, "idx_personName_id");

                    b.ToTable("personName", (string)null);
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.PractitionerEntity", b =>
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

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.ResearchStudyEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created");

                    b.Property<int>("RequestSourceSystemId")
                        .HasColumnType("int")
                        .HasColumnName("sourceSystem_id");

                    b.Property<string>("ShortTitle")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("shortTitle");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "RequestSourceSystemId" }, "idx_researchStudy_sourceSystem");

                    b.ToTable("researchStudy", (string)null);
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.ResearchStudyIdentifierEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created");

                    b.Property<int>("IdentifierTypeId")
                        .HasColumnType("int");

                    b.Property<int>("ResearchStudyId")
                        .HasColumnType("int")
                        .HasColumnName("researchStudy_id");

                    b.Property<int>("SourceSystemId")
                        .HasColumnType("int")
                        .HasColumnName("sourceSystem_id");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.HasIndex("IdentifierTypeId");

                    b.HasIndex(new[] { "ResearchStudyId" }, "idx_researchStudyIdentifier_researchStudyId");

                    b.HasIndex(new[] { "SourceSystemId" }, "idx_researchStudyIdentifier_sourceSystemId");

                    b.ToTable("researchStudyIdentifier", (string)null);
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.ResearchStudyIdentifierStatusEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)")
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

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.ResearchStudyIdentifierTypeEntity", b =>
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
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("description");

                    b.HasKey("Id");

                    b.ToTable("researchStudyIdentifierType", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Created = new DateTime(2024, 6, 7, 10, 16, 44, 744, DateTimeKind.Local).AddTicks(4215),
                            Description = "PROJECT"
                        },
                        new
                        {
                            Id = 2,
                            Created = new DateTime(2024, 6, 7, 10, 16, 44, 744, DateTimeKind.Local).AddTicks(4234),
                            Description = "PROTOCOL"
                        },
                        new
                        {
                            Id = 3,
                            Created = new DateTime(2024, 6, 7, 10, 16, 44, 744, DateTimeKind.Local).AddTicks(4237),
                            Description = "BUNDLE"
                        },
                        new
                        {
                            Id = 4,
                            Created = new DateTime(2024, 6, 7, 10, 16, 44, 744, DateTimeKind.Local).AddTicks(4240),
                            Description = "GRIS ID"
                        });
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.ResearchStudyTeamMemberEntity", b =>
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

                    b.Property<int?>("OrganizationId")
                        .HasColumnType("int")
                        .HasColumnName("organization_id");

                    b.Property<int>("PractitionerId")
                        .HasColumnType("int")
                        .HasColumnName("practitioner_id");

                    b.Property<int>("ResearchStudyId")
                        .HasColumnType("int")
                        .HasColumnName("researchStudy_id");

                    b.Property<int>("RoleTypeId")
                        .HasColumnType("int")
                        .HasColumnName("roleType_id");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.HasIndex(new[] { "PractitionerId" }, "idx_researchStudyTeamMember_PractitionerId");

                    b.HasIndex(new[] { "ResearchStudyId" }, "idx_researchStudyTeamMember_researchStudyId");

                    b.HasIndex(new[] { "RoleTypeId" }, "idx_researchStudyTeamMember_roleTypeId");

                    b.ToTable("practitionerRole", (string)null);
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.RoleTypeEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("code");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created");

                    b.Property<string>("Description")
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("description");

                    b.HasKey("Id");

                    b.ToTable("roleType", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = "CHF_INV@2.16.840.1.113883.2.1.3.8.5.2.3.5",
                            Created = new DateTime(2024, 6, 7, 10, 16, 44, 744, DateTimeKind.Local).AddTicks(3023),
                            Description = "Chief Investigator"
                        },
                        new
                        {
                            Id = 2,
                            Code = "STDY_CRDNTR@2.16.840.1.113883.2.1.3.8.5.2.3.5",
                            Created = new DateTime(2024, 6, 7, 10, 16, 44, 744, DateTimeKind.Local).AddTicks(3083),
                            Description = "Study Coordinator"
                        },
                        new
                        {
                            Id = 3,
                            Code = "RSRCH_ACT_CRDNTR@2.16.840.1.113883.2.1.3.8.5.2.3.5",
                            Created = new DateTime(2024, 6, 7, 10, 16, 44, 744, DateTimeKind.Local).AddTicks(3087),
                            Description = "Research Activity Coordinator"
                        },
                        new
                        {
                            Id = 4,
                            Code = "PRNCPL_INV@2.16.840.1.113883.2.1.3.8.5.2.3.5",
                            Created = new DateTime(2024, 6, 7, 10, 16, 44, 744, DateTimeKind.Local).AddTicks(3090),
                            Description = "Principal Investigator"
                        },
                        new
                        {
                            Id = 5,
                            Code = "CMPNY_RP@2.16.840.1.113883.2.1.3.8.5.2.3.5",
                            Created = new DateTime(2024, 6, 7, 10, 16, 44, 744, DateTimeKind.Local).AddTicks(3093),
                            Description = "Company Representative"
                        });
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.SourceSystemEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("code");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("description");

                    b.HasKey("Id");

                    b.ToTable("sourceSystem", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = "EDGE",
                            Created = new DateTime(2024, 6, 7, 10, 16, 44, 745, DateTimeKind.Local).AddTicks(8636),
                            Description = "Edge system"
                        },
                        new
                        {
                            Id = 2,
                            Code = "IRAS",
                            Created = new DateTime(2024, 6, 7, 10, 16, 44, 745, DateTimeKind.Local).AddTicks(8669),
                            Description = "IRAS system"
                        });
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.PersonNameEntity", b =>
                {
                    b.HasOne("NIHR.StudyManagement.Infrastructure.Repository.Models.PersonEntity", "Person")
                        .WithMany("PersonNames")
                        .HasForeignKey("PersonId")
                        .IsRequired()
                        .HasConstraintName("fk_personName_person");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.PractitionerEntity", b =>
                {
                    b.HasOne("NIHR.StudyManagement.Infrastructure.Repository.Models.PersonEntity", "Person")
                        .WithMany("Researchers")
                        .HasForeignKey("PersonId")
                        .IsRequired()
                        .HasConstraintName("fk_researcher_person");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.ResearchStudyEntity", b =>
                {
                    b.HasOne("NIHR.StudyManagement.Infrastructure.Repository.Models.SourceSystemEntity", "RequestSourceSystem")
                        .WithMany("ResearchStudies")
                        .HasForeignKey("RequestSourceSystemId")
                        .IsRequired()
                        .HasConstraintName("fk_researchStudy_sourceSystem");

                    b.Navigation("RequestSourceSystem");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.ResearchStudyIdentifierEntity", b =>
                {
                    b.HasOne("NIHR.StudyManagement.Infrastructure.Repository.Models.ResearchStudyIdentifierTypeEntity", "IdentifierType")
                        .WithMany("Identifiers")
                        .HasForeignKey("IdentifierTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NIHR.StudyManagement.Infrastructure.Repository.Models.ResearchStudyEntity", "ResearchStudy")
                        .WithMany("ResearchStudyIdentifiers")
                        .HasForeignKey("ResearchStudyId")
                        .IsRequired()
                        .HasConstraintName("fk_researchStudyIdentifier_researchStudy");

                    b.HasOne("NIHR.StudyManagement.Infrastructure.Repository.Models.SourceSystemEntity", "SourceSystem")
                        .WithMany("ResearchStudyIdentifiers")
                        .HasForeignKey("SourceSystemId")
                        .IsRequired()
                        .HasConstraintName("fk_researchStudyIdentifier_sourceSystem");

                    b.Navigation("IdentifierType");

                    b.Navigation("ResearchStudy");

                    b.Navigation("SourceSystem");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.ResearchStudyIdentifierStatusEntity", b =>
                {
                    b.HasOne("NIHR.StudyManagement.Infrastructure.Repository.Models.ResearchStudyIdentifierEntity", "ResearchStudyIdentifier")
                        .WithMany("IdentifierStatuses")
                        .HasForeignKey("ResearchStudyIdentifierId")
                        .IsRequired()
                        .HasConstraintName("fk_researchStudyIdentifierStatus_researchStudy");

                    b.Navigation("ResearchStudyIdentifier");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.ResearchStudyTeamMemberEntity", b =>
                {
                    b.HasOne("NIHR.StudyManagement.Infrastructure.Repository.Models.OrganisationEntity", "Organitation")
                        .WithMany("ResearchStudyTeamMembers")
                        .HasForeignKey("OrganizationId")
                        .HasConstraintName("fk_researchStudyTeamMember_organization");

                    b.HasOne("NIHR.StudyManagement.Infrastructure.Repository.Models.PractitionerEntity", "Practitioner")
                        .WithMany("ResearchStudyTeamMembers")
                        .HasForeignKey("PractitionerId")
                        .IsRequired()
                        .HasConstraintName("fk_researchStudyTeamMember_practitioner");

                    b.HasOne("NIHR.StudyManagement.Infrastructure.Repository.Models.ResearchStudyEntity", "ResearchStudy")
                        .WithMany("ResearchStudyTeamMembers")
                        .HasForeignKey("ResearchStudyId")
                        .IsRequired()
                        .HasConstraintName("fk_researchStudyTeamMember_researchStudy");

                    b.HasOne("NIHR.StudyManagement.Infrastructure.Repository.Models.RoleTypeEntity", "PersonRole")
                        .WithMany("ResearchStudyTeamMembers")
                        .HasForeignKey("RoleTypeId")
                        .IsRequired()
                        .HasConstraintName("fk_researchStudyTeamMember_personRole");

                    b.Navigation("Organitation");

                    b.Navigation("PersonRole");

                    b.Navigation("Practitioner");

                    b.Navigation("ResearchStudy");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.OrganisationEntity", b =>
                {
                    b.Navigation("ResearchStudyTeamMembers");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.PersonEntity", b =>
                {
                    b.Navigation("PersonNames");

                    b.Navigation("Researchers");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.PractitionerEntity", b =>
                {
                    b.Navigation("ResearchStudyTeamMembers");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.ResearchStudyEntity", b =>
                {
                    b.Navigation("ResearchStudyIdentifiers");

                    b.Navigation("ResearchStudyTeamMembers");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.ResearchStudyIdentifierEntity", b =>
                {
                    b.Navigation("IdentifierStatuses");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.ResearchStudyIdentifierTypeEntity", b =>
                {
                    b.Navigation("Identifiers");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.RoleTypeEntity", b =>
                {
                    b.Navigation("ResearchStudyTeamMembers");
                });

            modelBuilder.Entity("NIHR.StudyManagement.Infrastructure.Repository.Models.SourceSystemEntity", b =>
                {
                    b.Navigation("ResearchStudies");

                    b.Navigation("ResearchStudyIdentifiers");
                });
#pragma warning restore 612, 618
        }
    }
}
