using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ElexirApp_RSWEB.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ElexirApp_RSWEBUser class
    public class ElexirApp_RSWEBUser : IdentityUser
    {
        public int? KorisnikId { get; set; }

        public int? VrabotenId { get; set; }
    }
}
