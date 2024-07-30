using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incharge.Migrations
{
    /// <inheritdoc />
    public partial class removePaymentRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "PaymentRecordId",
                table: "clients");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_paymentrecord_PaymentrecordId",
                table: "sales");

            migrationBuilder.DropTable(
                name: "paymentrecord");

            migrationBuilder.DropIndex(
                name: "IX_sales_PaymentrecordId",
                table: "sales");

            migrationBuilder.DropIndex(
                name: "IX_clients_PaymentRecordId",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "PaymentrecordId",
                table: "sales");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentrecordId",
                table: "sales",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "paymentrecord",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    paymentstatus = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'"),
                    Uuid = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_sales_PaymentrecordId",
                table: "sales",
                column: "PaymentrecordId");

            migrationBuilder.CreateIndex(
                name: "IX_clients_PaymentRecordId",
                table: "clients",
                column: "PaymentRecordId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PaymentRecord_Uuid_UNIQUE",
                table: "paymentrecord",
                column: "Uuid",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "PaymentRecordId",
                table: "clients",
                column: "PaymentRecordId",
                principalTable: "paymentrecord",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_paymentrecord_PaymentrecordId",
                table: "sales",
                column: "PaymentrecordId",
                principalTable: "paymentrecord",
                principalColumn: "id");
        }
    }
}
