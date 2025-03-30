using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WypozyczalniaP2P.Data;
using WypozyczalniaP2P.Models;

namespace WypozyczalniaP2P.Controllers
{
    public class OpiniaKlientaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OpiniaKlientaController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: OpiniaKlienta
        public async Task<IActionResult> Index()
        {
            var opinie = _context.OpinieKlientow
                .Include(o => o.Autor)
                .Include(o => o.Klient);
            return View(await opinie.ToListAsync());
        }

        // GET: OpiniaKlienta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var opiniaKlienta = await _context.OpinieKlientow
                .Include(o => o.Autor)
                .Include(o => o.Klient)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (opiniaKlienta == null) return NotFound();

            return View(opiniaKlienta);
        }

        // GET: OpiniaKlienta/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.AutorId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email");
            ViewBag.KlientId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email");
            return View(new OpiniaKlienta { DataDodania = DateTime.Now }); // Domyślna data
        }

        // POST: OpiniaKlienta/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AutorId,KlientId,Ocena,Komentarz,DataDodania")] OpiniaKlienta opiniaKlienta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(opiniaKlienta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.AutorId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", opiniaKlienta.AutorId);
            ViewBag.KlientId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", opiniaKlienta.KlientId);
            return View(opiniaKlienta);
        }

        // GET: OpiniaKlienta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var opiniaKlienta = await _context.OpinieKlientow.FindAsync(id);
            if (opiniaKlienta == null) return NotFound();

            ViewBag.AutorId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", opiniaKlienta.AutorId);
            ViewBag.KlientId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", opiniaKlienta.KlientId);
            return View(opiniaKlienta);
        }

        // POST: OpiniaKlienta/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AutorId,KlientId,Ocena,Komentarz,DataDodania")] OpiniaKlienta opiniaKlienta)
        {
            if (id != opiniaKlienta.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(opiniaKlienta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OpiniaKlientaExists(opiniaKlienta.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.AutorId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", opiniaKlienta.AutorId);
            ViewBag.KlientId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", opiniaKlienta.KlientId);
            return View(opiniaKlienta);
        }

        // GET: OpiniaKlienta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var opiniaKlienta = await _context.OpinieKlientow
                .Include(o => o.Autor)
                .Include(o => o.Klient)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (opiniaKlienta == null) return NotFound();

            return View(opiniaKlienta);
        }

        // POST: OpiniaKlienta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var opiniaKlienta = await _context.OpinieKlientow.FindAsync(id);
            if (opiniaKlienta != null)
            {
                _context.OpinieKlientow.Remove(opiniaKlienta);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool OpiniaKlientaExists(int id)
        {
            return _context.OpinieKlientow.Any(e => e.Id == id);
        }
    }
}
