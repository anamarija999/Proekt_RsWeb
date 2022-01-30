using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ElexirApp_RSWEB.Data;
using ElexirApp_RSWEB.Models;
using ElexirApp_RSWEB.ViewModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ElexirApp_RSWEB.Controllers
{
    public class UslugasController : Controller
    {
        private readonly ElexirApp_RSWEBContext _context;

        public UslugasController(ElexirApp_RSWEBContext context)
        {
            _context = context;
        }


        [Authorize(Roles = "Vraboten,Korisnik")]
        public async Task<IActionResult> UslugaPoVraboten(int? id)
        {

            var vraboten = await _context.Vraboten.FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.Message = vraboten;

            if (id == null)
            {
                return NotFound();
            }

            if (vraboten == null)
            {
                return NotFound();
            }
            var ElexirAppContext = _context.Usluga.Where(m => (m.FirstEmployeeId == id) || (m.SecondEmployeeId == id)).Include(c => c.FirstEmployee).Include(c => c.SecondEmployee);
            return View(await ElexirAppContext.ToListAsync());
        }

        // GET: Uslugas

        [Authorize(Roles = "Admin,Korisnik")]
        public async Task<IActionResult> Index(string Benefit)
        {
            IQueryable<Usluga> uslugi = (IQueryable<Usluga>)_context.Usluga.AsQueryable();
            IQueryable<Vraboten> vraboten = _context.Vraboten.AsQueryable();
            IQueryable<string> benefitsQuery = _context.Usluga.OrderBy(m => m.Benefits).Select(m => m.Benefits).Distinct();
           

            if (!string.IsNullOrEmpty(Benefit))
            {
                uslugi = uslugi.Where(x => x.Benefits == Benefit);
            }

           
            var viewmodel = new FiltrirajUslugi
            {

                Benefits = new SelectList(await benefitsQuery.ToListAsync()),
                Vraboten = await vraboten.ToListAsync(),
                Uslugi = await uslugi.ToListAsync()
            };
            return View(viewmodel);

            
        }

        // GET: Uslugas/Details/5

        [Authorize(Roles = "Admin,Korisnik")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usluga = await _context.Usluga
                .Include(t => t.FirstEmployee)
                .Include(t => t.SecondEmployee)
                .Include(m => m.Korisnici).ThenInclude(m=>m.Korisnik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usluga == null)
            {
                return NotFound();
            }

            return View(usluga);
        }

        // GET: Uslugas/Create

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["FirstEmployeeId"] = new SelectList(_context.Vraboten, "Id", "FullName");
            ViewData["SecondEmployeeId"] = new SelectList(_context.Vraboten, "Id", "FullName");
            return View();
        }

        // POST: Uslugas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Duration,Benefits,FirstEmployeeId,SecondEmployeeId")] Usluga usluga)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usluga);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FirstEmployeeId"] = new SelectList(_context.Vraboten, "Id", "FullName", usluga.FirstEmployeeId);
            ViewData["SecondEmployeeId"] = new SelectList(_context.Vraboten, "Id", "FullName", usluga.SecondEmployeeId);
            return View(usluga);
        }

        // GET: Uslugas/Edit/5

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usluga = _context.Usluga.Where(m => m.Id == id).Include(m => m.Korisnici).First();

            if (usluga == null)
            {
                return NotFound();
            }

            var korisnici = _context.Korisnik.AsEnumerable();
            korisnici = korisnici.OrderBy(s => s.FullName);
            EditUslugaViewModel viewmodel = new EditUslugaViewModel
            {
                Usluga = usluga,
                ListaKorisnici = new MultiSelectList(korisnici, "Id", "FullName"),
                SelectedKorisnici = usluga.Korisnici.Select(sa => sa.KorisnikId)
            };

            ViewData["FirstEmployeeId"] = new SelectList(_context.Vraboten, "Id", "FullName", usluga.FirstEmployeeId);
            ViewData["SecondEmployeeId"] = new SelectList(_context.Vraboten, "Id", "FullName", usluga.SecondEmployeeId);
            return View(viewmodel);
        }

        // POST: Uslugas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, EditUslugaViewModel viewModel)
        {
            if (id != viewModel.Usluga.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewModel.Usluga);
                    await _context.SaveChangesAsync();
                    IEnumerable<int> listaKorisnici = viewModel.SelectedKorisnici;
                    IQueryable<Rezervacija> toBeRemoved = _context.Rezervacija.Where(s => !listaKorisnici.Contains(s.KorisnikId) && s.UslugaId == id);
                    _context.Rezervacija.RemoveRange(toBeRemoved);
                    IEnumerable<int> existKorisnici = _context.Rezervacija.Where(s => listaKorisnici.Contains(s.KorisnikId) && s.UslugaId == id).Select(s => s.KorisnikId);
                    IEnumerable<int> newKorisnici = listaKorisnici.Where(s => !existKorisnici.Contains(s));
                    foreach (int KorisnikId in newKorisnici)
                        _context.Rezervacija.Add(new Rezervacija { KorisnikId = KorisnikId, UslugaId = id });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UslugaExists(viewModel.Usluga.Id))
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
            ViewData["FirstEmployeeId"] = new SelectList(_context.Vraboten, "Id", "FullName", viewModel.Usluga.FirstEmployeeId);
            ViewData["SecondEmployeeId"] = new SelectList(_context.Vraboten, "Id", "FullName", viewModel.Usluga.SecondEmployeeId);
            return View(viewModel);
        }

        // GET: Uslugas/Delete/5

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usluga = await _context.Usluga
                .Include(t => t.FirstEmployee)
                .Include(t => t.SecondEmployee)
                .Include(t => t.Korisnici).ThenInclude(t => t.Korisnik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usluga == null)
            {
                return NotFound();
            }

            return View(usluga);
        }


        // POST: Uslugas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usluga = await _context.Usluga.FindAsync(id);
            _context.Usluga.Remove(usluga);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool UslugaExists(int id)
        {
            return _context.Usluga.Any(e => e.Id == id);
        }

    }
}
