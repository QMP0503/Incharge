using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incharge.Migrations
{
    /// <inheritdoc />
    public partial class moreUuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Uuid",
                table: "sales",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Uuid",
                table: "paymentrecord",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Uuid",
                table: "expenses",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "discounts",
                type: "decimal(10)",
                precision: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,30)",
                oldPrecision: 10);

            migrationBuilder.AddColumn<string>(
                name: "Uuid",
                table: "business_report",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "Sales_Uuid_UNIQUE",
                table: "sales",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PaymentRecord_Uuid_UNIQUE",
                table: "paymentrecord",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Expenses_Uuid_UNIQUE",
                table: "expenses",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "BusinessReport_Uuid_UNIQUE",
                table: "business_report",
                column: "Uuid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Sales_Uuid_UNIQUE",
                table: "sales");

            migrationBuilder.DropIndex(
                name: "PaymentRecord_Uuid_UNIQUE",
                table: "paymentrecord");

            migrationBuilder.DropIndex(
                name: "Expenses_Uuid_UNIQUE",
                table: "expenses");

            migrationBuilder.DropIndex(
                name: "BusinessReport_Uuid_UNIQUE",
                table: "business_report");

            migrationBuilder.DropColumn(
                name: "Uuid",
                table: "sales");

            migrationBuilder.DropColumn(
                name: "Uuid",
                table: "paymentrecord");

            migrationBuilder.DropColumn(
                name: "Uuid",
                table: "expenses");

            migrationBuilder.DropColumn(
                name: "Uuid",
                table: "business_report");

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "discounts",
                type: "decimal(10,30)",
                precision: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10)",
                oldPrecision: 10);
        }
    }
}
