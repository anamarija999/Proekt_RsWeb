using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ElexirApp_RSWEB.Data;
using ElexirApp_RSWEB.Models;
using ElexirApp_RSWEB.ViewModel;

namespace ElexirApp_RSWEB.Controllers
{
    public class RezervacijasController : Controller
    {
        private readonly ElexirApp_RSWEBContext _context;

        public RezervacijasController(ElexirApp_RSWEBContext context)
        {
            _context = context;
        }

        // GET: Rezervacijas

        [Authorize(Roles = "Admin,Vraboten")]
        public async Task<IActionResult> Index(string usluga)
        {
            IQueryable<Rezervacija> rezervacija = _context.Rezervacija.AsQueryable();
            IQueryable<string> uslugazapishana = _context.Usluga.OrderBy(m => m.Benefits).Select(m => m.Benefits).Distinct();
            

            if (!string.IsNullOrEmpty(usluga))
            {
                rezervacija = rezervacija.Where(s => s.Usluga.Benefits.Contains(usluga));
            }
          
            rezervacija = rezervacija.Include(r => r.Korisnik).Include(r => r.Usluga);
            var VM = new RezervacijaNaUsluga
            {
                Rezervacija = await rezervacija.ToListAsync(),
                UslugaMesto = new SelectList(await uslugazapishana.ToListAsync())
            };

            return View(VM);
        }


        // GET: Rezervacijas/Create

        [Authorize(Roles = "Korisnik,Admin")]
        public IActionResult Create()
        {
            ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "Id", "Id");
            ViewData["UslugaId"] = new SelectList(_context.Usluga, "Id", "Id");
            return View();
        }

        // POST: Rezervacijas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Korisnik,Admin")]
        public async Task<IActionResult> Create([Bind("Id,UslugaId,KorisnikId")] Rezervacija rezervacija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rezervacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "Id", "Id", rezervacija.KorisnikId);
            ViewData["UslugaId"] = new SelectList(_context.Usluga, "Id", "Id", rezervacija.UslugaId);
            return View(rezervacija);
        }

        // GET: Rezervacijas/Edit/5

        [Authorize(Roles = "Vraboten,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacii = await _context.Rezervacija.FindAsync(id);
            if (rezervacii == null)
            {
                return NotFound();
            }
            ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "Id", "FullName", rezervacii.KorisnikId);
            ViewData["UslugaId"] = new SelectList(_context.Usluga, "Id", "Benefits", rezervacii.UslugaId);
            return View(rezervacii);
        }

        // POST: Rezervacijas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Vraboten,Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UslugaId,KorisnikId")] Rezervacija rezervacii)
        {
            if (id != rezervacii.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rezervacii);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervacijaExists(rezervacii.Id))
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
            ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "Id", "FullName", rezervacii.KorisnikId);
            ViewData["UslugaId"] = new SelectList(_context.Usluga, "Id", "Benefits", rezervacii.UslugaId);
            return View(rezervacii);
        }
 
        // GET: Rezervacijas/Delete/5

        [Authorize(Roles = "Admin,Korisnik")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacii = await _context.Rezervacija
                .Include(r => r.Korisnik)
                .Include(r => r.Usluga)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervacii == null)
            {
                return NotFound();
            }

            return View(rezervacii);
        }

        // POST: Rezervacijas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Admin,Korisnik")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervacii = await _context.Rezervacija.FindAsync(id);
            _context.Rezervacija.Remove(rezervacii);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

      
        private bool RezervacijaExists(int id)
        {
            return _context.Rezervacija.Any(e => e.Id == id);
        }
        

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> NewRezz(int id)
        {
           
            IQueryable<Usluga> uslugi = _context.Usluga;
            IEnumerable<Korisnik> korisnici = _context.Korisnik;
            

            NewRezzViewModel ViewModel = new NewRezzViewModel
            {
                Uslugi = new SelectList(await uslugi.ToListAsync(), "Id", "Benefits"),
                Korisnici = new SelectList(korisnici.OrderBy(s => s.FullName).ToList(), "Id", "FullName"),

            };
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> NewRezz(NewRezzViewModel ViewModel)
        {
            var usluga = await _context.Usluga.FirstOrDefaultAsync(c => c.Id == ViewModel.UslugaId);
            if (usluga == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                foreach (int korisnikId in ViewModel.SelectedKorisnici)
                {
                    Rezervacija enroll = await _context.Rezervacija
                        .FirstOrDefaultAsync(c => c.UslugaId == ViewModel.UslugaId && c.KorisnikId == korisnikId);
                    if (enroll == null)
                    {

                        _context.Add(new Rezervacija { KorisnikId = korisnikId, UslugaId = ViewModel.UslugaId });
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

       
       

        [Authorize(Roles = "Korisnik")]
        public async Task<IActionResult> KorisnikViewModel(int? id)
        {

            if (id == null)
            {
                //id = 1;
                return NotFound();
            }

            var korisnik = await _context.Korisnik.FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.Message = korisnik;

            
           
            if (korisnik == null)
            {
                return NotFound();
            }
            var ElexirAppContext = _context.Rezervacija.Where(m => m.KorisnikId == id).Include(m => m.Korisnik).Include(m => m.Usluga);
            return View(await ElexirAppContext.ToListAsync());
        }


        [Authorize(Roles = "Vraboten")]
        public async Task<IActionResult> KorisniciPoUsluga(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var usluga = await _context.Usluga.FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.Message = usluga;
            if (usluga == null)
            {
                return NotFound();
            }
            var ElexirAppContext = _context.Rezervacija.Where(m => m.UslugaId == id).Include(m => m.Korisnik).Include(m => m.Usluga);
            return View(await ElexirAppContext.ToListAsync());
        }


    }
}
