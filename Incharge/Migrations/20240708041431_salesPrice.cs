using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incharge.Migrations
{
    /// <inheritdoc />
    public partial class salesPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "sales",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "expenses",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "sales");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "expenses");
        }
    }
}
