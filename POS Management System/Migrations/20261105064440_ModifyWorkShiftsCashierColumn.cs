using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class ModifyWorkShiftsCashierColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkShiftsCashiers_WorkShifts_WorkShiftsId",
                table: "WorkShiftsCashiers");

            migrationBuilder.DropIndex(
                name: "IX_WorkShiftsCashiers_WorkShiftsId",
                table: "WorkShiftsCashiers");

            migrationBuilder.DropColumn(
                name: "WorkShiftsId",
                table: "WorkShiftsCashiers");

            migrationBuilder.CreateIndex(
                name: "IX_WorkShiftsCashiers_WorkShiftId",
                table: "WorkShiftsCashiers",
                column: "WorkShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkShiftsCashiers_WorkShifts_WorkShiftId",
                table: "WorkShiftsCashiers",
                column: "WorkShiftId",
                principalTable: "WorkShifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkShiftsCashiers_WorkShifts_WorkShiftId",
                table: "WorkShiftsCashiers");

            migrationBuilder.DropIndex(
                name: "IX_WorkShiftsCashiers_WorkShiftId",
                table: "WorkShiftsCashiers");

            migrationBuilder.AddColumn<int>(
                name: "WorkShiftsId",
                table: "WorkShiftsCashiers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WorkShiftsCashiers_WorkShiftsId",
                table: "WorkShiftsCashiers",
                column: "WorkShiftsId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkShiftsCashiers_WorkShifts_WorkShiftsId",
                table: "WorkShiftsCashiers",
                column: "WorkShiftsId",
                principalTable: "WorkShifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
