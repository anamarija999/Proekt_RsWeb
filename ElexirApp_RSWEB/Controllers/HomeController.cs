
using ElexirApp_RSWEB.Areas.Identity.Data;
using ElexirApp_RSWEB.Data;
using ElexirApp_RSWEB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ElexirApp_RSWEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ElexirApp_RSWEBContext _context;
        private readonly UserManager<ElexirApp_RSWEBUser> userManager;

        public HomeController(ILogger<HomeController> logger, ElexirApp_RSWEBContext context, UserManager<ElexirApp_RSWEBUser> usrMgr)
        {
            _logger = logger;
            _context = context;
            userManager = usrMgr;
        }
       
        public async Task<IActionResult> IndexAsync()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Uslugas");
            }
            else if (User.IsInRole("Vraboten"))
            {
                var userID = userManager.GetUserId(User);
                ElexirApp_RSWEBUser user = await userManager.FindByIdAsync(userID);
                return RedirectToAction("UslugaPoVraboten", "Uslugas", new { id = user.VrabotenId });
            }
            else if (User.IsInRole("Korisnik"))
            {
                var userID = userManager.GetUserId(User);
                ElexirApp_RSWEBUser user = await userManager.FindByIdAsync(userID);
                return RedirectToAction("KorisnikViewModel", "Rezervacijas", new { id = user.KorisnikId });
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
