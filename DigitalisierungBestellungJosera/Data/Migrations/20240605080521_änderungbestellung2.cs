using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalisierungBestellungJosera.Data.Migrations
{
    /// <inheritdoc />
    public partial class änderungbestellung2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BestellNr",
                table: "Bestellung",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BestellNr",
                table: "Bestellung");
        }
    }
}
