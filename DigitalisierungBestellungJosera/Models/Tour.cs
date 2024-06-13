using DigitalisierungBestellungJosera.Data.Migrations;
using System.ComponentModel.DataAnnotations;

namespace DigitalisierungBestellungJosera.Models
{
    public class Tour
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateOnly Datum { get; set; }
        [Display(Name = "Maximales Ladegewicht (KG)")]
        public int MaxLadegewicht_in_KG { get; set; }
        [Display(Name = "Maximale Stellplätze")]
        public int MaxStellplatz { get; set; }
        [Display(Name = "aktuelles Gewicht")]
        public int aktuellegewicht { get; set; }
        [Display(Name = "belegte Stellplätze")]
        public int BelegteStellplätze { get; set; }
        [Display(Name = "aktuelle Stellplätze")]
        public int AktuellerStellplatz
        {
            get { return aktuellegewicht / 1000; }
        }
        // Navigation

        public List<Bestellung> Bestellungen { get; set; } = new List<Bestellung>();
        
        public int aktuellegewichtberechnen()
        {
            aktuellegewicht = 0;

            // Überprüfen Sie, ob Bestellungen vorhanden sind
            if (Bestellungen != null)
            {
                // Berechnen Sie das Gewicht für jede Bestellung in der Tour
                foreach (var bestellung in Bestellungen)
                {
                    // Überprüfen Sie, ob Positionen vorhanden sind
                    if (bestellung.Positionen != null)
                    {
                        // Berechnen Sie das Gewicht für jede Position in der Bestellung
                        foreach (var position in bestellung.Positionen)
                        {
                            // Produktgewicht * Stückzahl
                            aktuellegewicht += position.Produkt.Gewicht_in_KG * position.Stückzahl;
                        }
                    }
                }
            }   

            return aktuellegewicht;
        }
        
        public bool maxstellplatzerreicht()
        { 
            return AktuellerStellplatz >= MaxStellplatz; 
        }



    }
}
