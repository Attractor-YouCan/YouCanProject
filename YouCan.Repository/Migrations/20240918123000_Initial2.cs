using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace YouCan.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Statistics_StatisticId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ImpactModeEnd",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ImpactModeStart",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "ImpactModeEnd",
                table: "Statistics",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ImpactModeStart",
                table: "Statistics",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStudyDate",
                table: "Statistics",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StatisticId",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RealOrtTests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrtTestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealOrtTests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectLocalizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubjectId = table.Column<int>(type: "integer", nullable: false),
                    Culture = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Subtitle = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectLocalizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectLocalizations_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "IsPublished", "SubjectId" },
                values: new object[] { true, 6 });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "IsPublished", "SubjectId" },
                values: new object[] { true, 6 });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "IsPublished", "SubjectId" },
                values: new object[] { true, 6 });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "IsPublished", "SubjectId" },
                values: new object[] { true, 6 });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "IsPublished", "SubjectId" },
                values: new object[] { true, 6 });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "IsPublished", "SubjectId" },
                values: new object[] { true, 6 });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "IsPublished", "SubjectId" },
                values: new object[] { true, 6 });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "IsPublished", "SubjectId" },
                values: new object[] { true, 6 });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "IsPublished", "SubjectId" },
                values: new object[] { true, 6 });

            migrationBuilder.InsertData(
                table: "RealOrtTests",
                columns: new[] { "Id", "OrtTestDate" },
                values: new object[] { 1, null });

            migrationBuilder.InsertData(
                table: "SubjectLocalizations",
                columns: new[] { "Id", "Culture", "Description", "SubjectId", "Subtitle", "Title" },
                values: new object[,]
                {
                    { 1, "ru", null, 1, null, "Математика" },
                    { 2, "ky", null, 1, null, "Математика" },
                    { 3, "ru", null, 2, null, "Русский язык" },
                    { 4, "ky", null, 2, null, "Орус тили" },
                    { 5, "ru", null, 3, null, "Математика 1" },
                    { 6, "ky", null, 3, null, "Математика 1" },
                    { 7, "ru", null, 4, null, "Математика 2" },
                    { 8, "ky", null, 4, null, "Математика 2" },
                    { 9, "ru", null, 5, null, "Аналогия" },
                    { 10, "ru", null, 6, null, "Грамматика" },
                    { 11, "ru", null, 7, null, "Чтение и понимание" },
                    { 12, "ky", null, 7, null, "Окуу жана тушунуу" },
                    { 13, "ru", null, 8, null, "Аналогия" },
                    { 14, "ru", null, 9, null, "Дополнение предложений" },
                    { 15, "ky", null, 9, null, "Суйломго толуктоо" }
                });

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Математика");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Русский");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Математика 1");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Математика 2");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Аналогия");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Грамматика");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "Чтение и понимание");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Аналогия");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "Дополнение предложений");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectLocalizations_SubjectId",
                table: "SubjectLocalizations",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Statistics_StatisticId",
                table: "AspNetUsers",
                column: "StatisticId",
                principalTable: "Statistics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Statistics_StatisticId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "RealOrtTests");

            migrationBuilder.DropTable(
                name: "SubjectLocalizations");

            migrationBuilder.DropColumn(
                name: "ImpactModeEnd",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "ImpactModeStart",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "LastStudyDate",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "StatisticId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "ImpactModeEnd",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ImpactModeStart",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "IsPublished", "SubjectId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "IsPublished", "SubjectId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "IsPublished", "SubjectId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "IsPublished", "SubjectId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "IsPublished", "SubjectId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "IsPublished", "SubjectId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "IsPublished", "SubjectId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "IsPublished", "SubjectId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "IsPublished", "SubjectId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Mathematics");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Russian");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Mathematics1");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Mathematics2");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Analogy");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Grammar");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "Reading and Understanding");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Analogy");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "Addition of Offers");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Statistics_StatisticId",
                table: "AspNetUsers",
                column: "StatisticId",
                principalTable: "Statistics",
                principalColumn: "Id");
        }
    }
}
