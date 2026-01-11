using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NaradX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TemplatetablesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buttons_Components_ComponentId",
                table: "Buttons");

            migrationBuilder.DropForeignKey(
                name: "FK_Components_WhatsAppTemplates_WhatsAppTemplateId",
                table: "Components");

            migrationBuilder.DropIndex(
                name: "IX_Components_WhatsAppTemplateId",
                table: "Components");

            migrationBuilder.DropIndex(
                name: "IX_Buttons_ComponentId",
                table: "Buttons");

            migrationBuilder.DropColumn(
                name: "WhatsAppTemplateId",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "ComponentId",
                table: "Buttons");

            migrationBuilder.AddForeignKey(
                name: "FK_Buttons_Components_Id",
                table: "Buttons",
                column: "Id",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Components_WhatsAppTemplates_Id",
                table: "Components",
                column: "Id",
                principalTable: "WhatsAppTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buttons_Components_Id",
                table: "Buttons");

            migrationBuilder.DropForeignKey(
                name: "FK_Components_WhatsAppTemplates_Id",
                table: "Components");

            migrationBuilder.AddColumn<Guid>(
                name: "WhatsAppTemplateId",
                table: "Components",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ComponentId",
                table: "Buttons",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Components_WhatsAppTemplateId",
                table: "Components",
                column: "WhatsAppTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Buttons_ComponentId",
                table: "Buttons",
                column: "ComponentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buttons_Components_ComponentId",
                table: "Buttons",
                column: "ComponentId",
                principalTable: "Components",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Components_WhatsAppTemplates_WhatsAppTemplateId",
                table: "Components",
                column: "WhatsAppTemplateId",
                principalTable: "WhatsAppTemplates",
                principalColumn: "Id");
        }
    }
}
