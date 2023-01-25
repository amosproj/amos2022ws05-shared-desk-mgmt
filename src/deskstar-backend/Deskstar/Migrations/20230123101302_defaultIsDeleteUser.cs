using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Deskstar.Migrations
{
    public partial class defaultIsDeleteUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsMarkedForDeletion",
                table: "User",
                type: "boolean",
                nullable: false,
                defaultValueSql: "false",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsMarkedForDeletion",
                table: "User",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValueSql: "false");
        }
    }
}
