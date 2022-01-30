using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElexirApp_RSWEB.Models
{
    public class Vraboten
    {
        public int Id { get; set; }

        [Display(Name = "Име")]
        public string Ime { get; set; }

        [Display(Name = "Презиме")]
        public string Prezime { get; set; }

        [Display(Name = "Слика")]
        public string ProfilePicture { get; set; }

        [Display(Name = "Позиција")]
        public string Pozicija { get; set; }

        [Display(Name = "Услуга1")]
        public ICollection<Usluga> Usluga1 { get; set; }

        [Display(Name = "Услуга2")]
        public ICollection<Usluga> Usluga2 { get; set; }

        public string FullName
        {
            get { return String.Format("{0} {1}", Ime, Prezime); }
        
        }


    }
}
