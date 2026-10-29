using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class DBMASmodifyCashiers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashierBreak_Break_BreakId",
                table: "CashierBreak");

            migrationBuilder.DropForeignKey(
                name: "FK_CashierBreak_CashierAttendances_CashierAttendanceId",
                table: "CashierBreak");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CashierBreak",
                table: "CashierBreak");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Break",
                table: "Break");

            migrationBuilder.RenameTable(
                name: "CashierBreak",
                newName: "CashierBreaks");

            migrationBuilder.RenameTable(
                name: "Break",
                newName: "Breaks");

            migrationBuilder.RenameIndex(
                name: "IX_CashierBreak_CashierAttendanceId",
                table: "CashierBreaks",
                newName: "IX_CashierBreaks_CashierAttendanceId");

            migrationBuilder.RenameIndex(
                name: "IX_CashierBreak_BreakId",
                table: "CashierBreaks",
                newName: "IX_CashierBreaks_BreakId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CashierBreaks",
                table: "CashierBreaks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Breaks",
                table: "Breaks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CashierBreaks_Breaks_BreakId",
                table: "CashierBreaks",
                column: "BreakId",
                principalTable: "Breaks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CashierBreaks_CashierAttendances_CashierAttendanceId",
                table: "CashierBreaks",
                column: "CashierAttendanceId",
                principalTable: "CashierAttendances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashierBreaks_Breaks_BreakId",
                table: "CashierBreaks");

            migrationBuilder.DropForeignKey(
                name: "FK_CashierBreaks_CashierAttendances_CashierAttendanceId",
                table: "CashierBreaks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CashierBreaks",
                table: "CashierBreaks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Breaks",
                table: "Breaks");

            migrationBuilder.RenameTable(
                name: "CashierBreaks",
                newName: "CashierBreak");

            migrationBuilder.RenameTable(
                name: "Breaks",
                newName: "Break");

            migrationBuilder.RenameIndex(
                name: "IX_CashierBreaks_CashierAttendanceId",
                table: "CashierBreak",
                newName: "IX_CashierBreak_CashierAttendanceId");

            migrationBuilder.RenameIndex(
                name: "IX_CashierBreaks_BreakId",
                table: "CashierBreak",
                newName: "IX_CashierBreak_BreakId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CashierBreak",
                table: "CashierBreak",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Break",
                table: "Break",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CashierBreak_Break_BreakId",
                table: "CashierBreak",
                column: "BreakId",
                principalTable: "Break",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CashierBreak_CashierAttendances_CashierAttendanceId",
                table: "CashierBreak",
                column: "CashierAttendanceId",
                principalTable: "CashierAttendances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
