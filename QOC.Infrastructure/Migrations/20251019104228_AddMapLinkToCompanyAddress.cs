using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QOC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMapLinkToCompanyAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MapLink",
                table: "CompanyAddresses",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapLink",
                table: "CompanyAddresses");
        }
    }
}
