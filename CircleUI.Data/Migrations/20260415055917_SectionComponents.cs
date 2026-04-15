using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CircleUI.Data.Migrations
{
    /// <inheritdoc />
    public partial class SectionComponents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SectionComponent_Components_ComponentId",
                table: "SectionComponent");

            migrationBuilder.DropForeignKey(
                name: "FK_SectionComponent_Sections_SectionId",
                table: "SectionComponent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SectionComponent",
                table: "SectionComponent");

            migrationBuilder.RenameTable(
                name: "SectionComponent",
                newName: "SectionComponents");

            migrationBuilder.RenameIndex(
                name: "IX_SectionComponent_SectionId",
                table: "SectionComponents",
                newName: "IX_SectionComponents_SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_SectionComponent_ComponentId",
                table: "SectionComponents",
                newName: "IX_SectionComponents_ComponentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SectionComponents",
                table: "SectionComponents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SectionComponents_Components_ComponentId",
                table: "SectionComponents",
                column: "ComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SectionComponents_Sections_SectionId",
                table: "SectionComponents",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SectionComponents_Components_ComponentId",
                table: "SectionComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_SectionComponents_Sections_SectionId",
                table: "SectionComponents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SectionComponents",
                table: "SectionComponents");

            migrationBuilder.RenameTable(
                name: "SectionComponents",
                newName: "SectionComponent");

            migrationBuilder.RenameIndex(
                name: "IX_SectionComponents_SectionId",
                table: "SectionComponent",
                newName: "IX_SectionComponent_SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_SectionComponents_ComponentId",
                table: "SectionComponent",
                newName: "IX_SectionComponent_ComponentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SectionComponent",
                table: "SectionComponent",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SectionComponent_Components_ComponentId",
                table: "SectionComponent",
                column: "ComponentId",
                principalTable: "Components",
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
    }
}
