namespace DigitalisierungBestellungJosera.Models
{
    public class Kunde
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Vorname { get; set; }
        public string EMail { get; set; }
        public string Telefonnummer { get; set; }
        public int PLZ { get; set; }
        public string Ort { get; set; }
        public string Straße { get; set; }
        public int Nr { get; set; }

        //Navigation
        public List<Bestellung> Bestellung { get; set; } = new List<Bestellung>();
    }
}
