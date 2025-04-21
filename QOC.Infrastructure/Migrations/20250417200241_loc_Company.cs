using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QOC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class loc_Company : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SocialName",
                table: "CompanySocials",
                newName: "SocialNameEN");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "CompanyAddresses",
                newName: "AddressEN");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Companies",
                newName: "NameEN");

            migrationBuilder.AddColumn<string>(
                name: "ProjectProperties",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SocialNameAR",
                table: "CompanySocials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressAR",
                table: "CompanyAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameAR",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectProperties",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "SocialNameAR",
                table: "CompanySocials");

            migrationBuilder.DropColumn(
                name: "AddressAR",
                table: "CompanyAddresses");

            migrationBuilder.DropColumn(
                name: "NameAR",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "SocialNameEN",
                table: "CompanySocials",
                newName: "SocialName");

            migrationBuilder.RenameColumn(
                name: "AddressEN",
                table: "CompanyAddresses",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "NameEN",
                table: "Companies",
                newName: "Name");
        }
    }
}
