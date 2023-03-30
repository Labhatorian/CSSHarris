using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TakenlijstManager.Data;
using TakenlijstManager.Data.Entities;

namespace TakenlijstManager.Controllers
{
    public class TakenController : Controller
    {
        private readonly TakenManagerDbContext _context;

        public TakenController(TakenManagerDbContext context)
        {
            _context = context;
        }

        // GET: Taken
        public async Task<IActionResult> Index(string zoekwaarde = "")
        {
            var sessionZoekwaardesAsString = this.HttpContext.Session.GetString("zoekwardes");

            if (sessionZoekwaardesAsString is null)
            {
                var zoekwaardes = new string[2];
                zoekwaardes[0] = "a";
                zoekwaardes[1] = "b";

                this.HttpContext.Session.SetString("zoekwardes", JsonSerializer.Serialize(zoekwaardes));
            } else
            {
               var zoekWaardesArray = JsonSerializer.Deserialize<string[]>(sessionZoekwaardesAsString);
            }

            if (zoekwaarde.Length > 0)
            {
                return _context.Taken != null ?
                          View(await _context.Taken.Where(t => t.Naam.Contains(zoekwaarde)).ToListAsync()) :
                          Problem("Entity set 'TakenManagerDbContext.Taken'  is null.");
            }
              return _context.Taken != null ? 
                          View(await _context.Taken.ToListAsync()) :
                          Problem("Entity set 'TakenManagerDbContext.Taken'  is null.");
        }

        // GET: Taken/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Taken == null)
            {
                return NotFound();
            }

            var taak = await _context.Taken
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taak == null)
            {
                return NotFound();
            }

            return View(taak);
        }

        // GET: Taken/Create
        public IActionResult Create()
        {
            ViewBag.Statussen = new SelectList(_context.Statussen.Select(x => new { Id = x.Id, Name = x.Name }).ToList(), "Id", "Name");
            this.HttpContext.Session.SetString("time", DateTime.Now.TimeOfDay.ToString());
            return View();
        }

        // POST: Taken/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naam,Omvang,Prioriteit,StatusId")] Taak taak)
        {
            // this.HttpContext.Session.SetString("time", DateTime.Now.TimeOfDay.ToString());
            var time = this.HttpContext.Session.GetString("time");

            if (ModelState.IsValid)
            {
                _context.Add(taak);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taak);
        }

        // GET: Taken/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Taken == null)
            {
                return NotFound();
            }

            var taak = await _context.Taken.FindAsync(id);
            if (taak == null)
            {
                return NotFound();
            }
            return View(taak);
        }

        // POST: Taken/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naam,Omvang,Prioriteit")] Taak taak)
        {
            if (id != taak.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taak);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaakExists(taak.Id))
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
            return View(taak);
        }

        // GET: Taken/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Taken == null)
            {
                return NotFound();
            }

            var taak = await _context.Taken
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taak == null)
            {
                return NotFound();
            }

            return View(taak);
        }

        // POST: Taken/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Taken == null)
            {
                return Problem("Entity set 'TakenManagerDbContext.Taken'  is null.");
            }
            var taak = await _context.Taken.FindAsync(id);
            if (taak != null)
            {
                _context.Taken.Remove(taak);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaakExists(int id)
        {
          return (_context.Taken?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
