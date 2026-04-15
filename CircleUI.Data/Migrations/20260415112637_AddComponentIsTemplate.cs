using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CircleUI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddComponentIsTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTemplate",
                table: "Components",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTemplate",
                table: "Components");
        }
    }
}
