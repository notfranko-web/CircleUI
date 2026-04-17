using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CircleUI.Data.Migrations
{
    /// <inheritdoc />
    public partial class HeaderFooterSections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FooterSectionId",
                table: "WebsiteProjects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "HeaderSectionId",
                table: "WebsiteProjects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteProjects_FooterSectionId",
                table: "WebsiteProjects",
                column: "FooterSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteProjects_HeaderSectionId",
                table: "WebsiteProjects",
                column: "HeaderSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebsiteProjects_Sections_FooterSectionId",
                table: "WebsiteProjects",
                column: "FooterSectionId",
                principalTable: "Sections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WebsiteProjects_Sections_HeaderSectionId",
                table: "WebsiteProjects",
                column: "HeaderSectionId",
                principalTable: "Sections",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebsiteProjects_Sections_FooterSectionId",
                table: "WebsiteProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_WebsiteProjects_Sections_HeaderSectionId",
                table: "WebsiteProjects");

            migrationBuilder.DropIndex(
                name: "IX_WebsiteProjects_FooterSectionId",
                table: "WebsiteProjects");

            migrationBuilder.DropIndex(
                name: "IX_WebsiteProjects_HeaderSectionId",
                table: "WebsiteProjects");

            migrationBuilder.DropColumn(
                name: "FooterSectionId",
                table: "WebsiteProjects");

            migrationBuilder.DropColumn(
                name: "HeaderSectionId",
                table: "WebsiteProjects");
        }
    }
}
