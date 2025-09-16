using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NaradX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changecontact_new1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "config");

            migrationBuilder.CreateTable(
                name: "ConfigMasters",
                schema: "config",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfigKey = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsTenantSpecific = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DeactivatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeactivatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfigValues",
                schema: "config",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfigMasterId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ItemText = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ItemValue = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DeactivatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeactivatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfigValues_ConfigMasters_ConfigMasterId",
                        column: x => x.ConfigMasterId,
                        principalSchema: "config",
                        principalTable: "ConfigMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConfigValues_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigMasters_ConfigKey",
                schema: "config",
                table: "ConfigMasters",
                column: "ConfigKey",
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigValues_ConfigMasterId_TenantId_DisplayOrder",
                schema: "config",
                table: "ConfigValues",
                columns: new[] { "ConfigMasterId", "TenantId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigValues_ConfigMasterId_TenantId_ItemValue",
                schema: "config",
                table: "ConfigValues",
                columns: new[] { "ConfigMasterId", "TenantId", "ItemValue" },
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigValues_TenantId",
                schema: "config",
                table: "ConfigValues",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigValues",
                schema: "config");

            migrationBuilder.DropTable(
                name: "ConfigMasters",
                schema: "config");
        }
    }
}
