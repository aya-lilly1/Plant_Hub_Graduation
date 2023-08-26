using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plant_Hub_Models.Migrations
{
    public partial class editusertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConfirmationCode",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmationCode",
                table: "AspNetUsers");
        }
    }
}
