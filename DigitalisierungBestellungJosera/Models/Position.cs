using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalisierungBestellungJosera.Models
{
    public class Position
    {
        public int Id { get; set; }
        [Display(Name = "Produkt")]
        public int ProduktId { get; set; }
        [Display(Name = "BestellNr")]
        public int BestellungId { get; set; }
        public int Stückzahl {  get; set; }

        //Navigation-Properties
        public Bestellung? Bestellung { get; set; }
        public Produkt? Produkt { get; set; }
        // Positionsnummer (PositionsNr)
        [NotMapped] // Nicht in der Datenbank speichern
        public int PositionsNr { get; set; }
    }
}
