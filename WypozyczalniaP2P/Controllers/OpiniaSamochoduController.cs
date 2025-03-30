using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WypozyczalniaP2P.Data;
using WypozyczalniaP2P.Models;

namespace WypozyczalniaP2P.Controllers
{
    public class OpiniaSamochoduController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OpiniaSamochoduController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: OpiniaSamochodu
        public async Task<IActionResult> Index()
        {
            var opinie = _context.OpinieSamochodow
                .Include(o => o.Autor)
                .Include(o => o.Samochod);
            return View(await opinie.ToListAsync());
        }

        // GET: OpiniaSamochodu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var opiniaSamochodu = await _context.OpinieSamochodow
                .Include(o => o.Autor)
                .Include(o => o.Samochod)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (opiniaSamochodu == null) return NotFound();

            return View(opiniaSamochodu);
        }

        // GET: OpiniaSamochodu/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.AutorId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email");
            ViewBag.SamochodId = new SelectList(await _context.Samochody.ToListAsync(), "Id", "Marka");
            return View(new OpiniaSamochodu { DataDodania = DateTime.Now }); // Domyślna data
        }

        // POST: OpiniaSamochodu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AutorId,SamochodId,Ocena,Komentarz,DataDodania")] OpiniaSamochodu opiniaSamochodu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(opiniaSamochodu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.AutorId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", opiniaSamochodu.AutorId);
            ViewBag.SamochodId = new SelectList(await _context.Samochody.ToListAsync(), "Id", "Marka", opiniaSamochodu.SamochodId);
            return View(opiniaSamochodu);
        }

        // GET: OpiniaSamochodu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var opiniaSamochodu = await _context.OpinieSamochodow.FindAsync(id);
            if (opiniaSamochodu == null) return NotFound();

            ViewBag.AutorId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", opiniaSamochodu.AutorId);
            ViewBag.SamochodId = new SelectList(await _context.Samochody.ToListAsync(), "Id", "Marka", opiniaSamochodu.SamochodId);
            return View(opiniaSamochodu);
        }

        // POST: OpiniaSamochodu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AutorId,SamochodId,Ocena,Komentarz,DataDodania")] OpiniaSamochodu opiniaSamochodu)
        {
            if (id != opiniaSamochodu.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(opiniaSamochodu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OpiniaSamochoduExists(opiniaSamochodu.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.AutorId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", opiniaSamochodu.AutorId);
            ViewBag.SamochodId = new SelectList(await _context.Samochody.ToListAsync(), "Id", "Marka", opiniaSamochodu.SamochodId);
            return View(opiniaSamochodu);
        }

        // GET: OpiniaSamochodu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var opiniaSamochodu = await _context.OpinieSamochodow
                .Include(o => o.Autor)
                .Include(o => o.Samochod)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (opiniaSamochodu == null) return NotFound();

            return View(opiniaSamochodu);
        }

        // POST: OpiniaSamochodu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var opiniaSamochodu = await _context.OpinieSamochodow.FindAsync(id);
            if (opiniaSamochodu != null)
            {
                _context.OpinieSamochodow.Remove(opiniaSamochodu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool OpiniaSamochoduExists(int id)
        {
            return _context.OpinieSamochodow.Any(e => e.Id == id);
        }
    }
}
