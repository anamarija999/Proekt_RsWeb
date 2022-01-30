using ElexirApp_RSWEB.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElexirApp_RSWEB.ViewModel
{
    public class FiltrirajUslugi
    {
        public IList<Usluga> Uslugi { get; set; }
        public IList<Vraboten>  Vraboten { get; set; }
        public SelectList Benefits { get; set; }
        //public string SearchStringName { get; set; }
        public string Benefit { get; set; }
        
    }
}
