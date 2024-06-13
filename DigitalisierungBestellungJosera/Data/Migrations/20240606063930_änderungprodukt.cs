using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalisierungBestellungJosera.Data.Migrations
{
    /// <inheritdoc />
    public partial class änderungprodukt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Preis_in_EURO",
                table: "Produkt",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Preis_in_EURO",
                table: "Produkt",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
