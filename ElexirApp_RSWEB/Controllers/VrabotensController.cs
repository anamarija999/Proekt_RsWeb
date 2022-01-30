using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using ElexirApp_RSWEB.Data;
using ElexirApp_RSWEB.Models;
using ElexirApp_RSWEB.ViewModel;

namespace ElexirApp_RSWEB.Controllers
{
    public class VrabotensController : Controller
    {
        private readonly ElexirApp_RSWEBContext _context;
        private readonly IHostingEnvironment webHostingEnvironment;

        public VrabotensController(ElexirApp_RSWEBContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            webHostingEnvironment = hostingEnvironment;
        }

        // GET: Vodiches

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string SearchPozicija)
        {
            //return View(await _context.Vodich.ToListAsync());
            IQueryable<Vraboten> vraboteni = _context.Vraboten.AsQueryable();
            IQueryable<string> poziciiQuery = _context.Vraboten.OrderBy(m => m.Pozicija).Select(m => m.Pozicija).Distinct();
            if (!string.IsNullOrEmpty(SearchPozicija))
            {

                vraboteni = vraboteni.Where(x => x.Pozicija == SearchPozicija);
            }

            var VM = new FiltrirajVraboten
            {
                Vraboten = await vraboteni.ToListAsync(),
                Pozicii = new SelectList(await poziciiQuery.ToListAsync())
            };

            return View(VM);
        }

        // GET: Vrabotens/Details/5

        [Authorize(Roles = "Admin,Vraboten,Korisnik")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vraboten = await _context.Vraboten
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vraboten == null)
            {
                return NotFound();
            }

            return View(vraboten);
        }

        // GET: Vrabotens/Create

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vrabotens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VrabotenViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Vraboten vraboten = new Vraboten
                {
                    Id = model.Id,
                    Ime = model.Ime,
                    Prezime = model.Prezime,
                    Pozicija = model.Pozicija,
                    ProfilePicture = uniqueFileName
                };
                _context.Add(vraboten);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        private string UploadedFile(VrabotenViewModel model)
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


        // GET: Vrabotens/Edit/5

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vraboten = await _context.Vraboten.FindAsync(id);
            if (vraboten == null)
            {
                return NotFound();
            }
            return View(vraboten);
        }

        // POST: Vrabotens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, IFormFile imageUrl, [Bind("Id,Ime,Prezime,ProfilePicture,Pozicija")] Vraboten vraboten)
        {
            if (id != vraboten.Id)
            {
                return NotFound();
            }
            VrabotensController uploadImage = new VrabotensController(_context, webHostingEnvironment);
            vraboten.ProfilePicture = uploadImage.UploadedFile(imageUrl);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vraboten);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VrabotenExists(vraboten.Id))
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
            return View(vraboten);
        }

        // GET: Vrabotens/Delete/5

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vraboten = await _context.Vraboten
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vraboten == null)
            {
                return NotFound();
            }

            return View(vraboten);
        }

        // POST: Vrabotens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vraboten = await _context.Vraboten.FindAsync(id);
            _context.Vraboten.Remove(vraboten);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin")]
        private bool VrabotenExists(int id)
        {
            return _context.Vraboten.Any(e => e.Id == id);
        }
    }
}
