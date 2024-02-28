﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace NIHR.StudyManagement.Infrastructure.Migrations
{
    public partial class _01Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "personRole",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    type = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    description = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personRole", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "personType",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personType", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "researchInitiativeIdentifierType",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_researchInitiativeIdentifierType", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "researchInitiativeType",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_researchInitiativeType", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sourceSystem",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    description = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sourceSystem", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "person",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    personType_id = table.Column<int>(type: "int", nullable: false),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person", x => x.id);
                    table.ForeignKey(
                        name: "fk_person_type",
                        column: x => x.personType_id,
                        principalTable: "personType",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "researchInitiative",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    researchInitiativeType_id = table.Column<int>(type: "int", nullable: true),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_researchInitiative", x => x.id);
                    table.ForeignKey(
                        name: "fk_researchInitiative_type",
                        column: x => x.researchInitiativeType_id,
                        principalTable: "researchInitiativeType",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "researchInitiativeIdentifier",
                columns: table => new
                {
                    @int = table.Column<int>(name: "int", type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    sourceSystem_id = table.Column<int>(type: "int", nullable: false),
                    value = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    researchInitiativeIdentifierType_id = table.Column<int>(type: "int", nullable: false),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.@int);
                    table.ForeignKey(
                        name: "fk_researchInitiativeIdentifier_sourceSystem",
                        column: x => x.sourceSystem_id,
                        principalTable: "sourceSystem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_researchInitiativeIdentifier_type",
                        column: x => x.researchInitiativeIdentifierType_id,
                        principalTable: "researchInitiativeIdentifierType",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "personName",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    person_id = table.Column<int>(type: "int", nullable: false),
                    family = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    given = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personName", x => x.id);
                    table.ForeignKey(
                        name: "fk_personName_person",
                        column: x => x.person_id,
                        principalTable: "person",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "researcher",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    person_id = table.Column<int>(type: "int", nullable: false),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_researcher", x => x.id);
                    table.ForeignKey(
                        name: "fk_researcher_person",
                        column: x => x.person_id,
                        principalTable: "person",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "griResearchStudy",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    researchInitiative_id = table.Column<int>(type: "int", nullable: false),
                    gri = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    shortTitle = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    requestSourceSystem_id = table.Column<int>(type: "int", nullable: false),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_griResearchStudy", x => x.id);
                    table.ForeignKey(
                        name: "fk_griResearchStudy_researchInitiative",
                        column: x => x.researchInitiative_id,
                        principalTable: "researchInitiative",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_griResearchStudy_sourceSystem",
                        column: x => x.requestSourceSystem_id,
                        principalTable: "sourceSystem",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "griMapping",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    griResearchStudy_id = table.Column<int>(type: "int", nullable: false),
                    sourceSystem_id = table.Column<int>(type: "int", nullable: false),
                    researchInitiativeIdentifier_id = table.Column<int>(type: "int", nullable: false),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_griMapping", x => x.id);
                    table.ForeignKey(
                        name: "fk_griMapping_griResearchStudy",
                        column: x => x.griResearchStudy_id,
                        principalTable: "griResearchStudy",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_griMapping_researchInitiativeIdentifier",
                        column: x => x.researchInitiativeIdentifier_id,
                        principalTable: "researchInitiativeIdentifier",
                        principalColumn: "int");
                    table.ForeignKey(
                        name: "fk_griMapping_sourceSystem",
                        column: x => x.sourceSystem_id,
                        principalTable: "sourceSystem",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "researchStudyTeamMember",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    griMapping_id = table.Column<int>(type: "int", nullable: false),
                    researcher_id = table.Column<int>(type: "int", nullable: false),
                    personRole_id = table.Column<int>(type: "int", nullable: false),
                    effective_from = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    effective_to = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_researchStudyTeamMember", x => x.id);
                    table.ForeignKey(
                        name: "fk_researchStudyTeamMember_griResearch",
                        column: x => x.griMapping_id,
                        principalTable: "griResearchStudy",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_researchStudyTeamMember_personRol",
                        column: x => x.personRole_id,
                        principalTable: "personRole",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "researchStudyTeamMember_researcher",
                        column: x => x.researcher_id,
                        principalTable: "researcher",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "personRole",
                columns: new[] { "id", "created", "description", "type" },
                values: new object[] { 1, new DateTime(2024, 2, 20, 10, 33, 23, 980, DateTimeKind.Local).AddTicks(5038), "A Chief investigator role", "CHIEF_INVESTIGATOR" });

            migrationBuilder.InsertData(
                table: "personType",
                columns: new[] { "id", "created", "description" },
                values: new object[] { 1, new DateTime(2024, 2, 20, 10, 33, 23, 980, DateTimeKind.Local).AddTicks(5695), "RESEARCHER" });

            migrationBuilder.InsertData(
                table: "researchInitiativeIdentifierType",
                columns: new[] { "id", "created", "description" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 2, 20, 10, 33, 23, 981, DateTimeKind.Local).AddTicks(5896), "PROJECT" },
                    { 2, new DateTime(2024, 2, 20, 10, 33, 23, 981, DateTimeKind.Local).AddTicks(5922), "PROTOCOL" }
                });

            migrationBuilder.InsertData(
                table: "researchInitiativeType",
                columns: new[] { "id", "created", "description" },
                values: new object[] { 1, new DateTime(2024, 2, 20, 10, 33, 23, 981, DateTimeKind.Local).AddTicks(6579), "STUDY" });

            migrationBuilder.InsertData(
                table: "sourceSystem",
                columns: new[] { "id", "code", "created", "description" },
                values: new object[,]
                {
                    { 1, "EDGE", new DateTime(2024, 2, 20, 10, 33, 23, 982, DateTimeKind.Local).AddTicks(8192), "Edge system" },
                    { 2, "IRAS", new DateTime(2024, 2, 20, 10, 33, 23, 982, DateTimeKind.Local).AddTicks(8221), "IRAS system" }
                });

            migrationBuilder.CreateIndex(
                name: "fk_griMapping_griResearchStudy_idx",
                table: "griMapping",
                column: "griResearchStudy_id");

            migrationBuilder.CreateIndex(
                name: "fk_griMapping_researchInitiativeIdentifier_idx",
                table: "griMapping",
                column: "researchInitiativeIdentifier_id");

            migrationBuilder.CreateIndex(
                name: "fk_griMapping_sourceSystem_idx",
                table: "griMapping",
                column: "sourceSystem_id");

            migrationBuilder.CreateIndex(
                name: "fk_griResearchStudy_researchInitiative_idx",
                table: "griResearchStudy",
                column: "researchInitiative_id");

            migrationBuilder.CreateIndex(
                name: "fk_griResearchStudy_sourceSystem_idx",
                table: "griResearchStudy",
                column: "requestSourceSystem_id");

            migrationBuilder.CreateIndex(
                name: "fk_person_type_idx",
                table: "person",
                column: "personType_id");

            migrationBuilder.CreateIndex(
                name: "fk_personName_person_idx",
                table: "personName",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "fk_researcher_person_idx",
                table: "researcher",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "fk_researchInitiative_type_idx",
                table: "researchInitiative",
                column: "researchInitiativeType_id");

            migrationBuilder.CreateIndex(
                name: "fk_researchInitiativeIdentifier_sourceSystem_idx",
                table: "researchInitiativeIdentifier",
                column: "sourceSystem_id");

            migrationBuilder.CreateIndex(
                name: "fk_researchInitiativeIdentifier_type_idx",
                table: "researchInitiativeIdentifier",
                column: "researchInitiativeIdentifierType_id");

            migrationBuilder.CreateIndex(
                name: "fk_researchStudyTeamMember_griResearch_idx",
                table: "researchStudyTeamMember",
                column: "griMapping_id");

            migrationBuilder.CreateIndex(
                name: "fk_researchStudyTeamMember_personRol_idx",
                table: "researchStudyTeamMember",
                column: "personRole_id");

            migrationBuilder.CreateIndex(
                name: "researchStudyTeamMember_researcher_idx",
                table: "researchStudyTeamMember",
                column: "researcher_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "griMapping");

            migrationBuilder.DropTable(
                name: "personName");

            migrationBuilder.DropTable(
                name: "researchStudyTeamMember");

            migrationBuilder.DropTable(
                name: "researchInitiativeIdentifier");

            migrationBuilder.DropTable(
                name: "griResearchStudy");

            migrationBuilder.DropTable(
                name: "personRole");

            migrationBuilder.DropTable(
                name: "researcher");

            migrationBuilder.DropTable(
                name: "researchInitiativeIdentifierType");

            migrationBuilder.DropTable(
                name: "researchInitiative");

            migrationBuilder.DropTable(
                name: "sourceSystem");

            migrationBuilder.DropTable(
                name: "person");

            migrationBuilder.DropTable(
                name: "researchInitiativeType");

            migrationBuilder.DropTable(
                name: "personType");
        }
    }
}
