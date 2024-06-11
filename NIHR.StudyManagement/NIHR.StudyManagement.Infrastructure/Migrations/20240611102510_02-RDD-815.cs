using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NIHR.StudyManagement.Infrastructure.Migrations
{
    public partial class _02RDD815 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "studyRecordOutboxEntry",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    payload = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sourcesystem = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    eventtype = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    processingStartDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    processingCompletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studyRecordOutboxEntry", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "organisation",
                keyColumn: "id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 6, 11, 11, 25, 10, 100, DateTimeKind.Local).AddTicks(2982));

            migrationBuilder.UpdateData(
                table: "researchStudyIdentifierType",
                keyColumn: "id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 6, 11, 11, 25, 10, 100, DateTimeKind.Local).AddTicks(2459));

            migrationBuilder.UpdateData(
                table: "researchStudyIdentifierType",
                keyColumn: "id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 6, 11, 11, 25, 10, 100, DateTimeKind.Local).AddTicks(2468));

            migrationBuilder.UpdateData(
                table: "researchStudyIdentifierType",
                keyColumn: "id",
                keyValue: 3,
                column: "created",
                value: new DateTime(2024, 6, 11, 11, 25, 10, 100, DateTimeKind.Local).AddTicks(2470));

            migrationBuilder.UpdateData(
                table: "researchStudyIdentifierType",
                keyColumn: "id",
                keyValue: 4,
                column: "created",
                value: new DateTime(2024, 6, 11, 11, 25, 10, 100, DateTimeKind.Local).AddTicks(2472));

            migrationBuilder.UpdateData(
                table: "roleType",
                keyColumn: "id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 6, 11, 11, 25, 10, 100, DateTimeKind.Local).AddTicks(1831));

            migrationBuilder.UpdateData(
                table: "roleType",
                keyColumn: "id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 6, 11, 11, 25, 10, 100, DateTimeKind.Local).AddTicks(1867));

            migrationBuilder.UpdateData(
                table: "roleType",
                keyColumn: "id",
                keyValue: 3,
                column: "created",
                value: new DateTime(2024, 6, 11, 11, 25, 10, 100, DateTimeKind.Local).AddTicks(1869));

            migrationBuilder.UpdateData(
                table: "roleType",
                keyColumn: "id",
                keyValue: 4,
                column: "created",
                value: new DateTime(2024, 6, 11, 11, 25, 10, 100, DateTimeKind.Local).AddTicks(1871));

            migrationBuilder.UpdateData(
                table: "roleType",
                keyColumn: "id",
                keyValue: 5,
                column: "created",
                value: new DateTime(2024, 6, 11, 11, 25, 10, 100, DateTimeKind.Local).AddTicks(1873));

            migrationBuilder.UpdateData(
                table: "sourceSystem",
                keyColumn: "id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 6, 11, 11, 25, 10, 102, DateTimeKind.Local).AddTicks(1588));

            migrationBuilder.UpdateData(
                table: "sourceSystem",
                keyColumn: "id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 6, 11, 11, 25, 10, 102, DateTimeKind.Local).AddTicks(1635));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "studyRecordOutboxEntry");

            migrationBuilder.UpdateData(
                table: "organisation",
                keyColumn: "id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 6, 7, 10, 16, 44, 744, DateTimeKind.Local).AddTicks(4852));

            migrationBuilder.UpdateData(
                table: "researchStudyIdentifierType",
                keyColumn: "id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 6, 7, 10, 16, 44, 744, DateTimeKind.Local).AddTicks(4215));

            migrationBuilder.UpdateData(
                table: "researchStudyIdentifierType",
                keyColumn: "id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 6, 7, 10, 16, 44, 744, DateTimeKind.Local).AddTicks(4234));

            migrationBuilder.UpdateData(
                table: "researchStudyIdentifierType",
                keyColumn: "id",
                keyValue: 3,
                column: "created",
                value: new DateTime(2024, 6, 7, 10, 16, 44, 744, DateTimeKind.Local).AddTicks(4237));

            migrationBuilder.UpdateData(
                table: "researchStudyIdentifierType",
                keyColumn: "id",
                keyValue: 4,
                column: "created",
                value: new DateTime(2024, 6, 7, 10, 16, 44, 744, DateTimeKind.Local).AddTicks(4240));

            migrationBuilder.UpdateData(
                table: "roleType",
                keyColumn: "id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 6, 7, 10, 16, 44, 744, DateTimeKind.Local).AddTicks(3023));

            migrationBuilder.UpdateData(
                table: "roleType",
                keyColumn: "id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 6, 7, 10, 16, 44, 744, DateTimeKind.Local).AddTicks(3083));

            migrationBuilder.UpdateData(
                table: "roleType",
                keyColumn: "id",
                keyValue: 3,
                column: "created",
                value: new DateTime(2024, 6, 7, 10, 16, 44, 744, DateTimeKind.Local).AddTicks(3087));

            migrationBuilder.UpdateData(
                table: "roleType",
                keyColumn: "id",
                keyValue: 4,
                column: "created",
                value: new DateTime(2024, 6, 7, 10, 16, 44, 744, DateTimeKind.Local).AddTicks(3090));

            migrationBuilder.UpdateData(
                table: "roleType",
                keyColumn: "id",
                keyValue: 5,
                column: "created",
                value: new DateTime(2024, 6, 7, 10, 16, 44, 744, DateTimeKind.Local).AddTicks(3093));

            migrationBuilder.UpdateData(
                table: "sourceSystem",
                keyColumn: "id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 6, 7, 10, 16, 44, 745, DateTimeKind.Local).AddTicks(8636));

            migrationBuilder.UpdateData(
                table: "sourceSystem",
                keyColumn: "id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 6, 7, 10, 16, 44, 745, DateTimeKind.Local).AddTicks(8669));
        }
    }
}
