using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class updateidentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_PermissionRole_PermissionRoleId",
                table: "Permissions");

            migrationBuilder.DropTable(
                name: "PermissionRoleRole");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_PermissionRoleId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "PermissionRoleId",
                table: "Permissions");

            migrationBuilder.AddColumn<int>(
                name: "PermissionId",
                table: "PermissionRole",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "PermissionRole",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRole_PermissionId",
                table: "PermissionRole",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRole_RoleId",
                table: "PermissionRole",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionRole_AspNetRoles_RoleId",
                table: "PermissionRole",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionRole_Permissions_PermissionId",
                table: "PermissionRole",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionRole_AspNetRoles_RoleId",
                table: "PermissionRole");

            migrationBuilder.DropForeignKey(
                name: "FK_PermissionRole_Permissions_PermissionId",
                table: "PermissionRole");

            migrationBuilder.DropIndex(
                name: "IX_PermissionRole_PermissionId",
                table: "PermissionRole");

            migrationBuilder.DropIndex(
                name: "IX_PermissionRole_RoleId",
                table: "PermissionRole");

            migrationBuilder.DropColumn(
                name: "PermissionId",
                table: "PermissionRole");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "PermissionRole");

            migrationBuilder.AddColumn<int>(
                name: "PermissionRoleId",
                table: "Permissions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PermissionRoleRole",
                columns: table => new
                {
                    PermissionsId = table.Column<int>(type: "int", nullable: false),
                    RolesId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionRoleRole", x => new { x.PermissionsId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_PermissionRoleRole_AspNetRoles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionRoleRole_PermissionRole_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "PermissionRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_PermissionRoleId",
                table: "Permissions",
                column: "PermissionRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRoleRole_RolesId",
                table: "PermissionRoleRole",
                column: "RolesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_PermissionRole_PermissionRoleId",
                table: "Permissions",
                column: "PermissionRoleId",
                principalTable: "PermissionRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
