using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CircleUI.Data.Migrations
{
    /// <inheritdoc />
    public partial class SectionsDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageSection_Pages_PageId",
                table: "PageSection");

            migrationBuilder.DropForeignKey(
                name: "FK_PageSection_Section_SectionId",
                table: "PageSection");

            migrationBuilder.DropForeignKey(
                name: "FK_SectionComponent_Section_SectionId",
                table: "SectionComponent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Section",
                table: "Section");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PageSection",
                table: "PageSection");

            migrationBuilder.RenameTable(
                name: "Section",
                newName: "Sections");

            migrationBuilder.RenameTable(
                name: "PageSection",
                newName: "PageSections");

            migrationBuilder.RenameIndex(
                name: "IX_PageSection_SectionId",
                table: "PageSections",
                newName: "IX_PageSections_SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_PageSection_PageId",
                table: "PageSections",
                newName: "IX_PageSections_PageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sections",
                table: "Sections",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PageSections",
                table: "PageSections",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PageSections_Pages_PageId",
                table: "PageSections",
                column: "PageId",
                principalTable: "Pages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PageSections_Sections_SectionId",
                table: "PageSections",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SectionComponent_Sections_SectionId",
                table: "SectionComponent",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageSections_Pages_PageId",
                table: "PageSections");

            migrationBuilder.DropForeignKey(
                name: "FK_PageSections_Sections_SectionId",
                table: "PageSections");

            migrationBuilder.DropForeignKey(
                name: "FK_SectionComponent_Sections_SectionId",
                table: "SectionComponent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sections",
                table: "Sections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PageSections",
                table: "PageSections");

            migrationBuilder.RenameTable(
                name: "Sections",
                newName: "Section");

            migrationBuilder.RenameTable(
                name: "PageSections",
                newName: "PageSection");

            migrationBuilder.RenameIndex(
                name: "IX_PageSections_SectionId",
                table: "PageSection",
                newName: "IX_PageSection_SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_PageSections_PageId",
                table: "PageSection",
                newName: "IX_PageSection_PageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Section",
                table: "Section",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PageSection",
                table: "PageSection",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PageSection_Pages_PageId",
                table: "PageSection",
                column: "PageId",
                principalTable: "Pages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
