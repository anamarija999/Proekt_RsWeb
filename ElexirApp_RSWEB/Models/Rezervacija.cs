using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElexirApp_RSWEB.Models
{
    public class Rezervacija
    {
        public int Id { get; set; }

        [Display(Name = "Корисник")]
        public int KorisnikId { get; set; }
        public Korisnik Korisnik{ get; set; }

        [Display(Name = "Услуга")]
        public int UslugaId { get; set; }
        public Usluga Usluga { get; set; }
        [Display(Name = "Саат")]
        public string Hour { get; set; }
    }
}
