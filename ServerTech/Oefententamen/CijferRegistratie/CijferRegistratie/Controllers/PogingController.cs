using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CijferRegistratie.Data;
using CijferRegistratie.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.NetworkInformation;

namespace CijferRegistratie.Controllers
{
    public class PogingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PogingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Poging
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Pogingen.Include(p => p.Vak);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Poging/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pogingen == null)
            {
                return NotFound();
            }

            var poging = await _context.Pogingen
                .Include(p => p.Vak)
                .FirstOrDefaultAsync(m => m.PogingId == id);
            if (poging == null)
            {
                return NotFound();
            }

            return View(poging);
        }

        // GET: Poging/Create
        public IActionResult Create()
        {
            ViewData["VakId"] = new SelectList(_context.Vakken, "VakId", "Naam");
            return View();
        }

        // POST: Poging/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PogingId,Jaar,Resultaat,VakId")] Poging poging)
        {
            if (ModelState.IsValid)
            {
                using HttpClient client = new();
                poging.StudentType = await client.GetStringAsync("https://localhost:7233/StudentType");

                _context.Add(poging);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["VakId"] = new SelectList(_context.Vakken, "VakId", "Naam", poging.VakId);
            return View(poging);
        }

        // GET: Poging/CreateSpecial
        [HttpGet]
        public IActionResult CreateSpecial(int id)
        {
            ViewData["VakId"] = id;
            ViewData["VakNaam"] = _context.Vakken.FirstOrDefault(x => x.VakId == id).Naam;
            return View();
        }

        // POST: Poging/CreateSpecial
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSpecial([Bind("PogingId,Jaar,Resultaat,VakId")] Poging poging)
        {
            if (ModelState.IsValid)
            {
                if (_context.Pogingen.Where(p => p.VakId == poging.VakId).Where(p => p.Resultaat > poging.Resultaat).FirstOrDefault() is null)
                {
                    using HttpClient client = new();
                    poging.StudentType = await client.GetStringAsync("https://localhost:7233/StudentType");

                    _context.Add(poging);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewData["VakId"] = poging.VakId;
            ViewData["VakNaam"] = _context.Vakken.FirstOrDefault(x => x.VakId == poging.VakId).Naam;
            return View(poging);
        }

        // GET: Poging/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pogingen == null)
            {
                return NotFound();
            }

            var poging = await _context.Pogingen.FindAsync(id);
            if (poging == null)
            {
                return NotFound();
            }
            ViewData["VakId"] = new SelectList(_context.Vakken, "VakId", "Naam", poging.VakId);
            return View(poging);
        }

        // POST: Poging/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PogingId,Jaar,Resultaat,VakId")] Poging poging)
        {
            if (id != poging.PogingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(poging);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PogingExists(poging.PogingId))
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
            ViewData["VakId"] = new SelectList(_context.Vakken, "VakId", "Naam", poging.VakId);
            return View(poging);
        }

        // GET: Poging/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pogingen == null)
            {
                return NotFound();
            }

            var poging = await _context.Pogingen
                .Include(p => p.Vak)
                .FirstOrDefaultAsync(m => m.PogingId == id);
            if (poging == null)
            {
                return NotFound();
            }

            return View(poging);
        }

        // POST: Poging/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pogingen == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Pogingen'  is null.");
            }
            var poging = await _context.Pogingen.FindAsync(id);
            if (poging != null)
            {
                _context.Pogingen.Remove(poging);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PogingExists(int id)
        {
          return (_context.Pogingen?.Any(e => e.PogingId == id)).GetValueOrDefault();
        }
    }
}
