using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElexirApp_RSWEB.Models
{
    public class User
    {
        [Required]
        [Display(Name = "Kорисничко име")]
        public string UserName { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]

        [Display(Name = "Е-мејл")]
        public string Email { get; set; }
        [Required]

        [Display(Name = "Лозинка")]
        public string Password { get; set; }

        [Display(Name = "Вработен")]
        public int? VrabotenId { get; set; }

        [Display(Name = "Корисник")]
        public int? KorisnikId { get; set; }
    }
}
