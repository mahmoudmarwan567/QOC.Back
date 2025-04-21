using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QOC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class projects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Sliders",
                newName: "TitleEN");

            migrationBuilder.RenameColumn(
                name: "Subtitle",
                table: "Sliders",
                newName: "TitleAR");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Sliders",
                newName: "SubtitleEN");

            migrationBuilder.RenameColumn(
                name: "ButtonText",
                table: "Sliders",
                newName: "SubtitleAR");

            migrationBuilder.RenameColumn(
                name: "ProjectProperties",
                table: "Projects",
                newName: "ProjectPropertiesEN");

            migrationBuilder.RenameColumn(
                name: "ProjectName",
                table: "Projects",
                newName: "ProjectPropertiesAR");

            migrationBuilder.RenameColumn(
                name: "ProjectDescription",
                table: "Projects",
                newName: "ProjectNameEN");

            migrationBuilder.AddColumn<string>(
                name: "ButtonTextAR",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ButtonTextEN",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAR",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEN",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectDescriptionAR",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectDescriptionEN",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectNameAR",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ButtonTextAR",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "ButtonTextEN",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "DescriptionAR",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "DescriptionEN",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "ProjectDescriptionAR",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectDescriptionEN",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectNameAR",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "TitleEN",
                table: "Sliders",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "TitleAR",
                table: "Sliders",
                newName: "Subtitle");

            migrationBuilder.RenameColumn(
                name: "SubtitleEN",
                table: "Sliders",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "SubtitleAR",
                table: "Sliders",
                newName: "ButtonText");

            migrationBuilder.RenameColumn(
                name: "ProjectPropertiesEN",
                table: "Projects",
                newName: "ProjectProperties");

            migrationBuilder.RenameColumn(
                name: "ProjectPropertiesAR",
                table: "Projects",
                newName: "ProjectName");

            migrationBuilder.RenameColumn(
                name: "ProjectNameEN",
                table: "Projects",
                newName: "ProjectDescription");
        }
    }
}
