using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CircleUI.Data.Migrations
{
    /// <inheritdoc />
    public partial class SectionAndPageModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_Components_ParentId",
                table: "Components");

            migrationBuilder.DropForeignKey(
                name: "FK_Components_Pages_PageId",
                table: "Components");

            migrationBuilder.DropIndex(
                name: "IX_Components_PageId",
                table: "Components");

            migrationBuilder.DropIndex(
                name: "IX_Components_ParentId",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "PageId",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Components");

            migrationBuilder.CreateTable(
                name: "PageSection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageSection_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SectionComponent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionComponent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SectionComponent_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PageSection_PageId",
                table: "PageSection",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_SectionComponent_ComponentId",
                table: "SectionComponent",
                column: "ComponentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PageSection");

            migrationBuilder.DropTable(
                name: "SectionComponent");

            migrationBuilder.AddColumn<Guid>(
                name: "PageId",
                table: "Components",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Components",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Components_PageId",
                table: "Components",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_Components_ParentId",
                table: "Components",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Components_ParentId",
                table: "Components",
                column: "ParentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Pages_PageId",
                table: "Components",
                column: "PageId",
                principalTable: "Pages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
