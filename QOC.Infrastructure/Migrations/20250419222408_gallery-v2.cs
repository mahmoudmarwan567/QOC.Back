using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QOC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class galleryv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Galleries",
                newName: "DescriptionEN");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAR",
                table: "Galleries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionAR",
                table: "Galleries");

            migrationBuilder.RenameColumn(
                name: "DescriptionEN",
                table: "Galleries",
                newName: "Description");
        }
    }
}
