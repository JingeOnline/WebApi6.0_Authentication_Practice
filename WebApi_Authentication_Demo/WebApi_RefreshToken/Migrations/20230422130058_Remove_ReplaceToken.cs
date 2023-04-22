using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi_RefreshToken.Migrations
{
    public partial class Remove_ReplaceToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReplacedByToken",
                table: "tb_RefreshToken");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReplacedByToken",
                table: "tb_RefreshToken",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
