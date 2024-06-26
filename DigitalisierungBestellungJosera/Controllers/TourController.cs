﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DigitalisierungBestellungJosera.Data;
using DigitalisierungBestellungJosera.Models;
using DigitalisierungBestellungJosera.Data.Migrations;

namespace DigitalisierungBestellungJosera.Controllers
{
    public class TourController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TourController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tour
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Tour.ToListAsync());
        //}
        public async Task<IActionResult> Index()
        {
            var tours = await _context.Tour
                .Include(b => b.Bestellungen)
                .ThenInclude(b => b.Positionen)
                .ThenInclude(b => b.Produkt)
                .ToListAsync();

            foreach (var tour in tours)
            {
                tour.aktuellegewicht = tour.aktuellegewichtberechnen();
            }

            return View(tours);
        }


        // GET: Tour/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _context.Tour
                .Include(b => b.Bestellungen)
                .ThenInclude(b => b.Kunde)
                .Include(b => b.Bestellungen)
                .ThenInclude(b => b.Positionen)
                .ThenInclude(b => b.Produkt)

                .FirstOrDefaultAsync(m => m.ID == id);
            if (tour == null)
            {
                return NotFound();
            }

            return View(tour);
        }

        // GET: Tour/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tour/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Datum,MaxLadegewicht_in_KG,MaxStellplatz")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tour);
        }

        // GET: Tour/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _context.Tour.FindAsync(id);
            if (tour == null)
            {
                return NotFound();
            }
            return View(tour);
        }

        // POST: Tour/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Datum,MaxLadegewicht_in_KG,MaxStellplatz")] Tour tour)
        {
            if (id != tour.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Laden der Tour mit allen relevanten Beziehungen
                    var existingTour = await _context.Tour
                        .Include(t => t.Bestellungen)
                            .ThenInclude(b => b.Positionen)
                                .ThenInclude(p => p.Produkt)
                        .FirstOrDefaultAsync(t => t.ID == id);

                    if (existingTour == null)
                    {
                        return NotFound();
                    }

                    int aktuellesGewicht = existingTour.aktuellegewichtberechnen();

                    if (aktuellesGewicht > tour.MaxLadegewicht_in_KG)
                    {
                        ModelState.AddModelError("", $"Das aktuelle Gewicht der Tour ({aktuellesGewicht} kg) " +
                            $"überschreitet das neue maximale Ladegewicht ({tour.MaxLadegewicht_in_KG} kg). " +
                            $"Bitte erhöhen Sie das maximale Ladegewicht oder entfernen Sie einige Bestellungen.");
                        return View(tour);
                    }

                    // Aktualisieren der Tour-Instanz
                    existingTour.Name = tour.Name;
                    existingTour.Datum = tour.Datum;
                    existingTour.MaxLadegewicht_in_KG = tour.MaxLadegewicht_in_KG;
                    existingTour.MaxStellplatz = tour.MaxStellplatz;

                    _context.Update(existingTour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TourExists(id))
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
            return View(tour);
        }
               

        // GET: Tour/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _context.Tour.FirstOrDefaultAsync(m => m.ID == id);

            if (tour == null)
            {
                return NotFound();
            }

            return View(tour);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tour = await _context.Tour.FindAsync(id);

            if (tour != null)
            {
                _context.Tour.Remove(tour);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TourExists(int id)
        {
            return _context.Tour.Any(e => e.ID == id);
        }
    }
}
