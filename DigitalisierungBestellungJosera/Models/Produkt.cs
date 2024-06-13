using System.ComponentModel.DataAnnotations;

namespace DigitalisierungBestellungJosera.Models
{
    public class Produkt
    {
        public int Id { get; set; }
        public string ChargeNr { get; set; }
        public string Name { get; set; }
        [Display(Name = "Preis in Euro")]
        public string Preis_in_EURO { get; set; }
        [Display(Name = "Gewicht (KG)")]
        public int Gewicht_in_KG { get; set; }

        //Navigation
        public List<Position> Positionen { get; set; } = new List<Position>();
    }
}
