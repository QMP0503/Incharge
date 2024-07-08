using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incharge.Migrations
{
    /// <inheritdoc />
    public partial class uuidBR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Uuid",
                table: "business_report",
                type: "char(255)",
                fixedLength: true,
                nullable: false,
                defaultValueSql: "(uuid())",
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Uuid",
                table: "business_report",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(255)",
                oldFixedLength: true,
                oldDefaultValueSql: "(uuid())")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
