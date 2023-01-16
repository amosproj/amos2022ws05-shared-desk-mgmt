using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Deskstar.Migrations
{
    public partial class MarkUserAsDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMarkedForDeletion",
                table: "User",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMarkedForDeletion",
                table: "User");
        }
    }
}
