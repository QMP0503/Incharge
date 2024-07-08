using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incharge.Migrations
{
    /// <inheritdoc />
    public partial class expenseType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "TotalPrice",
                table: "products",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AccountsPayable",
                table: "business_report",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AccountsRecievable",
                table: "business_report",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EquipmentAndMantaince",
                table: "business_report",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Insurance",
                table: "business_report",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MembershipFee",
                table: "business_report",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NewMembershipSales",
                table: "business_report",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OtherExpenses",
                table: "business_report",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Profit",
                table: "business_report",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Rent",
                table: "business_report",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Utilities",
                table: "business_report",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Wages",
                table: "business_report",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountsPayable",
                table: "business_report");

            migrationBuilder.DropColumn(
                name: "AccountsRecievable",
                table: "business_report");

            migrationBuilder.DropColumn(
                name: "EquipmentAndMantaince",
                table: "business_report");

            migrationBuilder.DropColumn(
                name: "Insurance",
                table: "business_report");

            migrationBuilder.DropColumn(
                name: "MembershipFee",
                table: "business_report");

            migrationBuilder.DropColumn(
                name: "NewMembershipSales",
                table: "business_report");

            migrationBuilder.DropColumn(
                name: "OtherExpenses",
                table: "business_report");

            migrationBuilder.DropColumn(
                name: "Profit",
                table: "business_report");

            migrationBuilder.DropColumn(
                name: "Rent",
                table: "business_report");

            migrationBuilder.DropColumn(
                name: "Utilities",
                table: "business_report");

            migrationBuilder.DropColumn(
                name: "Wages",
                table: "business_report");

            migrationBuilder.AlterColumn<double>(
                name: "TotalPrice",
                table: "products",
                type: "double",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double");
        }
    }
}
