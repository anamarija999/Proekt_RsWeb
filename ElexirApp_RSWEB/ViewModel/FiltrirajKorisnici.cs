using ElexirApp_RSWEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElexirApp_RSWEB.ViewModel
{
    public class FiltrirajKorisnici
    {

        public IList<Korisnik> Korisnici { get; set; }
        public string StringName { get; set; }
        public string StringSurname { get; set; }
    }
}
