using ElexirApp_RSWEB.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElexirApp_RSWEB.ViewModel
{
    public class FiltrirajVraboten
    {
        public IList<Vraboten> Vraboten { get; set; }

        public SelectList Pozicii { get; set; }
        public string SearchPozicija { get; set; }
    }
}
