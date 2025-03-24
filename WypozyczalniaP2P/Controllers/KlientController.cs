using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WypozyczalniaP2P.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WypozyczalniaP2P.Controllers
{
    public class KlientController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public KlientController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Klient/Index
        public async Task<IActionResult> Index()
        {
            var identityUsers = await _userManager.GetUsersInRoleAsync("Klient"); // IList<IdentityUser>
            var klienci = identityUsers.OfType<Klient>().ToList();
            return View(klienci);
        }

        // GET: Klient/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null || !await _userManager.IsInRoleAsync(user, "Klient"))
            {
                return NotFound();
            }

            var klient = user as Klient; // Rzutowanie na Klient
            return View(klient);
        }

        // GET: Klient/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Klient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Imie,Nazwisko,NumerPrawaJazdy,Adres,Miejscowosc,KodPocztowy,DataUrodzenia,Email")] Klient klient, string password)
        {
            if (ModelState.IsValid)
            {
                klient.UserName = klient.Email; // Ustawienie UserName na Email
                var result = await _userManager.CreateAsync(klient, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(klient, "Klient");
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(klient);
        }

        // GET: Klient/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null || !await _userManager.IsInRoleAsync(user, "Klient"))
            {
                return NotFound();
            }

            var klient = user as Klient;
            return View(klient);
        }

        // POST: Klient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Imie,Nazwisko,NumerPrawaJazdy,Adres,Miejscowosc,KodPocztowy,DataUrodzenia,Email")] Klient klient)
        {
            if (id != klient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null || !await _userManager.IsInRoleAsync(user, "Klient"))
                {
                    return NotFound();
                }

                var existingKlient = user as Klient;
                existingKlient.Imie = klient.Imie;
                existingKlient.Nazwisko = klient.Nazwisko;
                existingKlient.NumerPrawaJazdy = klient.NumerPrawaJazdy;
                existingKlient.Adres = klient.Adres;
                existingKlient.Miejscowosc = klient.Miejscowosc;
                existingKlient.KodPocztowy = klient.KodPocztowy;
                existingKlient.DataUrodzenia = klient.DataUrodzenia;
                existingKlient.Email = klient.Email;
                existingKlient.UserName = klient.Email;

                var result = await _userManager.UpdateAsync(existingKlient);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(klient);
        }

        // GET: Klient/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null || !await _userManager.IsInRoleAsync(user, "Klient"))
            {
                return NotFound();
            }

            var klient = user as Klient;
            return View(klient);
        }

        // POST: Klient/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && await _userManager.IsInRoleAsync(user, "Klient"))
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
