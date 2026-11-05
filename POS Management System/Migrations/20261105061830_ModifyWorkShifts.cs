using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class ModifyWorkShifts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkShifts_AspNetUsers_UserId",
                table: "WorkShifts");

            migrationBuilder.DropIndex(
                name: "IX_WorkShifts_UserId",
                table: "WorkShifts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WorkShifts");

            migrationBuilder.CreateTable(
                name: "WorkShiftsCashiers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkShiftId = table.Column<int>(type: "int", nullable: false),
                    WorkShiftsId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkShiftsCashiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkShiftsCashiers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkShiftsCashiers_WorkShifts_WorkShiftsId",
                        column: x => x.WorkShiftsId,
                        principalTable: "WorkShifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkShiftsCashiers_UserId",
                table: "WorkShiftsCashiers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkShiftsCashiers_WorkShiftsId",
                table: "WorkShiftsCashiers",
                column: "WorkShiftsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkShiftsCashiers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "WorkShifts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_WorkShifts_UserId",
                table: "WorkShifts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkShifts_AspNetUsers_UserId",
                table: "WorkShifts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
