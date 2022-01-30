using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElexirApp_RSWEB.ViewModel
{
    public class KorisnikViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Име")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Презиме")]
        public string LastName { get; set; }


        [Display(Name = "Слика")]
        public IFormFile? Picture { get; set; }
    }
}
