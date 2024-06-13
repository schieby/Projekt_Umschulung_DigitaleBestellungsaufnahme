using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalisierungBestellungJosera.Data.Migrations
{
    /// <inheritdoc />
    public partial class änderungposition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BestellNr",
                table: "Bestellung",
                newName: "BestellungsNr");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BestellungsNr",
                table: "Bestellung",
                newName: "BestellNr");
        }
    }
}
