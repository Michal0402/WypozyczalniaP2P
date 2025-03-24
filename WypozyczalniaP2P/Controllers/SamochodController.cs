using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WypozyczalniaP2P.Data;
using WypozyczalniaP2P.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WypozyczalniaP2P.Controllers
{
    public class SamochodController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SamochodController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Samochod/Index
        public async Task<IActionResult> Index()
        {
            var samochody = await _context.Samochody
                .Include(s => s.TypSamochodu)
                .Include(s => s.Wlasciciel)
                .ToListAsync();
            return View(samochody);
        }

        // GET: Samochod/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var samochod = await _context.Samochody
                .Include(s => s.TypSamochodu)
                .Include(s => s.Wlasciciel)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (samochod == null)
            {
                return NotFound();
            }

            return View(samochod);
        }

        // GET: Samochod/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.TypSamochoduId = new SelectList(await _context.TypySamochodow.ToListAsync(), "Id", "Nazwa");
            ViewBag.WlascicielId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email");
            return View();
        }

        // POST: Samochod/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Marka,Model,TypSamochoduId,NumerRejestracyjny,RokProdukcji,Przebieg,MocSilnika,PojemnoscSilnika,RodzajPaliwa,Skrzynia,LiczbaMiejsc,IloscDrzwi,RodzajNapedu,Vin,Kolor,WlascicielId,CzyDostepny")] Samochod samochod)
        {
            if (ModelState.IsValid)
            {
                _context.Add(samochod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TypSamochoduId = new SelectList(await _context.TypySamochodow.ToListAsync(), "Id", "Nazwa", samochod.TypSamochoduId);
            ViewBag.WlascicielId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", samochod.WlascicielId);
            return View(samochod);
        }

        // GET: Samochod/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var samochod = await _context.Samochody.FindAsync(id);
            if (samochod == null)
            {
                return NotFound();
            }

            ViewBag.TypSamochoduId = new SelectList(await _context.TypySamochodow.ToListAsync(), "Id", "Nazwa", samochod.TypSamochoduId);
            ViewBag.WlascicielId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", samochod.WlascicielId);
            return View(samochod);
        }

        // POST: Samochod/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Marka,Model,TypSamochoduId,NumerRejestracyjny,RokProdukcji,Przebieg,MocSilnika,PojemnoscSilnika,RodzajPaliwa,Skrzynia,LiczbaMiejsc,IloscDrzwi,RodzajNapedu,Vin,Kolor,WlascicielId,CzyDostepny")] Samochod samochod)
        {
            if (id != samochod.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(samochod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SamochodExists(samochod.Id))
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

            ViewBag.TypSamochoduId = new SelectList(await _context.TypySamochodow.ToListAsync(), "Id", "Nazwa", samochod.TypSamochoduId);
            ViewBag.WlascicielId = new SelectList(await _userManager.GetUsersInRoleAsync("Klient"), "Id", "Email", samochod.WlascicielId);
            return View(samochod);
        }

        // GET: Samochod/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var samochod = await _context.Samochody
                .Include(s => s.TypSamochodu)
                .Include(s => s.Wlasciciel)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (samochod == null)
            {
                return NotFound();
            }

            return View(samochod);
        }

        // POST: Samochod/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var samochod = await _context.Samochody.FindAsync(id);
            if (samochod != null)
            {
                _context.Samochody.Remove(samochod);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SamochodExists(int id)
        {
            return _context.Samochody.Any(e => e.Id == id);
        }
    }
}
