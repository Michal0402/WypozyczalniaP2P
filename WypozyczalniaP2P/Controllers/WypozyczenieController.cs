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
using WypozyczalniaP2P.Services;

namespace WypozyczalniaP2P.Controllers
{
    public class WypozyczenieController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly CarAvailabilityService _availabilityService;

        public WypozyczenieController(ApplicationDbContext context, UserManager<IdentityUser> userManager, CarAvailabilityService availabilityService)
        {
            _context = context;
            _userManager = userManager;
            _availabilityService = availabilityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOccupiedDates(int carId)
        {
            var occupiedEvents = await _context.Wypozyczenia
                .Where(r => r.SamochodId == carId)
                .Select(r => new
                {
                    Start = r.DataRozpoczecia,
                    End = r.DataZakonczenia
                })
                .ToListAsync();

            var result = occupiedEvents.Select(e => new
            {
                start = e.Start.ToString("yyyy-MM-dd"),
                end = e.End.HasValue ? e.End.Value.AddDays(1).ToString("yyyy-MM-dd") : DateTime.MaxValue.ToString("yyyy-MM-dd")
            });

            return Json(result);
        }

        public IActionResult PrepareWypozyczSamochod(int samochodId, int ogloszenieId)
        {
            var ogloszenie = _context.Ogłoszenia.FirstOrDefault(o => o.Id == ogloszenieId && o.SamochodId == samochodId);
            if (ogloszenie == null)
            {
                return NotFound();
            }

            TempData["SamochodId"] = samochodId;
            TempData["OgloszenieId"] = ogloszenieId;
            TempData["CenaZaDzien"] = ogloszenie.CenaZaDzien.ToString(System.Globalization.CultureInfo.InvariantCulture);
            return RedirectToAction("WypozyczSamochod");
        }

        public async Task<IActionResult> WypozyczSamochod()
        {
            if (!TempData.ContainsKey("SamochodId") || !TempData.ContainsKey("OgloszenieId"))
            {
                return NotFound();
            }

            int samochodId = (int)TempData["SamochodId"];
            int ogloszenieId = (int)TempData["OgloszenieId"];
            if (!decimal.TryParse(TempData["CenaZaDzien"]?.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal cenaZaDzien))
            {
                return BadRequest("Nieprawidłowy format ceny za dzień.");
            }

            if (samochodId == null) return NotFound();

            var samochod = await _context.Samochody.FindAsync(samochodId);
            if (samochod == null) return NotFound();

            var model = new Wypozyczenie
            {
                SamochodId = samochodId,
                KlientId = _userManager.GetUserId(User), // ID zalogowanego klienta
                WypozyczajacyId = samochod.WlascicielId, // ID właściciela samochodu
                DataRozpoczecia = DateTime.Today
            };

            ViewBag.CenaZaDzien = cenaZaDzien;
            ViewBag.OgloszenieId = ogloszenieId;
            ViewBag.Samochod = samochod;
            return View(new Wypozyczenie { SamochodId = samochodId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WypozyczSamochod([Bind("SamochodId,KlientId,WypozyczajacyId,DataRozpoczecia,DataZakonczenia")] Wypozyczenie wynajem)
        {
            var samochod = await _context.Samochody.FindAsync(wynajem.SamochodId);
            if (samochod == null)
            {
                ModelState.AddModelError("", "Wybrany samochód nie jest dostępny.");
            }

            // Walidacja dostępności za pomocą CarAvailabilityService
            if (!await _availabilityService.IsCarAvailable(wynajem.SamochodId, wynajem.DataRozpoczecia, wynajem.DataZakonczenia ?? DateTime.MaxValue))
            {
                ModelState.AddModelError("", "Samochód jest już zarezerwowany w wybranym okresie.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(wynajem);
                _context.Update(samochod);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Samochod");
            }

            ViewBag.Samochod = samochod;
            return View(wynajem);
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
