using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QOC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class aboutUs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Services",
                newName: "TitleEN");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Services",
                newName: "TitleAR");

            migrationBuilder.RenameColumn(
                name: "FullDescription",
                table: "AboutUs",
                newName: "FullDescriptionEN");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "AboutUs",
                newName: "FullDescriptionAR");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAR",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEN",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "ProjectCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAR",
                table: "AboutUs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEN",
                table: "AboutUs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionAR",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "DescriptionEN",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "ProjectCategories");

            migrationBuilder.DropColumn(
                name: "DescriptionAR",
                table: "AboutUs");

            migrationBuilder.DropColumn(
                name: "DescriptionEN",
                table: "AboutUs");

            migrationBuilder.RenameColumn(
                name: "TitleEN",
                table: "Services",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "TitleAR",
                table: "Services",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "FullDescriptionEN",
                table: "AboutUs",
                newName: "FullDescription");

            migrationBuilder.RenameColumn(
                name: "FullDescriptionAR",
                table: "AboutUs",
                newName: "Description");
        }
    }
}
