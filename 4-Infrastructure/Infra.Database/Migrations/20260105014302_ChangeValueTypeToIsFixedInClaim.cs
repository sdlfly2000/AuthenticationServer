using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Database.Migrations
{
    /// <inheritdoc />
    public partial class ChangeValueTypeToIsFixedInClaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValueType",
                table: "Claim");

            migrationBuilder.AddColumn<bool>(
                name: "IsFixed",
                table: "Claim",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFixed",
                table: "Claim");

            migrationBuilder.AddColumn<string>(
                name: "ValueType",
                table: "Claim",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
