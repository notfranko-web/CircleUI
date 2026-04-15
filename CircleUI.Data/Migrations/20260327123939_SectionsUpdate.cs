using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CircleUI.Data.Migrations
{
    /// <inheritdoc />
    public partial class SectionsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Section",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HTMLId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CSSClass = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Section", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SectionComponent_SectionId",
                table: "SectionComponent",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_PageSection_SectionId",
                table: "PageSection",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PageSection_Section_SectionId",
                table: "PageSection",
                column: "SectionId",
                principalTable: "Section",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SectionComponent_Section_SectionId",
                table: "SectionComponent",
                column: "SectionId",
                principalTable: "Section",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageSection_Section_SectionId",
                table: "PageSection");

            migrationBuilder.DropForeignKey(
                name: "FK_SectionComponent_Section_SectionId",
                table: "SectionComponent");

            migrationBuilder.DropTable(
                name: "Section");

            migrationBuilder.DropIndex(
                name: "IX_SectionComponent_SectionId",
                table: "SectionComponent");

            migrationBuilder.DropIndex(
                name: "IX_PageSection_SectionId",
                table: "PageSection");
        }
    }
}
