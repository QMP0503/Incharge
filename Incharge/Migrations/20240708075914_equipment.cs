using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incharge.Migrations
{
    /// <inheritdoc />
    public partial class equipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EquipmentAndMantaince",
                table: "business_report",
                newName: "Mantaince");

            migrationBuilder.AlterColumn<double>(
                name: "Revenue",
                table: "business_report",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Cost",
                table: "business_report",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Equipment",
                table: "business_report",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Equipment",
                table: "business_report");

            migrationBuilder.RenameColumn(
                name: "Mantaince",
                table: "business_report",
                newName: "EquipmentAndMantaince");

            migrationBuilder.AlterColumn<double>(
                name: "Revenue",
                table: "business_report",
                type: "double",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<double>(
                name: "Cost",
                table: "business_report",
                type: "double",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double");
        }
    }
}
