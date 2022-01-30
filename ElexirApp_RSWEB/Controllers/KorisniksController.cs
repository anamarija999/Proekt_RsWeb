using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElexirApp_RSWEB.Data;
using ElexirApp_RSWEB.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using ElexirApp_RSWEB.ViewModel;


namespace ElexirApp_RSWEB.Controllers
{
    public class KorisniksController : Controller
    {
        private readonly ElexirApp_RSWEBContext _context;

        private readonly IHostingEnvironment webHostingEnvironment;


        public KorisniksController(ElexirApp_RSWEBContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;

            webHostingEnvironment = hostingEnvironment;
        }

        // GET: Korisnicks

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string StringName, string StringSurname)
        {
            
            IQueryable<Korisnik> korisnici = (IQueryable<Korisnik>)_context.Korisnik.AsQueryable();
            if (!string.IsNullOrEmpty(StringName))
            {
                korisnici = korisnici.Where(s => s.Ime.Contains(StringName));
            }
            if (!string.IsNullOrEmpty(StringSurname))
            {
                korisnici = korisnici.Where(s => s.Prezime.Contains(StringSurname));
            }
           
            var VM = new FiltrirajKorisnici
            {
                Korisnici = await korisnici.ToListAsync()
            };

            return View(VM);
        }

        // GET: Korsniks/Details/5

        [Authorize(Roles = "Admin,Vraboten")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _context.Korisnik
                .FirstOrDefaultAsync(m => m.Id == id);
            if (korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }

        // GET: Korisniks/Create

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Korisniks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(KorisnikViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Korisnik korisnik = new Korisnik
                {
                    Id = model.Id,
                    Ime = model.FirstName,
                    Prezime = model.LastName,
                    ProfilePicture = uniqueFileName
                };
                _context.Add(korisnik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        private string UploadedFile(KorisnikViewModel model)
        {
            string uniqueFileName = null;

            if (model.Picture != null)
            {
                string uploadsFolder = Path.Combine(webHostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.Picture.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Picture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        //Oveloaded function UploadedFile for Edit
        public string UploadedFile(IFormFile file)
        {
            string uniqueFileName = null;
            if (file != null)
            {
                string uploadsFolder = Path.Combine(webHostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: Korisniks/Edit/5

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _context.Korisnik.FindAsync(id);
            if (korisnik == null)
            {
                return NotFound();
            }
            return View(korisnik);
        }

        // POST: Korisniks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, IFormFile imageUrl,[Bind("Id,Ime,Prezime,Vozrast,ProfilePicture")] Korisnik korisnik)
        {
            if (id != korisnik.Id)
            {
                return NotFound();
            }

            KorisniksController uploadImage = new KorisniksController(_context, webHostingEnvironment);
            korisnik.ProfilePicture = uploadImage.UploadedFile(imageUrl);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(korisnik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KorisnikExists(korisnik.Id))
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
            return View(korisnik);
        }

        // GET: Korisniks/Delete/5

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _context.Korisnik
                .FirstOrDefaultAsync(m => m.Id == id);
            if (korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }

        // POST: Korisniks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var korisnik = await _context.Korisnik.FindAsync(id);
            _context.Korisnik.Remove(korisnik);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin")]
        private bool KorisnikExists(int id)
        {
            return _context.Korisnik.Any(e => e.Id == id);
        }
    }
}
