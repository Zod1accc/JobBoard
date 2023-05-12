using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoard.Migrations
{
    public partial class EditStatusTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Status");

            migrationBuilder.RenameColumn(
                name: "Desc",
                table: "Status",
                newName: "Value");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Status",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "Status");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Status",
                newName: "Desc");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Status",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");
        }
    }
}
