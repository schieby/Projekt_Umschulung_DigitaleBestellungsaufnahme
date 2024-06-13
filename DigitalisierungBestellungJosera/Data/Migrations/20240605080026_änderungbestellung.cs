using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalisierungBestellungJosera.Data.Migrations
{
    /// <inheritdoc />
    public partial class änderungbestellung : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gesamtgewicht_in_KG",
                table: "Bestellung");

            migrationBuilder.DropColumn(
                name: "Menge",
                table: "Bestellung");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Gesamtgewicht_in_KG",
                table: "Bestellung",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Menge",
                table: "Bestellung",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
