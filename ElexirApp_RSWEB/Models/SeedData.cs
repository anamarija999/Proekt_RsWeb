using ElexirApp_RSWEB.Areas.Identity.Data;
using ElexirApp_RSWEB.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElexirApp_RSWEB.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ElexirApp_RSWEBUser>>();
            IdentityResult roleResult;

            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }

            //Add Admin User
            ElexirApp_RSWEBUser user = await UserManager.FindByEmailAsync("admin1@beauty.com");
            if (user == null)
            {
                var User = new ElexirApp_RSWEBUser();
                User.Email = "admin1@beauty.com";
                User.UserName = "admin1@beauty.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result = await UserManager.AddToRoleAsync(User, "Admin"); }
            }

            //Add Korisnik Role
            var roleCheck2 = await RoleManager.RoleExistsAsync("Korisnik");
            if (!roleCheck2) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Korisnik")); }
            //Add Korisnik User
            ElexirApp_RSWEBUser user2 = await UserManager.FindByEmailAsync("korisnik1@beauty.com");
            if (user2 == null)
            {
                var User = new ElexirApp_RSWEBUser();
                User.Email = "korisnik1@beauty.com";
                User.UserName = "korisnik1@beauty.com";
                User.KorisnikId = 1;
                string userPWD = "Korisnik123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Planinar
                if (chkUser.Succeeded) { var result = await UserManager.AddToRoleAsync(User, "Korisnik"); }
            }

            //Add Vraboten Role
            var roleCheck3 = await RoleManager.RoleExistsAsync("Vraboten");
            if (!roleCheck3) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Vraboten")); }

            //Add Vraboten User
            ElexirApp_RSWEBUser user3 = await UserManager.FindByEmailAsync("vraboten1@beauty.com");
            if (user3 == null)
            {
                var User = new ElexirApp_RSWEBUser();
                User.Email = "vraboten1@beauty.com";
                User.UserName = "vraboten1@beauty.com";
                User.VrabotenId = 1;
                string userPWD = "Vraboten123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Vodich
                if (chkUser.Succeeded) { var result = await UserManager.AddToRoleAsync(User, "Vraboten"); }
            }

            ElexirApp_RSWEBUser user4 = await UserManager.FindByEmailAsync("vraboten2@beauty.com");
            if (user4 == null)
            {
                var User = new ElexirApp_RSWEBUser();
                User.Email = "vraboten2@beauty.com";
                User.UserName = "vraboten2@beauty.com";
                User.VrabotenId = 2;
                string userPWD = "Vraboten1234";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Vraboten
                if (chkUser.Succeeded) { var result = await UserManager.AddToRoleAsync(User, "Vraboten"); }
            }

            var roleCheck6 = await RoleManager.RoleExistsAsync("Korisnik");
            if (!roleCheck6) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Korisnik")); }
            //Add Planinar User
            ElexirApp_RSWEBUser user6 = await UserManager.FindByEmailAsync("korisnik2@beauty.com");
            if (user6 == null)
            {
                var User = new ElexirApp_RSWEBUser();
                User.Email = "korisnik2@beauty.com";
                User.UserName = "korisnik2@beauty.com";
                User.KorisnikId = 2;
                string userPWD = "Korisnik1234";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Planinar
                if (chkUser.Succeeded) { var result = await UserManager.AddToRoleAsync(User, "Korisnik"); }
            }

        }
            public static void Initialize(IServiceProvider serviceProvider)
            {
                using (var context = new ElexirApp_RSWEBContext(
                    serviceProvider.GetRequiredService<DbContextOptions<ElexirApp_RSWEBContext>>()))
                {

                    CreateUserRoles(serviceProvider).Wait();



                    if (context.Usluga.Any() || context.Vraboten.Any() || context.Korisnik.Any())
                {
                    return;
                }

                context.Vraboten.AddRange(
                    new Vraboten { Ime = "Андреа", Prezime = "Ивановска", Pozicija = "Маникир/Педикир" },
                    new Vraboten { Ime = "Лука", Prezime = "Ралев", Pozicija = "Стилист за коса" },
                    new Vraboten { Ime = "Ана Марија", Prezime = "Трајковска", Pozicija = "Маникир/Педикир" },
                    new Vraboten { Ime = "Илина", Prezime = "Крстевска", Pozicija = "Шминка" },
                    new Vraboten { Ime = "Виктор", Prezime = "Дамчев", Pozicija = "Шминка" },
                    new Vraboten { Ime = "Иван", Prezime = "Костовски", Pozicija = "Стилист за коса" }
                );

                context.SaveChanges();

                context.Korisnik.AddRange(
                    new Korisnik { Ime = "Ангела", Prezime = "Тасиќ" },
                    new Korisnik { Ime = "Сара", Prezime = "Стојановска" },
                    new Korisnik { Ime = "Марија", Prezime = "Зајкова" },
                    new Korisnik { Ime = "Теа", Prezime = "Гиговска" },
                    new Korisnik { Ime = "Мила", Prezime = "Павловска" },
                    new Korisnik { Ime = "Дена", Prezime = "Сачмаровска" }
                    );
                context.SaveChanges();

                context.Usluga.AddRange(
                    new Usluga
                    {
                        Name = "Шминка",
                        Price = 1500,
                        Duration = "30min-1h",
                        Benefits = "За Табло",
                        FirstEmployeeId = context.Vraboten.Single(d => d.Ime == "Виктор" && d.Prezime == "Дамчев").Id,
                        SecondEmployeeId = context.Vraboten.Single(d => d.Ime == "Илина" && d.Prezime == "Крстевска").Id
                    },
                    new Usluga
                    {
                        Name = "Шминка",
                        Price = 2500,
                        Duration = "1h-1:30h",
                        Benefits = "За Свадба",
                        FirstEmployeeId = context.Vraboten.Single(d => d.Ime == "Виктор" && d.Prezime == "Дамчев").Id,
                        SecondEmployeeId = context.Vraboten.Single(d => d.Ime == "Илина" && d.Prezime == "Крстевска").Id
                    },
                    new Usluga
                    {
                        Name = "Шминка",
                        Price = 1500,
                        Duration = "30min-1h",
                        Benefits = "Нормална",
                        FirstEmployeeId = context.Vraboten.Single(d => d.Ime == "Виктор" && d.Prezime == "Дамчев").Id,
                        SecondEmployeeId = context.Vraboten.Single(d => d.Ime == "Илина" && d.Prezime == "Крстевска").Id
                    },
                    new Usluga
                    {
                        Name = "Коса",
                        Price = 1000,
                        Duration = "1h",
                        Benefits = "Шишање+Фенирање",
                        FirstEmployeeId = context.Vraboten.Single(d => d.Ime == "Лука" && d.Prezime == "Ралев").Id,
                        SecondEmployeeId = context.Vraboten.Single(d => d.Ime == "Иван" && d.Prezime == "Костовски").Id
                    },
                    new Usluga
                    {
                        Name = "Коса",
                        Price = 1500,
                        Duration = "1h-2h",
                        Benefits = "Фризура",
                        FirstEmployeeId = context.Vraboten.Single(d => d.Ime == "Лука" && d.Prezime == "Ралев").Id,
                        SecondEmployeeId = context.Vraboten.Single(d => d.Ime == "Иван" && d.Prezime == "Костовски").Id
                    },
                    new Usluga
                    {
                        Name = "Маникир/Педикир",
                        Price = 800,
                        Duration = "1h",
                        Benefits = "Гел лак",
                        FirstEmployeeId = context.Vraboten.Single(d => d.Ime == "Андреа" && d.Prezime == "Ивановска").Id,
                        SecondEmployeeId = context.Vraboten.Single(d => d.Ime == "Ана Марија" && d.Prezime == "Трајковска").Id
                    },
                    new Usluga
                    {
                        Name = "Маникир/Педикир",
                        Price = 1200,
                        Duration = "1h-1:30h",
                        Benefits = "Корекција гел",
                        FirstEmployeeId = context.Vraboten.Single(d => d.Ime == "Андреа" && d.Prezime == "Ивановска").Id,
                        SecondEmployeeId = context.Vraboten.Single(d => d.Ime == "Ана Марија" && d.Prezime == "Трајковска").Id
                    },
                    new Usluga
                    {
                        Name = "Маникир/Педикир",
                        Price = 2100,
                        Duration = "2h-2:30h",
                        Benefits = "Гел+Педикир",
                        FirstEmployeeId = context.Vraboten.Single(d => d.Ime == "Андреа" && d.Prezime == "Ивановска").Id,
                        SecondEmployeeId = context.Vraboten.Single(d => d.Ime == "Ана Марија" && d.Prezime == "Трајковска").Id
                    }

                );
                context.SaveChanges();
                context.Rezervacija.AddRange(
                   new Rezervacija { KorisnikId = 1, UslugaId = 1 ,Hour="12:00"},
                   new Rezervacija { KorisnikId = 2, UslugaId = 2, Hour = "10:00" },
                   new Rezervacija { KorisnikId = 2, UslugaId = 6, Hour = "11:00" },
                   new Rezervacija { KorisnikId = 1, UslugaId = 3 , Hour = "13:00" },
                   new Rezervacija { KorisnikId = 3, UslugaId = 8 , Hour = "17:00" },
                   new Rezervacija { KorisnikId = 6, UslugaId = 7, Hour = "14:00" }

               );

                context.SaveChanges();
            }
        }
    }
}




