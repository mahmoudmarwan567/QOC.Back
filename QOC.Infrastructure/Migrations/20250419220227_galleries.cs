using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QOC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class galleries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ProjectCategories",
                newName: "NameEN");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ProjectCategories",
                newName: "NameAR");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAR",
                table: "ProjectCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEN",
                table: "ProjectCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Galleries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Galleries", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Galleries");

            migrationBuilder.DropColumn(
                name: "DescriptionAR",
                table: "ProjectCategories");

            migrationBuilder.DropColumn(
                name: "DescriptionEN",
                table: "ProjectCategories");

            migrationBuilder.RenameColumn(
                name: "NameEN",
                table: "ProjectCategories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NameAR",
                table: "ProjectCategories",
                newName: "Description");
        }
    }
}
