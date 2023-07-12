using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCardTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "CVC",
                table: "Cards",
                newName: "Cvc");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "Cards",
                newName: "HolderName");

            migrationBuilder.RenameColumn(
                name: "Month",
                table: "Cards",
                newName: "ExpireYear");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Cards",
                newName: "ExpireMonth");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cvc",
                table: "Cards",
                newName: "CVC");

            migrationBuilder.RenameColumn(
                name: "HolderName",
                table: "Cards",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "ExpireYear",
                table: "Cards",
                newName: "Month");

            migrationBuilder.RenameColumn(
                name: "ExpireMonth",
                table: "Cards",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
