using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElexirApp_RSWEB.Models
{
    public class Usluga
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Услуга")]
        public string Name { get; set; }

        [Range(500, 10000)]
        [Display(Name = "Цена во денари")]
        public int Price { get; set; }

        [Display(Name = "Времетраење")]
        public string Duration { get; set; }

        [Display(Name = "Tип")]
        public string Benefits { get; set; }
   
        public int? FirstEmployeeId { get; set; }
        [Display(Name = "Вработен1")]
        public Vraboten FirstEmployee { get; set; }

        
        public int? SecondEmployeeId { get; set; }
        [Display(Name = "Вработен2")]
        public Vraboten SecondEmployee { get; set; }

        public ICollection<Rezervacija> Korisnici { get; set; }
    }
}
