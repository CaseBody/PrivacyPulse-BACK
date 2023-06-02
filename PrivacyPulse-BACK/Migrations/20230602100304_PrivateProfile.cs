using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrivacyPulse_BACK.Migrations
{
    /// <inheritdoc />
    public partial class PrivateProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PrivateProfile",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrivateProfile",
                table: "Users");
        }
    }
}
