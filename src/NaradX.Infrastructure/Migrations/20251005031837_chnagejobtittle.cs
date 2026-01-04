using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NaradX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class chnagejobtittle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Contacts",
                newName: "JobTitle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JobTitle",
                table: "Contacts",
                newName: "Title");
        }
    }
}
