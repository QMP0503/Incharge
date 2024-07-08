using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incharge.Migrations
{
    /// <inheritdoc />
    public partial class saleDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_discount");

            migrationBuilder.CreateTable(
                name: "sale_discount",
                columns: table => new
                {
                    saleid = table.Column<int>(type: "int", nullable: false),
                    discountid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.saleid, x.discountid });
                    table.ForeignKey(
                        name: "sale_discount_ibfk_1",
                        column: x => x.saleid,
                        principalTable: "sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "sale_discount_ibfk_2",
                        column: x => x.discountid,
                        principalTable: "discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "discountid",
                table: "sale_discount",
                column: "discountid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sale_discount");

            migrationBuilder.CreateTable(
                name: "product_discount",
                columns: table => new
                {
                    productid = table.Column<int>(type: "int", nullable: false),
                    discountid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.productid, x.discountid });
                    table.ForeignKey(
                        name: "product_discount_ibfk_1",
                        column: x => x.productid,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "product_discount_ibfk_2",
                        column: x => x.discountid,
                        principalTable: "discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "discountid",
                table: "product_discount",
                column: "discountid");
        }
    }
}
