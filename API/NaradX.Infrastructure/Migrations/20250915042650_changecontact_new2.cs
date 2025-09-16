using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NaradX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changecontact_new2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ConfigValues",
                schema: "config",
                newName: "ConfigValues");

            migrationBuilder.RenameTable(
                name: "ConfigMasters",
                schema: "config",
                newName: "ConfigMasters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "config");

            migrationBuilder.RenameTable(
                name: "ConfigValues",
                newName: "ConfigValues",
                newSchema: "config");

            migrationBuilder.RenameTable(
                name: "ConfigMasters",
                newName: "ConfigMasters",
                newSchema: "config");
        }
    }
}
