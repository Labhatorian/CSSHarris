using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NerdyGadgets.Models;

namespace NerdyGadgets.Data
{
    public class ProductCategoriesController : Controller
    {
        private readonly NerdyGadgetsDbContext _context;

        public ProductCategoriesController(NerdyGadgetsDbContext context)
        {
            _context = context;
        }

        // GET: ProductCategories
        public async Task<IActionResult> Index()
        {
            var nerdyGadgetsDbContext = _context.ProductsCategories.Include(p => p.Category).Include(p => p.Product);
            return View(await nerdyGadgetsDbContext.ToListAsync());
        }

        // GET: ProductCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductsCategories == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductsCategories
                .Include(p => p.Category)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
        }

        // GET: ProductCategories/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryNumber", "Name");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Title");
            return View();
        }

        // POST: ProductCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,CategoryId")] ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryNumber", "Name", productCategory.CategoryId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Title", productCategory.ProductId);
            return View(productCategory);
        }

        // GET: ProductCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductsCategories == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductsCategories.FirstOrDefaultAsync(p => p.ProductId == 1 && p.CategoryId == 1);
            if (productCategory == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryNumber", "Name", productCategory.CategoryId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Title", productCategory.ProductId);
            return View(productCategory);
        }

        // POST: ProductCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,CategoryId")] ProductCategory productCategory)
        {
            if (id != productCategory.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCategoryExists(productCategory.ProductId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryNumber", "Name", productCategory.CategoryId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Title", productCategory.ProductId);
            return View(productCategory);
        }

        // GET: ProductCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductsCategories == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductsCategories
                .Include(p => p.Category)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
        }

        // POST: ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductsCategories == null)
            {
                return Problem("Entity set 'NerdyGadgetsDbContext.ProductsCategories'  is null.");
            }
            var productCategory = await _context.ProductsCategories.FindAsync(id);
            if (productCategory != null)
            {
                _context.ProductsCategories.Remove(productCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductCategoryExists(int id)
        {
          return (_context.ProductsCategories?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
