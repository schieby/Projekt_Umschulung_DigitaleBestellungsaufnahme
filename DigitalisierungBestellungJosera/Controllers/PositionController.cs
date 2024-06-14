using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DigitalisierungBestellungJosera.Data;
using DigitalisierungBestellungJosera.Models;

namespace DigitalisierungBestellungJosera.Controllers
{
    public class PositionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PositionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Position
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Position
                .Include(p => p.Bestellung)
                .ThenInclude(p=> p.Tour)
                .Include(p => p.Produkt)
                .Include(p => p.Bestellung.Kunde);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Position/Details/5
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Position
                .Include(p => p.Bestellung)
                .ThenInclude (p => p.Kunde)            
                .Include(p => p.Produkt)                
                .FirstOrDefaultAsync(m => m.Id == id);
            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        // GET: Position/Create
        public IActionResult Create()
        {
            ViewData["BestellungId"] = new SelectList(
        _context.Bestellung
        .Include(b => b.Kunde)
        .Select(b => new
        {
            b.Id,
            DisplayText = b.BestellungsNr + " | " + b.Kunde.Vorname + " " + b.Kunde.Name
        }),
        "Id",
        "DisplayText");


            ViewData["ProduktId"] = new SelectList(
       _context.Produkt
       //.Include(b => b.Gewicht_in_KG)
       .Select(b => new
       {
           b.Id,
           DisplayText = b.Name + " | " + b.Gewicht_in_KG + " Kg"
       }),
       "Id",
       "DisplayText");
            return View();
        }

        // POST: Position/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProduktId,BestellungId,Stückzahl")] Position position)
        {
            
            if (ModelState.IsValid)
            {
                // Laden Sie das Produkt, um das Gewicht zu überprüfen
                var produkt = await _context.Produkt.FindAsync(position.ProduktId);

                if (produkt == null)
                {
                    ModelState.AddModelError("", "Das ausgewählte Produkt existiert nicht.");
                    ViewData["BestellungId"] = new SelectList(
                        _context.Bestellung
                            .Include(b => b.Kunde)
                            .Select(b => new
                            {
                                b.Id,
                                DisplayText = b.BestellungsNr + " | " + b.Kunde.Vorname + " " + b.Kunde.Name
                            }),
                        "Id",
                        "DisplayText", position.BestellungId);
                    ViewData["ProduktId"] = new SelectList(
       _context.Produkt
       //.Include(b => b.Gewicht_in_KG)
       .Select(b => new
       {
           b.Id,
           DisplayText = b.Name + " | " + b.Gewicht_in_KG + " Kg"
       }),
       "Id",
       "DisplayText");
                    return View(position);
                }

                // Laden Sie die Bestellung, zu der diese Position gehört, einschließlich der Tour
                var bestellung = await _context.Bestellung
                    .Include(b => b.Tour)
                    .ThenInclude(p => p.Bestellungen)
                    .ThenInclude(b => b.Positionen)
                    .ThenInclude(p => p.Produkt)
                    .FirstOrDefaultAsync(b => b.Id == position.BestellungId);

                if (bestellung == null)
                {
                    return NotFound(); // Handhaben Sie den Fall, wenn die Bestellung nicht gefunden wird
                }

                // Berechnen Sie das aktuelle Gewicht der Tour
                int aktuellegewicht = bestellung.Tour.aktuellegewichtberechnen();
                

                // Überprüfen Sie, ob das aktuelle Gewicht das maximale Gewicht überschreitet
                if (aktuellegewicht + (produkt.Gewicht_in_KG * position.Stückzahl) > bestellung.Tour.MaxLadegewicht_in_KG)
                {
                    // Handhaben Sie den Fall, wenn das Gewicht das maximale Gewicht überschreitet
                    // Zum Beispiel, indem Sie eine Fehlermeldung anzeigen oder die Aktion abbrechen
                    ModelState.AddModelError("", "Das aktuelle Gewicht überschreitet das maximale Ladegewicht der Tour.");
                    ViewData["BestellungId"] = new SelectList(
                        _context.Bestellung
                            .Include(b => b.Kunde)
                            .Select(b => new
                            {
                                b.Id,
                                DisplayText = b.BestellungsNr + " | " + b.Kunde.Vorname + " " + b.Kunde.Name
                            }),
                        "Id",
                        "DisplayText", position.BestellungId);
                    ViewData["ProduktId"] = new SelectList(_context.Produkt, "Id", "Name", position.ProduktId);
                    return View(position);
                }

                // Überprüfen Sie, ob das maximale Stellplatzlimit erreicht ist
                if (bestellung.Tour.maxstellplatzerreicht())
                {
                    // Wenn das Limit erreicht ist, fügen Sie die Position trotzdem hinzu, aber zeigen Sie eine Meldung an
                    TempData["StellplatzLimitError"] = "Das maximale Stellplatzlimit wurde erreicht.";
                }

                // Fügen Sie die Position hinzu, wenn die Validierung erfolgreich ist
                _context.Add(position);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["BestellungId"] = new SelectList(
                _context.Bestellung
                .Include(b => b.Kunde)
                .Select(b => new
                {
                    b.Id,
                    DisplayText = b.BestellungsNr + " | " + b.Kunde.Vorname + " " + b.Kunde.Name
                }),
                "Id",
                "DisplayText", position.BestellungId);
            ViewData["ProduktId"] = new SelectList(_context.Produkt, "Id", "Name", position.ProduktId);
            return View(position);
        }

        // GET: Position/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Position.FindAsync(id);
            if (position == null)
            {
                return NotFound();
            }

            ViewData["BestellungId"] = new SelectList(
                _context.Bestellung
                    .Include(b => b.Kunde)
                    .Select(b => new
                    {
                        b.Id,
                        DisplayText = b.BestellungsNr + " | " + b.Kunde.Vorname + " " + b.Kunde.Name
                    }),
                "Id",
                "DisplayText",
                position.BestellungId  // Vorausgewählte BestellungId
            );

            ViewData["ProduktId"] = new SelectList(
                _context.Produkt
                    .Select(b => new
                    {
                        b.Id,
                        DisplayText = b.Name + " | " + b.Gewicht_in_KG + " Kg"
                    }),
                "Id",
                "DisplayText",
                position.ProduktId  // Vorausgewählte ProduktId
            );

            return View(position);
        }

        // POST: Position/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProduktId,BestellungId,Stückzahl")] Position position)
        {
            if (id != position.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var originalPosition = await _context.Position.FindAsync(id);
                    if (originalPosition == null)
                    {
                        return NotFound();
                    }
                    // Berechnen Sie das aktuelle Gewicht der Tour
                    var bestellung = await _context.Bestellung
                    .Include(b => b.Tour)
                    .ThenInclude(p => p.Bestellungen)
                    .ThenInclude(b => b.Positionen)
                    .ThenInclude(p => p.Produkt)
                    .FirstOrDefaultAsync(b => b.Id == position.BestellungId);

                    if (bestellung == null)
                    {
                        return NotFound(); // Handhaben Sie den Fall, wenn die Bestellung nicht gefunden wird
                    }

                    int aktuellegewicht = bestellung.Tour.aktuellegewichtberechnen();                
                    
                    // Überprüfen Sie, ob das aktuelle Gewicht das maximale Gewicht überschreitet
                    var produkt = await _context.Produkt.FindAsync(position.ProduktId);
                    if (produkt == null)
                    {
                        return NotFound(); // Handhaben Sie den Fall, wenn das Produkt nicht gefunden wird
                    }
                    // Anpassen des Aktuellengewichts auf basis der änderung der Stückzahl
                    int mengenänderung = position.Stückzahl - originalPosition.Stückzahl;
                    aktuellegewicht += mengenänderung * originalPosition.Produkt.Gewicht_in_KG;

                    if (aktuellegewicht > bestellung.Tour.MaxLadegewicht_in_KG)
                    {
                        // Handhaben Sie den Fall, wenn das Gewicht das maximale Gewicht überschreitet
                        // Zum Beispiel, indem Sie eine Fehlermeldung anzeigen oder die Aktion abbrechen
                        ModelState.AddModelError("", "Das aktuelle Gewicht überschreitet das maximale Ladegewicht der Tour.");
                        ViewData["BestellungId"] = new SelectList(
                            _context.Bestellung
                                .Include(b => b.Kunde)
                                .Select(b => new
                                {
                                    b.Id,
                                    DisplayText = b.BestellungsNr + " | " + b.Kunde.Vorname + " " + b.Kunde.Name
                                }),
                            "Id",
                            "DisplayText", position.BestellungId);
                        ViewData["ProduktId"] = new SelectList(_context.Produkt, "Id", "Name", position.ProduktId);
                        return View(position);
                    }
                    
                    // Berechnen Sie die neuen Stellplätze, indem Sie die aktuelle Position entfernen und die neue hinzufügen
                    int neueStellplätze = bestellung.Tour.Bestellungen.Sum(b =>
                        b.Positionen.Sum(p => p.Produkt.Gewicht_in_KG * p.Stückzahl / 1000));

                    neueStellplätze -= originalPosition.Produkt.Gewicht_in_KG * originalPosition.Stückzahl / 1000;
                    neueStellplätze += produkt.Gewicht_in_KG * position.Stückzahl / 1000;

                    // Prüfen, ob das Stellplatzlimit erreicht ist
                    if (neueStellplätze > bestellung.Tour.MaxStellplatz)
                    {
                        TempData["StellplatzLimitError"] = "Das maximale Stellplatzlimit wurde erreicht.";
                        ViewData["BestellungId"] = new SelectList(
                            _context.Bestellung
                                .Include(b => b.Kunde)
                                .Select(b => new
                                {
                                    b.Id,
                                    DisplayText = b.BestellungsNr + " | " + b.Kunde.Vorname + " " + b.Kunde.Name
                                }),
                            "Id",
                            "DisplayText", position.BestellungId);
                        ViewData["ProduktId"] = new SelectList(
                            _context.Produkt
                                .Select(p => new
                                {
                                    p.Id,
                                    DisplayText = p.Name + " | " + p.Gewicht_in_KG + " Kg"
                                }),
                            "Id",
                            "DisplayText", position.ProduktId);
                        return View(position);
                    }

                    // Änderungen an der Originalposition vornehmen und im Kontext aktualisieren
                    originalPosition.ProduktId = position.ProduktId;
                    originalPosition.Stückzahl = position.Stückzahl;
                    _context.Update(originalPosition);
                    await _context.SaveChangesAsync();                    
                
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PositionExists(position.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BestellungId"] = new SelectList(_context.Bestellung, "Id", "BestellungsNr", position.BestellungId);
            ViewData["ProduktId"] = new SelectList(
       _context.Produkt
       //.Include(b => b.Gewicht_in_KG)
       .Select(b => new
       {
           b.Id,
           DisplayText = b.Name + " | " + b.Gewicht_in_KG
       }),
       "Id",
       "DisplayText");
            return View(position);
        }

        // GET: Position/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Position
                .Include(p => p.Bestellung)
                .Include(p => p.Produkt)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        // POST: Position/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var position = await _context.Position
            .Include(p => p.Bestellung)
            .ThenInclude(b => b.Tour)
            .FirstOrDefaultAsync(p => p.Id == id);
            
            var bestellung = position.Bestellung;
            var tour = bestellung.Tour;
            // Entfernen der Position
            if (position != null)
            {
                _context.Position.Remove(position);
            }

            await _context.SaveChangesAsync();
            // Aktualisieren des aktuellen Gewichts der Tour
            int aktuellesGewicht = tour.aktuellegewichtberechnen();
            foreach (var pos in bestellung.Positionen)
            {
                aktuellesGewicht += pos.Produkt.Gewicht_in_KG * pos.Stückzahl;
            }
            tour.aktuellegewicht = aktuellesGewicht;
            // Speichern der Änderungen in der Datenbank
            _context.Update(tour);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool PositionExists(int id)
        {
            return _context.Position.Any(e => e.Id == id);
        }
    }
}
