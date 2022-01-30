using ElexirApp_RSWEB.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElexirApp_RSWEB.ViewModel
{
    public class EditUslugaViewModel
    {
        public Usluga Usluga { get; set; }
        public IEnumerable<int> SelectedKorisnici { get; set; }
        public IEnumerable<SelectListItem> ListaKorisnici { get; set; }
    }
}
