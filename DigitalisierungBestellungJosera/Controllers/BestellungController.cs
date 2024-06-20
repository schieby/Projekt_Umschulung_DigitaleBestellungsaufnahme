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
    public class BestellungController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BestellungController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bestellung

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Bestellung
                .Include(b => b.Kunde)
                .Include(b => b.Tour);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Bestellung/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bestellung = await _context.Bestellung
                .Include(b => b.Kunde)
                .Include(b => b.Tour)
                .Include(b => b.Positionen)
                .ThenInclude (b => b.Produkt)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bestellung == null)
            {
                return NotFound();
            }

            return View(bestellung);
        }
        
        // GET: Bestellung/Create
        public IActionResult Create()
        {
            // Die ViewData["KundeId"] wird mit einer SelectListe die Kundeninformationen enthält vorbereitet
            // Das Dropdownmenü im View wird mit dieser Liste gefüllt
            ViewData["KundeId"] = new SelectList(
        _context.Kunde.Select(k => new
        {
            k.Id,
            FullName = k.Vorname + " " + k.Name + " | " + k.PLZ + " " + k.Ort + " " + k.Straße + " " + k.Nr
            // Verbindung des Namen und der Adresse im Format:
            // Vorname Nachname | PLZ Ort Straße Nr
        }),
        "Id", "FullName");// Was im Dropdownmenu angezeigt wird

            // Die ViewData["TourId"] wird mit einer SelectListe die Tourinformationen enthält vorbereitet
            // Das Dropdownmenü im View wird mit dieser Liste gefüllt
            ViewData["TourId"] = new SelectList(_context.Tour, "ID", "Name");
            // Rückgabe der View zu Erstellung einer neuen Bestellung
            return View();
        }

        // POST: Bestellung/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KundeId,TourId,BestellungsNr,Bestelldatum")] Bestellung bestellung)
        {
            // Überprüfen der gültigkeit der Modelldaten
            if (ModelState.IsValid)
            {
                //Hinzufügen des datenkontext zur Bestellung
                _context.Add(bestellung);
                // Asynchrones Speicher der Änderung in der Datenbank
                await _context.SaveChangesAsync();
                // Umleitung auf die Index View wenn das Speicher erfolgreich ist
                return RedirectToAction(nameof(Index));
            }
            // Bei ungültigem Modell wird die View erneut angezeigt
            // das Dropdownmenü wird mit den bestehenden Werten gefüllt 
            ViewData["KundeId"] = new SelectList(
        _context.Kunde.Select(k => new
        {
            k.Id,
            FullName = k.Vorname + " " + k.Name + " | " + k.PLZ + " " + k.Ort + " " + k.Straße + " " + k.Nr
            // Verbindung des Namen und der Adresse im Format:
            // Vorname Nachname | PLZ Ort Straße Nr
        }),
        "Id","FullName", // Was im Dropdownmenu angezeigt wird
        bestellung.KundeId);
            ViewData["TourId"] = new SelectList(_context.Tour, "ID", "Name", bestellung.TourId);
            // Es wird die Bestellungs View zurückgegeben
            return View(bestellung);
        }

        // GET: Bestellung/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bestellung = await _context.Bestellung.FindAsync(id);
            if (bestellung == null)
            {
                return NotFound();
            }
            ViewData["KundeId"] = new SelectList(
        _context.Kunde.Select(k => new
        {
            k.Id,
            FullName = k.Vorname + " " + k.Name + " | " + k.PLZ + " " + k.Ort + " " + k.Straße + " " + k.Nr
        }),
        "Id", "FullName");
            ViewData["TourId"] = new SelectList(_context.Tour, "ID", "Name", bestellung.TourId);
            return View(bestellung);
        }

        // POST: Bestellung/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KundeId,TourId,BestellungsNr,Bestelldatum")] Bestellung bestellung)
        {
            if (id != bestellung.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bestellung);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BestellungExists(bestellung.Id))
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
            ViewData["KundeId"] = new SelectList(
        _context.Kunde.Select(k => new
        {
            k.Id,
            FullName = k.Vorname + " " + k.Name + " | " + k.PLZ + " " + k.Ort + " " + k.Straße + " " + k.Nr
        }),
        "Id", "FullName");
            ViewData["TourId"] = new SelectList(_context.Tour, "ID", "Name", bestellung.TourId);
            return View(bestellung);
        }

        // GET: Bestellung/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bestellung = await _context.Bestellung
                .Include(b => b.Kunde)
                .Include(b => b.Tour)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bestellung == null)
            {
                return NotFound();
            }

            return View(bestellung);
        }
       

        // POST: Bestellung/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bestellung = await _context.Bestellung.FindAsync(id);
            if (bestellung != null)
            {
                _context.Bestellung.Remove(bestellung);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BestellungExists(int id)
        {
            return _context.Bestellung.Any(e => e.Id == id);
        }
    }
}
