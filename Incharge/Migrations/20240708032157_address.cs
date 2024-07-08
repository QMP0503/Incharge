using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incharge.Migrations
{
    /// <inheritdoc />
    public partial class address : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "employees",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "clients",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "MonthlyMembers",
                table: "business_report",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "MonthlyMembers",
                table: "business_report");
        }
    }
}
