using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Deskstar.Migrations
{
    public partial class deleteRessources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMarkedForDeletion",
                table: "Room",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMarkedForDeletion",
                table: "Floor",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMarkedForDeletion",
                table: "DeskType",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMarkedForDeletion",
                table: "Desk",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMarkedForDeletion",
                table: "Building",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMarkedForDeletion",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "IsMarkedForDeletion",
                table: "Floor");

            migrationBuilder.DropColumn(
                name: "IsMarkedForDeletion",
                table: "DeskType");

            migrationBuilder.DropColumn(
                name: "IsMarkedForDeletion",
                table: "Desk");

            migrationBuilder.DropColumn(
                name: "IsMarkedForDeletion",
                table: "Building");
        }
    }
}
