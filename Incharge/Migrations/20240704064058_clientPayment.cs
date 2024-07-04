using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incharge.Migrations
{
    /// <inheritdoc />
    public partial class clientPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentrecordId",
                table: "sales",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "paymentrecord",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_sales_PaymentrecordId",
                table: "sales",
                column: "PaymentrecordId");

            migrationBuilder.CreateIndex(
                name: "IX_clients_PaymentRecordId",
                table: "clients",
                column: "PaymentRecordId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_paymentrecord_PaymentrecordId",
                table: "sales",
                column: "PaymentrecordId",
                principalTable: "paymentrecord",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sales_paymentrecord_PaymentrecordId",
                table: "sales");

            migrationBuilder.DropIndex(
                name: "IX_sales_PaymentrecordId",
                table: "sales");

            migrationBuilder.DropIndex(
                name: "IX_clients_PaymentRecordId",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "PaymentrecordId",
                table: "sales");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "paymentrecord");
        }
    }
}
