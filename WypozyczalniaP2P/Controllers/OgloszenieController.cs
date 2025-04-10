using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WypozyczalniaP2P.Data;
using WypozyczalniaP2P.Models;

namespace WypozyczalniaP2P.Controllers
{
    public class OgloszenieController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OgloszenieController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Ogloszenie
        public async Task<IActionResult> Index()
        {
            var ogloszenia = _context.Ogłoszenia
                .Include(o => o.Klient)
                .Include(o => o.Samochod);
            return View(await ogloszenia.ToListAsync());
        }

        // GET: Ogloszenie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ogloszenie = await _context.Ogłoszenia
                .Include(o => o.Klient)
                .Include(o => o.Samochod)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ogloszenie == null) return NotFound();

            return View(ogloszenie);
        }

        // GET: Ogloszenie/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.KlientId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email");
            ViewBag.SamochodId = new SelectList(await _context.Samochody.ToListAsync(), "Id", "Marka");
            return View();
        }

        // POST: Ogloszenie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Tytul,Opis,KlientId,CenaZaDzien,SamochodId,DataUtworzenia")] Ogloszenie ogloszenie)
        {
            if (ModelState.IsValid)
            {
                ogloszenie.DataUtworzenia = DateTime.Now;

                _context.Add(ogloszenie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.KlientId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", ogloszenie.KlientId);
            ViewBag.SamochodId = new SelectList(await _context.Samochody.ToListAsync(), "Id", "Marka", ogloszenie.SamochodId);
            return View(ogloszenie);
        }

        // GET: Ogloszenie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ogloszenie = await _context.Ogłoszenia.FindAsync(id);
            if (ogloszenie == null) return NotFound();

            ViewBag.KlientId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", ogloszenie.KlientId);
            ViewBag.SamochodId = new SelectList(await _context.Samochody.ToListAsync(), "Id", "Marka", ogloszenie.SamochodId);
            return View(ogloszenie);
        }

        // POST: Ogloszenie/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tytul,Opis,KlientId,CenaZaDzien,SamochodId,DataUtworzenia")] Ogloszenie ogloszenie)
        {
            if (id != ogloszenie.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ogloszenie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OgloszenieExists(ogloszenie.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.KlientId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", ogloszenie.KlientId);
            ViewBag.SamochodId = new SelectList(await _context.Samochody.ToListAsync(), "Id", "Marka", ogloszenie.SamochodId);
            return View(ogloszenie);
        }

        // GET: Ogloszenie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var ogloszenie = await _context.Ogłoszenia
                .Include(o => o.Klient)
                .Include(o => o.Samochod)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ogloszenie == null) return NotFound();

            return View(ogloszenie);
        }

        // POST: Ogloszenie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ogloszenie = await _context.Ogłoszenia.FindAsync(id);
            if (ogloszenie != null)
            {
                _context.Ogłoszenia.Remove(ogloszenie);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool OgloszenieExists(int id)
        {
            return _context.Ogłoszenia.Any(e => e.Id == id);
        }
    }
}
