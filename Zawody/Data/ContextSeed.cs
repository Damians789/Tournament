using Microsoft.AspNetCore.Identity;
using Zawody.Models;
using Zawody.Data;
using Zawody.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Zawody.Data
{
    
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<ZawodyUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in En.GetValues<Roles>())
            {
                await roleManager.CreateAsync(new IdentityRole(role.ToString()));
            }
            //Seed Roles
            /*await roleManager.CreateAsync(new IdentityRole(Helpers.Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Helpers.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Helpers.Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Helpers.Roles.Basic.ToString()));*/
        }
        public static async Task SeedSuperAdminAsync(UserManager<ZawodyUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ZawodyUser
            {
                UserName = "superadmin",
                Email = "superadmin@gmail.com",
                FirstName = "super",
                LastName = "admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    foreach (var role in En.GetValues<Roles>())
                    {
                        await userManager.AddToRoleAsync(defaultUser, role.ToString());
                    }
                    /*await userManager.CreateAsync(defaultUser, "123Pa$$word.");
                    await userManager.AddToRoleAsync(defaultUser, Helpers.Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Helpers.Roles.Moderator.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Helpers.Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Helpers.Roles.SuperAdmin.ToString());*/
                }

            }
        }

        public static void Initialize(ApplicationDbContext context)
        {
            //context.Database.EnsureCreated();
            /*context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;*/
            context.Database.Migrate();

            // Look for any players.
            if (context.Addresses.Any())
            {
                return;   // DB has been seeded
            }

            var addresses = new Address[]
            {
                new Address
                {
                    City = "Toronto", Street = "170 Princes Boulevard", Country = "Canada"
                },
                new Address
                {
                    City = "Col. Santa Ursula", Street = "Calzada de Tlalpan No. 3465", Country = "Mexico"
                },
                new Address
                {
                    City = "Baranquilla", Street = "Esquina. Av. Circunvalar", Country = "Columbia"
                },
                new Address
                {
                    City = "Auckland", Street = "Reimers Ave", Country = "New Zealand"
                },
                new Address
                {
                    City = "Hafnarfjordur", Street = "Kaplakrika IS-220", Country = "Iceland"
                },
            };

            foreach (Address a in addresses)
            {
                context.Addresses.Add(a);
            }
            context.SaveChanges();

            var stadions = new Stadion[]
            {
                new Stadion { Name = "BMO Field", Pojemnosc = 30991, AddressId = addresses.Single( a => a.Street == "170 Princes Boulevard").Id},
                new Stadion { Name = "Estadio Azteca", Pojemnosc = 87523, AddressId = addresses.Single( a => a.Street == "Calzada de Tlalpan No. 3465").Id},
                new Stadion { Name = "Estadio Roberto Meléndez", Pojemnosc = 49612, AddressId = addresses.Single( a => a.Street == "Esquina. Av. Circunvalar").Id},
                new Stadion { Name = "Eden Park", Pojemnosc = 50000, AddressId = addresses.Single( a => a.Street == "Reimers Ave").Id},
                new Stadion { Name = "Kaplakriki", Pojemnosc = 6450, AddressId = addresses.Single( a => a.Street == "Kaplakrika IS-220").Id},

            };

            context.Stadiony.AddRange(stadions);
            context.SaveChanges();

            var hmm = context.Users.ToList();
            var userid = GetUserId(hmm);

            var trainers = new Trener[]
            {
                new Trener { LastName = "Bradley", FirstName = "Bob", DateOfBirth = DateTime.Parse("1958-03-03"), HireDate = DateTime.Parse("1972-03-03"), CreatedById = userid },
                new Trener { LastName = "Solari Poggio", FirstName = "Santiago Hernán", DateOfBirth = DateTime.Parse("1976-09-07"), HireDate = DateTime.Parse("1972-09-03"), CreatedById = userid },
                new Trener { LastName = "Real", FirstName = "Juan Cruz", DateOfBirth = DateTime.Parse("1976-09-08"), HireDate = DateTime.Parse("2005-07-12"), CreatedById = userid },
                new Trener { LastName = "Paulo", FirstName = "Sousa", DateOfBirth = DateTime.Parse("1970-08-30"), HireDate = DateTime.Parse("2021-04-20"), CreatedById = userid },
                new Trener { LastName = "Jóhannesson", FirstName = "Ólafur Davíd", DateOfBirth = DateTime.Parse("1957-08-30"), HireDate = DateTime.Parse("1985-10-12"), CreatedById = userid },
            };

            context.Trenerzy.AddRange(trainers);
            context.SaveChanges();

            var teams = new Team[]
            {
                new Team { Name = "Toronto FC", ShortenedName = "Toronto",
                StadionID = stadions.Single( i => i.Name == "BMO Field").Id,
                TrenerID = trainers.Single( i => i.LastName == "Bradley").ID
                },
                new Team { Name = "CF América", ShortenedName = "América",
                StadionID = stadions.Single( i => i.Name == "Estadio Azteca").Id,
                TrenerID = trainers.Single( i => i.LastName == "Solari Poggio").ID
                },
                new Team { Name = "CD Popular Junior FC SA", ShortenedName = "Junior",
                StadionID = stadions.Single( i => i.Name == "Estadio Roberto Meléndez").Id,
                TrenerID = trainers.Single( i => i.LastName == "Real").ID
                },
                new Team { Name = "Manchester United", ShortenedName = "Man UTD"
                },
                new Team { Name = "FH Hafnarfjörður", ShortenedName = "FH",
                StadionID = stadions.Single( i => i.Name == "Kaplakriki").Id,
                TrenerID = trainers.Single( i => i.LastName == "Jóhannesson").ID
                },
            };

            context.Teams.AddRange(teams);
            context.SaveChanges();

            var players = new Player[]
            {
                new Player { LastName = "Bono", FirstName = "Alex", DateOfBirth = DateTime.Parse("1994-04-25"), Pozycja = "Bramkarz", CreatedById = userid,
                TeamID = teams.Single(t => t.ShortenedName == "Toronto").Id},
                new Player { LastName = "Westberg", FirstName = "Quentin", DateOfBirth = DateTime.Parse("1986-04-25"), CreatedById = userid,
                TeamID = teams.Single(t => t.Name == "Toronto FC").Id, Pozycja = "Napastnik"},
                new Player { LastName = "Mavinga", FirstName = "Chris", DateOfBirth = DateTime.Parse("1991-05-26"), CreatedById = userid,
                TeamID = teams.Single(t => t.Name == "Toronto FC").Id, Pozycja = "Pomocnik"},
                new Player { LastName = "O'Neill", FirstName = "Shane", DateOfBirth = DateTime.Parse("1993-09-02"), CreatedById = userid,
                TeamID = teams.Single(t => t.Name == "Toronto FC").Id, Pozycja = "Obrońca"},

                new Player { LastName = "Ochoa Magaña", FirstName = "Francisco Guillermo", DateOfBirth = DateTime.Parse("1958-03-03"), CreatedById = userid,
                TeamID = teams.Single(t => t.Name == "CF América").Id, Pozycja = "Bramkarz"},
                new Player { LastName = "Jiménez Fabela", FirstName = "Óscar Francisco", DateOfBirth = DateTime.Parse("1988-09-12"), CreatedById = userid,
                TeamID = teams.Single(t => t.Name == "CF América").Id, Pozycja = "Napastnik"},
                new Player { LastName = "Tapia Méndez", FirstName = "Fernando", DateOfBirth = DateTime.Parse("2001-08-17"), CreatedById = userid,
                TeamID = teams.Single(t => t.Name == "CF América").Id, Pozycja = "Pomocnik"},
                new Player { LastName = "Fuentes Vargas", FirstName = "Luis Fernando", DateOfBirth = DateTime.Parse("1986-09-14"), CreatedById = userid,
                TeamID = teams.Single(t => t.Name == "CF América").Id, Pozycja = "Obrońca"},

                new Player { LastName = "Viera Galain", FirstName = "Mario Sebastián", DateOfBirth = DateTime.Parse("1983-03-07"), CreatedById = userid,
                TeamID = teams.Single(t => t.Name == "CD Popular Junior FC SA").Id, Pozycja = "Bramkarz"},
                new Player { LastName = "Chaux Ospina", FirstName = "Eder Aleixo", DateOfBirth = DateTime.Parse("1991-12-20"), CreatedById = userid,
                TeamID = teams.Single(t => t.Name == "CD Popular Junior FC SA").Id, Pozycja = "Napastnik"},
                new Player { LastName = "Araujo Lemuz", FirstName = "Sebastián", DateOfBirth = DateTime.Parse("1997-08-19"), CreatedById = userid,
                TeamID = teams.Single(t => t.Name == "CD Popular Junior FC SA").Id, Pozycja = "Pomocnik"},
                new Player { LastName = "Castro Dinas", FirstName = "Joan Sebastián", DateOfBirth = DateTime.Parse("2001-08-17"), CreatedById = userid,
                TeamID = teams.Single(t => t.Name == "CD Popular Junior FC SA").Id, Pozycja = "Obrońca"},

                new Player { LastName = "Nielsen", FirstName = "Gunnar", DateOfBirth = DateTime.Parse("1986-09-07"), CreatedById = userid,
                TeamID = teams.Single(t => t.Name == "FH Hafnarfjörður").Id, Pozycja = "Bramkarz"},
                new Player { LastName = "Guðmundsson", FirstName = "Atli Gunnar", DateOfBirth = DateTime.Parse("1993-09-08"), CreatedById = userid,
                TeamID = teams.Single(t => t.Name == "FH Hafnarfjörður").Id, Pozycja = "Napastnik"},
                new Player { LastName = "Arnarsson", FirstName = "Daði Freyr", DateOfBirth = DateTime.Parse("1998-09-23"), CreatedById = userid,
                TeamID = teams.Single(t => t.Name == "FH Hafnarfjörður").Id, Pozycja = "Pomocnik"},
                new Player { LastName = "Gunnarsson", FirstName = "Hörður Ingi", DateOfBirth = DateTime.Parse("1998-08-14"), CreatedById = userid,
                TeamID = teams.Single(t => t.Name == "FH Hafnarfjörður").Id, Pozycja = "Obrońca"},

            };

            /*foreach (Player p in players)
            {
                var playersInDataBase = context.Players.Where(
                    p =>
                            p.Team.Id == p.TeamID).SingleOrDefault();
                if (playersInDataBase == null)
                {
                    context.Players.Add(p);
                }
            }
            context.SaveChanges();*/

            context.Players.AddRange(players);
            context.SaveChanges();

        }
        private static string? GetUserId(List<ZawodyUser> hmm)
        {
            foreach (ZawodyUser z in hmm)
            {
                if (z.FirstName == "super")
                {
                    return z.Id;
                }
            }
            return null;
        }
    }
}
