using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class DBMASmodifyCashier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreakMinutes",
                table: "CashierAttendances");

            migrationBuilder.CreateTable(
                name: "Break",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Break", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CashierBreak",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BreakId = table.Column<int>(type: "int", nullable: false),
                    CashierAttendanceId = table.Column<int>(type: "int", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: true),
                    End = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashierBreak", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashierBreak_Break_BreakId",
                        column: x => x.BreakId,
                        principalTable: "Break",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CashierBreak_CashierAttendances_CashierAttendanceId",
                        column: x => x.CashierAttendanceId,
                        principalTable: "CashierAttendances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CashierBreak_BreakId",
                table: "CashierBreak",
                column: "BreakId");

            migrationBuilder.CreateIndex(
                name: "IX_CashierBreak_CashierAttendanceId",
                table: "CashierBreak",
                column: "CashierAttendanceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashierBreak");

            migrationBuilder.DropTable(
                name: "Break");

            migrationBuilder.AddColumn<int>(
                name: "BreakMinutes",
                table: "CashierAttendances",
                type: "int",
                nullable: true);
        }
    }
}
