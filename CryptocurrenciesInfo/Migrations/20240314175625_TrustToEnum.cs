using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptocurrenciesInfo.Migrations
{
    /// <inheritdoc />
    public partial class TrustToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Trust",
                table: "CoinMarket",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Trust",
                table: "CoinMarket",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
