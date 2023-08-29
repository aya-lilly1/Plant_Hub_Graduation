using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plant_Hub_Models.Migrations
{
    public partial class addAr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CareDetailsAr",
                table: "Plants",
                type: "nvarchar(800)",
                maxLength: 800,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Plants",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MedicalBenefitAr",
                table: "Plants",
                type: "nvarchar(800)",
                maxLength: 800,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PlantNameAr",
                table: "Plants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeasonAr",
                table: "Plants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CategoryNameAr",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Categories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CareDetailsAr",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "MedicalBenefitAr",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "PlantNameAr",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "SeasonAr",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "CategoryNameAr",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Categories");
        }
    }
}
