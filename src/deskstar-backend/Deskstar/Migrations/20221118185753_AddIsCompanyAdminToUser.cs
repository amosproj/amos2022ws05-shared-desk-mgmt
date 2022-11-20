using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Deskstar.Migrations
{
    public partial class AddIsCompanyAdminToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompanyAdmin",
                table: "User",
                type: "boolean",
                nullable: false,
                defaultValueSql: "false");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompanyAdmin",
                table: "User");
        }
    }
}
