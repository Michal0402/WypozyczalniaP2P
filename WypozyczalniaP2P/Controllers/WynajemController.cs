using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WypozyczalniaP2P.Data;
using WypozyczalniaP2P.Models;

namespace WypozyczalniaP2P.Controllers
{
    public class WynajemController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public WynajemController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Wynajem
        public async Task<IActionResult> Index()
        {
            var wynajmy = _context.Wynajmy
                .Include(w => w.Klient)
                .Include(w => w.Samochod);
            return View(await wynajmy.ToListAsync());
        }

        // GET: Wynajem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var wynajem = await _context.Wynajmy
                .Include(w => w.Klient)
                .Include(w => w.Samochod)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (wynajem == null) return NotFound();

            return View(wynajem);
        }

        // GET: Wynajem/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.KlientId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email");
            ViewBag.SamochodId = new SelectList(await _context.Samochody.ToListAsync(), "Id", "Marka");
            return View(new Wynajem { DataRozpoczecia = DateTime.Today }); // Domyślna data rozpoczęcia
        }

        // POST: Wynajem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KlientId,SamochodId,DataRozpoczecia,DataZakonczenia,IloscDni,CalkowityKosztWynajmu")] Wynajem wynajem)
        {
            if (ModelState.IsValid)
            {
                // Opcjonalna logika obliczania IloscDni i CalkowityKosztWynajmu
                wynajem.IloscDni = (wynajem.DataZakonczenia - wynajem.DataRozpoczecia).Days;
                // Przykład: koszt na podstawie ceny za dzień z Ogloszenie (trzeba dostosować)
                var ogloszenie = await _context.Ogłoszenia.FirstOrDefaultAsync(o => o.SamochodId == wynajem.SamochodId);
                if (ogloszenie != null)
                {
                    wynajem.CalkowityKosztWynajmu = wynajem.IloscDni * ogloszenie.CenaZaDzien;
                }

                _context.Add(wynajem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.KlientId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", wynajem.KlientId);
            ViewBag.SamochodId = new SelectList(await _context.Samochody.ToListAsync(), "Id", "Marka", wynajem.SamochodId);
            return View(wynajem);
        }

        // GET: Wynajem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var wynajem = await _context.Wynajmy.FindAsync(id);
            if (wynajem == null) return NotFound();

            ViewBag.KlientId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", wynajem.KlientId);
            ViewBag.SamochodId = new SelectList(await _context.Samochody.ToListAsync(), "Id", "Marka", wynajem.SamochodId);
            return View(wynajem);
        }

        // POST: Wynajem/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KlientId,SamochodId,DataRozpoczecia,DataZakonczenia,IloscDni,CalkowityKosztWynajmu")] Wynajem wynajem)
        {
            if (id != wynajem.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Opcjonalna aktualizacja IloscDni i CalkowityKosztWynajmu
                    wynajem.IloscDni = (wynajem.DataZakonczenia - wynajem.DataRozpoczecia).Days;
                    var ogloszenie = await _context.Ogłoszenia.FirstOrDefaultAsync(o => o.SamochodId == wynajem.SamochodId);
                    if (ogloszenie != null)
                    {
                        wynajem.CalkowityKosztWynajmu = wynajem.IloscDni * ogloszenie.CenaZaDzien;
                    }

                    _context.Update(wynajem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WynajemExists(wynajem.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.KlientId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", wynajem.KlientId);
            ViewBag.SamochodId = new SelectList(await _context.Samochody.ToListAsync(), "Id", "Marka", wynajem.SamochodId);
            return View(wynajem);
        }

        // GET: Wynajem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var wynajem = await _context.Wynajmy
                .Include(w => w.Klient)
                .Include(w => w.Samochod)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (wynajem == null) return NotFound();

            return View(wynajem);
        }

        // POST: Wynajem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wynajem = await _context.Wynajmy.FindAsync(id);
            if (wynajem != null)
            {
                _context.Wynajmy.Remove(wynajem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool WynajemExists(int id)
        {
            return _context.Wynajmy.Any(e => e.Id == id);
        }
    }
}
