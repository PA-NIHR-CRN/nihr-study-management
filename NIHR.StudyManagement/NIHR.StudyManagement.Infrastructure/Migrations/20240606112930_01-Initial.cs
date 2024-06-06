using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NIHR.StudyManagement.Infrastructure.Migrations
{
    public partial class _01Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "organisation",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organisation", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "person",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "researchStudyIdentifierType",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_researchStudyIdentifierType", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roleType",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roleType", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sourceSystem",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sourceSystem", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "personName",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    person_id = table.Column<int>(type: "int", nullable: false),
                    family = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    given = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
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
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "practitioner",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    person_id = table.Column<int>(type: "int", nullable: false),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_practitioner", x => x.id);
                    table.ForeignKey(
                        name: "fk_researcher_person",
                        column: x => x.person_id,
                        principalTable: "person",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "researchStudy",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    gri = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    shortTitle = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    requestSourceSystem_id = table.Column<int>(type: "int", nullable: false),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_researchStudy", x => x.id);
                    table.ForeignKey(
                        name: "fk_griResearchStudy_sourceSystem",
                        column: x => x.requestSourceSystem_id,
                        principalTable: "sourceSystem",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "practitionerRole",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    researchStudy_id = table.Column<int>(type: "int", nullable: false),
                    practitioner_id = table.Column<int>(type: "int", nullable: false),
                    roleType_id = table.Column<int>(type: "int", nullable: false),
                    effective_from = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    effective_to = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_practitionerRole", x => x.id);
                    table.ForeignKey(
                        name: "fk_researchStudyTeamMember_griResearch",
                        column: x => x.researchStudy_id,
                        principalTable: "researchStudy",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_researchStudyTeamMember_personRol",
                        column: x => x.roleType_id,
                        principalTable: "roleType",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "researchStudyTeamMember_researcher",
                        column: x => x.practitioner_id,
                        principalTable: "practitioner",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "researchStudyIdentifier",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    griResearchStudy_id = table.Column<int>(type: "int", nullable: false),
                    sourceSystem_id = table.Column<int>(type: "int", nullable: false),
                    value = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdentifierTypeId = table.Column<int>(type: "int", nullable: false),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_researchStudyIdentifier", x => x.id);
                    table.ForeignKey(
                        name: "fk_griMapping_griResearchStudy",
                        column: x => x.griResearchStudy_id,
                        principalTable: "researchStudy",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_griMapping_sourceSystem",
                        column: x => x.sourceSystem_id,
                        principalTable: "sourceSystem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_researchStudyIdentifier_researchStudyIdentifierType_Identifi~",
                        column: x => x.IdentifierTypeId,
                        principalTable: "researchStudyIdentifierType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "researchStudyIdentifierStatus",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ResearchStudyIdentifierId = table.Column<int>(type: "int", nullable: false),
                    code = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FromDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_researchStudyIdentifierStatus", x => x.id);
                    table.ForeignKey(
                        name: "fk_griResearchStudyStatus_griMapping",
                        column: x => x.ResearchStudyIdentifierId,
                        principalTable: "researchStudyIdentifier",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "organisation",
                columns: new[] { "id", "code", "created", "description" },
                values: new object[] { 1, "org01", new DateTime(2024, 6, 6, 12, 29, 30, 195, DateTimeKind.Local).AddTicks(7842), "Development organisation" });

            migrationBuilder.InsertData(
                table: "researchStudyIdentifierType",
                columns: new[] { "id", "created", "description" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 6, 6, 12, 29, 30, 195, DateTimeKind.Local).AddTicks(7448), "PROJECT" },
                    { 2, new DateTime(2024, 6, 6, 12, 29, 30, 195, DateTimeKind.Local).AddTicks(7456), "PROTOCOL" },
                    { 3, new DateTime(2024, 6, 6, 12, 29, 30, 195, DateTimeKind.Local).AddTicks(7459), "BUNDLE" },
                    { 4, new DateTime(2024, 6, 6, 12, 29, 30, 195, DateTimeKind.Local).AddTicks(7461), "GRIS ID" }
                });

            migrationBuilder.InsertData(
                table: "roleType",
                columns: new[] { "id", "code", "created", "description" },
                values: new object[,]
                {
                    { 1, "CHF_INV@2.16.840.1.113883.2.1.3.8.5.2.3.5", new DateTime(2024, 6, 6, 12, 29, 30, 195, DateTimeKind.Local).AddTicks(6917), "Chief Investigator" },
                    { 2, "STDY_CRDNTR@2.16.840.1.113883.2.1.3.8.5.2.3.5", new DateTime(2024, 6, 6, 12, 29, 30, 195, DateTimeKind.Local).AddTicks(6957), "Study Coordinator" },
                    { 3, "RSRCH_ACT_CRDNTR@2.16.840.1.113883.2.1.3.8.5.2.3.5", new DateTime(2024, 6, 6, 12, 29, 30, 195, DateTimeKind.Local).AddTicks(6960), "Research Activity Coordinator" },
                    { 4, "PRNCPL_INV@2.16.840.1.113883.2.1.3.8.5.2.3.5", new DateTime(2024, 6, 6, 12, 29, 30, 195, DateTimeKind.Local).AddTicks(6962), "Principal Investigator" },
                    { 5, "CMPNY_RP@2.16.840.1.113883.2.1.3.8.5.2.3.5", new DateTime(2024, 6, 6, 12, 29, 30, 195, DateTimeKind.Local).AddTicks(6963), "Company Representative" }
                });

            migrationBuilder.InsertData(
                table: "sourceSystem",
                columns: new[] { "id", "code", "created", "description" },
                values: new object[,]
                {
                    { 1, "EDGE", new DateTime(2024, 6, 6, 12, 29, 30, 196, DateTimeKind.Local).AddTicks(6503), "Edge system" },
                    { 2, "IRAS", new DateTime(2024, 6, 6, 12, 29, 30, 196, DateTimeKind.Local).AddTicks(6519), "IRAS system" }
                });

            migrationBuilder.CreateIndex(
                name: "fk_personName_person_idx",
                table: "personName",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "fk_researcher_person_idx",
                table: "practitioner",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "fk_researchStudyTeamMember_griResearch_idx",
                table: "practitionerRole",
                column: "researchStudy_id");

            migrationBuilder.CreateIndex(
                name: "fk_researchStudyTeamMember_personRol_idx",
                table: "practitionerRole",
                column: "roleType_id");

            migrationBuilder.CreateIndex(
                name: "researchStudyTeamMember_researcher_idx",
                table: "practitionerRole",
                column: "practitioner_id");

            migrationBuilder.CreateIndex(
                name: "fk_griResearchStudy_sourceSystem_idx",
                table: "researchStudy",
                column: "requestSourceSystem_id");

            migrationBuilder.CreateIndex(
                name: "fk_griMapping_griResearchStudy_idx",
                table: "researchStudyIdentifier",
                column: "griResearchStudy_id");

            migrationBuilder.CreateIndex(
                name: "fk_griMapping_sourceSystem_idx",
                table: "researchStudyIdentifier",
                column: "sourceSystem_id");

            migrationBuilder.CreateIndex(
                name: "IX_researchStudyIdentifier_IdentifierTypeId",
                table: "researchStudyIdentifier",
                column: "IdentifierTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_researchStudyIdentifierStatus_ResearchStudyIdentifierId",
                table: "researchStudyIdentifierStatus",
                column: "ResearchStudyIdentifierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "organisation");

            migrationBuilder.DropTable(
                name: "personName");

            migrationBuilder.DropTable(
                name: "practitionerRole");

            migrationBuilder.DropTable(
                name: "researchStudyIdentifierStatus");

            migrationBuilder.DropTable(
                name: "roleType");

            migrationBuilder.DropTable(
                name: "practitioner");

            migrationBuilder.DropTable(
                name: "researchStudyIdentifier");

            migrationBuilder.DropTable(
                name: "person");

            migrationBuilder.DropTable(
                name: "researchStudy");

            migrationBuilder.DropTable(
                name: "researchStudyIdentifierType");

            migrationBuilder.DropTable(
                name: "sourceSystem");
        }
    }
}
