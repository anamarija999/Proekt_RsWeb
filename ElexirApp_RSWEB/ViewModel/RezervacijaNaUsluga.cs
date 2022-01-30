
using ElexirApp_RSWEB.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElexirApp_RSWEB.ViewModel
{
    public class RezervacijaNaUsluga

    {
        public IList<Rezervacija> Rezervacija { get; set; }

        public SelectList UslugaMesto { get; set; }
        public string Usluga { get; set; }
    }
}
