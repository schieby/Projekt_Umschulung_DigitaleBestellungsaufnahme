using System.ComponentModel.DataAnnotations;

namespace DigitalisierungBestellungJosera.Models
{
    public class Bestellung
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        public int KundeId { get; set; }
        [Display(Name = "Tour")]
        public int TourId { get; set; }
        public int BestellungsNr {  get; set; }
        public DateOnly Bestelldatum { get; set; }

        //Navigation-Properties
        public Kunde? Kunde { get; set; } 
        public Tour? Tour { get; set; }
        // Navigation
        public List<Position> Positionen { get; set; } = new List<Position>();
    }
}
