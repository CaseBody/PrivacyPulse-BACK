using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrivacyPulse_BACK.Migrations
{
    /// <inheritdoc />
    public partial class fixDbCollation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER DATABASE PrivacyPulseDB COLLATE Latin1_General_CS_AS;", true);

            migrationBuilder.Sql(@$"            
                ALTER TABLE Users ALTER COLUMN Username nvarchar(max) COLLATE Latin1_General_CS_AS NOT NULL
                ALTER TABLE Users ALTER COLUMN Biography nvarchar(max) COLLATE Latin1_General_CS_AS NOT NULL
                ALTER TABLE Users ALTER COLUMN EncryptedPrivateKey nvarchar(max) COLLATE Latin1_General_CS_AS NOT NULL
                ALTER TABLE Users ALTER COLUMN PublicKey nvarchar(max) COLLATE Latin1_General_CS_AS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
