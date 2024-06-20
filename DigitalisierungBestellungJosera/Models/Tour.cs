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
            get { return aktuellegewicht / 1000; }// berechnet den aktuellen Stellplatz ein Stellplatz entspricht 1000 kg
        }
        // Navigation

        public List<Bestellung> Bestellungen { get; set; } = new List<Bestellung>();
        
        public int aktuellegewichtberechnen()
        {
            aktuellegewicht = 0;

            // Überprüfung ob die Bestellungen vorhanden sind
            if (Bestellungen != null)
            {
                // Berechnung des Gewicht für jede Bestellung in der Tour
                foreach (var bestellung in Bestellungen)
                {
                    // Überprüfung ob die Positionen vorhanden sind
                    if (bestellung.Positionen != null)
                    {
                        // Berechnung des Gewicht für jede Position in der Bestellung
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
        
        // Überprüfung ob die maximalen Stellplätze erreicht sind
        public bool maxstellplatzerreicht()
        { 
            // Gleicht die aktuellen Stellplätze mit den maximalen Stellplätzen ab
            // gibt wahr oder falsch zurück
            return AktuellerStellplatz >= MaxStellplatz; 
        }



    }
}
