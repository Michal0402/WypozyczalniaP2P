using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static NuGet.Packaging.PackagingConstants;
using System.Security.Cryptography;
using System;
using WypozyczalniaP2P.Data;
using WypozyczalniaP2P.Models;
namespace WypozyczalniaP2P.Controllers
{
    public class WypozyczenieController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public WypozyczenieController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Wypozyczenie
        public async Task<IActionResult> Index()
        {
            var wypozyczenia = _context.Wypozyczenia
                .Include(w => w.Klient)
                .Include(w => w.Samochod)
                .Include(w => w.Wypozyczajacy);
            return View(await wypozyczenia.ToListAsync());
        }

        // GET: Wypozyczenie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var wypozyczenie = await _context.Wypozyczenia
                .Include(w => w.Klient)
                .Include(w => w.Samochod)
                .Include(w => w.Wypozyczajacy)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (wypozyczenie == null) return NotFound();

            return View(wypozyczenie);
        }

        // GET: Wypozyczenie/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.KlientId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email");
            ViewBag.SamochodId = new SelectList(await _context.Samochody.ToListAsync(), "Id", "Marka");
            ViewBag.WypozyczajacyId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email");
            return View(new Wypozyczenie { DataRozpoczecia = DateTime.Today }); // Domyślna data
        }

        // POST: Wypozyczenie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KlientId,SamochodId,WypozyczajacyId,DataRozpoczecia,DataZakonczenia")] Wypozyczenie wypozyczenie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wypozyczenie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.KlientId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", wypozyczenie.KlientId);
            ViewBag.SamochodId = new SelectList(await _context.Samochody.ToListAsync(), "Id", "Marka", wypozyczenie.SamochodId);
            ViewBag.WypozyczajacyId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", wypozyczenie.WypozyczajacyId);
            return View(wypozyczenie);
        }

        // GET: Wypozyczenie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var wypozyczenie = await _context.Wypozyczenia.FindAsync(id);
            if (wypozyczenie == null) return NotFound();

            ViewBag.KlientId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", wypozyczenie.KlientId);
            ViewBag.SamochodId = new SelectList(await _context.Samochody.ToListAsync(), "Id", "Marka", wypozyczenie.SamochodId);
            ViewBag.WypozyczajacyId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", wypozyczenie.WypozyczajacyId);
            return View(wypozyczenie);
        }

        // POST: Wypozyczenie/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KlientId,SamochodId,WypozyczajacyId,DataRozpoczecia,DataZakonczenia")] Wypozyczenie wypozyczenie)
        {
            if (id != wypozyczenie.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wypozyczenie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WypozyczenieExists(wypozyczenie.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.KlientId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", wypozyczenie.KlientId);
            ViewBag.SamochodId = new SelectList(await _context.Samochody.ToListAsync(), "Id", "Marka", wypozyczenie.SamochodId);
            ViewBag.WypozyczajacyId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", wypozyczenie.WypozyczajacyId);
            return View(wypozyczenie);
        }

        // GET: Wypozyczenie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var wypozyczenie = await _context.Wypozyczenia
                .Include(w => w.Klient)
                .Include(w => w.Samochod)
                .Include(w => w.Wypozyczajacy)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (wypozyczenie == null) return NotFound();

            return View(wypozyczenie);
        }

        // POST: Wypozyczenie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wypozyczenie = await _context.Wypozyczenia.FindAsync(id);
            if (wypozyczenie != null)
            {
                _context.Wypozyczenia.Remove(wypozyczenie);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool WypozyczenieExists(int id)
        {
            return _context.Wypozyczenia.Any(e => e.Id == id);
        }
    }
}
