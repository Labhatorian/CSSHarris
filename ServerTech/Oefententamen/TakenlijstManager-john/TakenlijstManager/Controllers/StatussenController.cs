using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TakenlijstManager.Data;
using TakenlijstManager.Data.Entities;

namespace TakenlijstManager.Controllers
{
    public class StatussenController : Controller
    {
        private readonly TakenManagerDbContext _context;

        public StatussenController(TakenManagerDbContext context)
        {
            _context = context;
        }

        // GET: Statussen
        public async Task<IActionResult> Index()
        {
              return _context.Statussen != null ? 
                          View(await _context.Statussen.ToListAsync()) :
                          Problem("Entity set 'TakenManagerDbContext.Statussen'  is null.");
        }

        // GET: Statussen/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Statussen == null)
            {
                return NotFound();
            }

            var status = await _context.Statussen
                .FirstOrDefaultAsync(m => m.Id == id);
            if (status == null)
            {
                return NotFound();
            }

            return View(status);
        }

        // GET: Statussen/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Statussen/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,VolgendeStatus")] Status status)
        {
            if (ModelState.IsValid)
            {
                _context.Add(status);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(status);
        }

        // GET: Statussen/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Statussen == null)
            {
                return NotFound();
            }

            var status = await _context.Statussen.FindAsync(id);
            if (status == null)
            {
                return NotFound();
            }
            return View(status);
        }

        // POST: Statussen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,VolgendeStatus")] Status status)
        {
            if (id != status.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(status);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusExists(status.Id))
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
            return View(status);
        }

        // GET: Statussen/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Statussen == null)
            {
                return NotFound();
            }

            var status = await _context.Statussen
                .FirstOrDefaultAsync(m => m.Id == id);
            if (status == null)
            {
                return NotFound();
            }

            return View(status);
        }

        // POST: Statussen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Statussen == null)
            {
                return Problem("Entity set 'TakenManagerDbContext.Statussen'  is null.");
            }
            var status = await _context.Statussen.FindAsync(id);
            if (status != null)
            {
                _context.Statussen.Remove(status);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatusExists(int id)
        {
          return (_context.Statussen?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
