using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Incharge.Migrations
{
    /// <inheritdoc />
    public partial class changeEquipmentAndGymClassRel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "GymClassId",
                table: "equipment");

            migrationBuilder.CreateTable(
                name: "EquipmentGymclass",
                columns: table => new
                {
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    GymClassesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentGymclass", x => new { x.EquipmentId, x.GymClassesId });
                    table.ForeignKey(
                        name: "FK_EquipmentGymclass_equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentGymclass_gymclass_GymClassesId",
                        column: x => x.GymClassesId,
                        principalTable: "gymclass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentGymclass_GymClassesId",
                table: "EquipmentGymclass",
                column: "GymClassesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentGymclass");

            migrationBuilder.AddForeignKey(
                name: "GymClassId",
                table: "equipment",
                column: "GymClassId",
                principalTable: "gymclass",
                principalColumn: "Id");
        }
    }
}
