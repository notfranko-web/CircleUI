using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CircleUI.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProjectColors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackgroundColor",
                table: "WebsiteProjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ButtonColor",
                table: "WebsiteProjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ButtonTextColor",
                table: "WebsiteProjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryTextColor",
                table: "WebsiteProjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SecondaryTextColor",
                table: "WebsiteProjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                table: "WebsiteProjects");

            migrationBuilder.DropColumn(
                name: "ButtonColor",
                table: "WebsiteProjects");

            migrationBuilder.DropColumn(
                name: "ButtonTextColor",
                table: "WebsiteProjects");

            migrationBuilder.DropColumn(
                name: "PrimaryTextColor",
                table: "WebsiteProjects");

            migrationBuilder.DropColumn(
                name: "SecondaryTextColor",
                table: "WebsiteProjects");
        }
    }
}
