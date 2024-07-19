using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incharge.Migrations
{
    /// <inheritdoc />
    public partial class changeContxt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateIndex(
                name: "Client_FirstName",
                table: "clients",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "Client_LastName",
                table: "clients",
                column: "LastName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Client_FirstName",
                table: "clients");

            migrationBuilder.DropIndex(
                name: "Client_LastName",
                table: "clients");

        }
    }
}
