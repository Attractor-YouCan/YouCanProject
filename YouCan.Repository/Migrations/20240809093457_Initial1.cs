using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace YouCan.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Initial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TariffEndDate",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TariffId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tariffs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tariffs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Tariffs",
                columns: new[] { "Id", "Duration", "Name", "Price" },
                values: new object[,]
                {
                    { 1, null, "Start", 0 },
                    { 2, 1, "Pro", 1 },
                    { 3, 3, "Premium", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TariffId",
                table: "AspNetUsers",
                column: "TariffId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Tariffs_TariffId",
                table: "AspNetUsers",
                column: "TariffId",
                principalTable: "Tariffs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Tariffs_TariffId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Tariffs");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TariffId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TariffEndDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TariffId",
                table: "AspNetUsers");
        }
    }
}
