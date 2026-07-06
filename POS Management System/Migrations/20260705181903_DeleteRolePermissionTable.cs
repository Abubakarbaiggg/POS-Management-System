using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class DeleteRolePermissionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_SupplierPayments_StockInId",
                table: "SupplierPayments");

            migrationBuilder.DropIndex(
                name: "IX_CustomerPayments_StockOutId",
                table: "CustomerPayments");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierPayments_StockInId",
                table: "SupplierPayments",
                column: "StockInId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPayments_StockOutId",
                table: "CustomerPayments",
                column: "StockOutId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SupplierPayments_StockInId",
                table: "SupplierPayments");

            migrationBuilder.DropIndex(
                name: "IX_CustomerPayments_StockOutId",
                table: "CustomerPayments");

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplierPayments_StockInId",
                table: "SupplierPayments",
                column: "StockInId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPayments_StockOutId",
                table: "CustomerPayments",
                column: "StockOutId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions",
                column: "RoleId");
        }
    }
}
