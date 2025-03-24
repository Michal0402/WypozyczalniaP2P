using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WypozyczalniaP2P.Data;
using WypozyczalniaP2P.Models;

namespace WypozyczalniaP2P.Controllers
{
    public class TypSamochoduController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TypSamochoduController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TypSamochodus
        public async Task<IActionResult> Index()
        {
            return View(await _context.TypySamochodow.ToListAsync());
        }

        // GET: TypSamochodus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typSamochodu = await _context.TypySamochodow
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typSamochodu == null)
            {
                return NotFound();
            }

            return View(typSamochodu);
        }

        // GET: TypSamochodus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypSamochodus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa,Opis")] TypSamochodu typSamochodu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typSamochodu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typSamochodu);
        }

        // GET: TypSamochodus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typSamochodu = await _context.TypySamochodow.FindAsync(id);
            if (typSamochodu == null)
            {
                return NotFound();
            }
            return View(typSamochodu);
        }

        // POST: TypSamochodus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa,Opis")] TypSamochodu typSamochodu)
        {
            if (id != typSamochodu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typSamochodu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypSamochoduExists(typSamochodu.Id))
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
            return View(typSamochodu);
        }

        // GET: TypSamochodus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typSamochodu = await _context.TypySamochodow
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typSamochodu == null)
            {
                return NotFound();
            }

            return View(typSamochodu);
        }

        // POST: TypSamochodus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var typSamochodu = await _context.TypySamochodow.FindAsync(id);
            if (typSamochodu != null)
            {
                _context.TypySamochodow.Remove(typSamochodu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypSamochoduExists(int id)
        {
            return _context.TypySamochodow.Any(e => e.Id == id);
        }
    }
}
