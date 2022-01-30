using ElexirApp_RSWEB.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElexirApp_RSWEB.ViewModel
{
    public class NewRezzViewModel
    {
        public Rezervacija Rezervacija { get; set; }
        public virtual Usluga Usluga { get; set; }
        public int UslugaId { get; set; }

        [Display(Name = "Корисници")]
        public IEnumerable<int> SelectedKorisnici { get; set; }
      
        public SelectList Uslugi { get; set; }
        public SelectList Korisnici { get; set; }

        [Display(Name = "Саат")]
        public string Hour { get; set; }

       
    }
}
