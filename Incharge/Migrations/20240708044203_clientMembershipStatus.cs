using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incharge.Migrations
{
    /// <inheritdoc />
    public partial class clientMembershipStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MembershipStatus",
                table: "clients",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MembershipStatus",
                table: "clients");
        }
    }
}
