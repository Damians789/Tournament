/*using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zawody.Models;
using System.Security.Claims;

namespace Zawody.Data

{
    public class DbInitialize
    {
        public static void Initialize(ApplicationDbContext context, UserManager<ZawodyUser> _userManager)
        {
            //context.Database.EnsureCreated();

            // Look for any players.
            if (context.Players.Any())
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

            var lol = _userManager.Users.Where(s => s.UserName == "superadmin");
            var userid = _userManager.GetUserId((ClaimsPrincipal)lol);

            var trainers = new Trener[]
            {
                new Trener { LastName = "Bradley", FirstName = "Bob", DateOfBirth = DateTime.Parse("1958-03-03"), CreatorId = userid },
                new Trener { LastName = "Solari Poggio", FirstName = "Santiago Hernán", DateOfBirth = DateTime.Parse("1976-09-07"), CreatorId = userid },
                new Trener { LastName = "Real", FirstName = "Juan Cruz", DateOfBirth = DateTime.Parse("1976-09-08"), CreatorId = userid },
                new Trener { LastName = "Bradley", FirstName = "Bob", DateOfBirth = DateTime.Parse("1958-03-03"), CreatorId = userid },
                new Trener { LastName = "Jóhannesson", FirstName = "Ólafur Davíd", DateOfBirth = DateTime.Parse("1957-08-30"), CreatorId = userid },
            };

            context.Trenerzy.AddRange(trainers);
            context.SaveChanges();

            var teams = new Team[]
            {
                new Team { Name = "Toronto FC", ShortenedNamed = "Toronto",
                StadionID = stadions.Single( i => i.Name == "BMO Field").Id,
                TrenerID = trainers.Single( i => i.LastName == "Bradley").ID
                },
                new Team { Name = "CF América", ShortenedNamed = "América",
                StadionID = stadions.Single( i => i.Name == "Estadio Azteca").Id,
                TrenerID = trainers.Single( i => i.LastName == "Solari Poggio").ID
                },
                new Team { Name = "CD Popular Junior FC SA", ShortenedNamed = "Junior",
                StadionID = stadions.Single( i => i.Name == "Estadio Roberto Meléndez").Id,
                TrenerID = trainers.Single( i => i.LastName == "Solari Poggio").ID
                },
                new Team { Name = "CF América", ShortenedNamed = "América",
                StadionID = stadions.Single( i => i.Name == "Estadio Azteca").Id,
                TrenerID = trainers.Single( i => i.LastName == "Solari Poggio").ID
                },
                new Team { Name = "FH Hafnarfjörður", ShortenedNamed = "FH",
                StadionID = stadions.Single( i => i.Name == "Kaplakriki").Id,
                TrenerID = trainers.Single( i => i.LastName == "Jóhannesson").ID
                },
            };

            foreach (Team t in teams)
            {
                var teamsInDataBase = context.Teams.Where(
                    t =>
                            t.Stadion.Id == t.StadionID &&
                            t.Trener.ID == t.TrenerID).SingleOrDefault();
                if (teamsInDataBase == null)
                {
                    context.Teams.Add(t);
                }
            }
            context.SaveChanges();

            *//*foreach (Enrollment e in enrollments)
            {
                var enrollmentInDataBase = context.Enrollments.Where(
                    s =>
                            s.Student.ID == e.StudentID &&
                            s.Course.CourseID == e.CourseID).SingleOrDefault();
                if (enrollmentInDataBase == null)
                {
                    context.Enrollments.Add(e);
                }
            }
            context.SaveChanges();*//*


            context.Teams.AddRange(teams);
            context.SaveChanges();

            var players = new Player[]
            {
                new Player { LastName = "Bono", FirstName = "Alex", DateOfBirth = DateTime.Parse("1994-04-25"), CreatorId = userid,
                TeamID = teams.Single(t => t.Name == "Toronto FC").Id},
                new Player { LastName = "Westberg", FirstName = "Quentin", DateOfBirth = DateTime.Parse("1986-04-25"), CreatorId = userid,
                TeamID = teams.Single(t => t.Name == "Toronto FC").Id},
                new Player { LastName = "Mavinga", FirstName = "Chris", DateOfBirth = DateTime.Parse("1991-26-05"), CreatorId = userid,
                TeamID = teams.Single(t => t.Name == "Toronto FC").Id},
                new Player { LastName = "O'Neill", FirstName = "Shane", DateOfBirth = DateTime.Parse("1993-09-02"), CreatorId = userid,
                TeamID = teams.Single(t => t.Name == "Toronto FC").Id},

                new Player { LastName = "Ochoa Magaña", FirstName = "Francisco Guillermo", DateOfBirth = DateTime.Parse("1958-03-03"), CreatorId = userid,
                TeamID = teams.Single(t => t.Name == "CF América").Id},
                new Player { LastName = "Jiménez Fabela", FirstName = "Óscar Francisco", DateOfBirth = DateTime.Parse("1988-09-12"), CreatorId = userid,
                TeamID = teams.Single(t => t.Name == "CF América").Id},
                new Player { LastName = "Tapia Méndez", FirstName = "Fernando", DateOfBirth = DateTime.Parse("2001-08-17"), CreatorId = userid,
                TeamID = teams.Single(t => t.Name == "CF América").Id},
                new Player { LastName = "Fuentes Vargas", FirstName = "Luis Fernando", DateOfBirth = DateTime.Parse("1986-09-14"), CreatorId = userid,
                TeamID = teams.Single(t => t.Name == "CF América").Id},

                new Player { LastName = "Viera Galain", FirstName = "Mario Sebastián", DateOfBirth = DateTime.Parse("1983-03-07"), CreatorId = userid,
                TeamID = teams.Single(t => t.Name == "CF América").Id},
                new Player { LastName = "Chaux Ospina", FirstName = "Eder Aleixo", DateOfBirth = DateTime.Parse("1991-12-20"), CreatorId = userid,
                TeamID = teams.Single(t => t.Name == "CF América").Id},
                new Player { LastName = "Araujo Lemuz", FirstName = "Sebastián", DateOfBirth = DateTime.Parse("1997-08-19"), CreatorId = userid,
                TeamID = teams.Single(t => t.Name == "CF América").Id},
                new Player { LastName = "Castro Dinas", FirstName = "Joan Sebastián", DateOfBirth = DateTime.Parse("2001-08-17"), CreatorId = userid,
                TeamID = teams.Single(t => t.Name == "CF América").Id},

                new Player { LastName = "Nielsen", FirstName = "Gunnar", DateOfBirth = DateTime.Parse("1986-09-07"), CreatorId = userid,
                TeamID = teams.Single(t => t.Name == "FH Hafnarfjörður").Id},
                new Player { LastName = "Guðmundsson", FirstName = "Atli Gunnar", DateOfBirth = DateTime.Parse("1993-09-08"), CreatorId = userid,
                TeamID = teams.Single(t => t.Name == "FH Hafnarfjörður").Id},
                new Player { LastName = "Arnarsson", FirstName = "Daði Freyr", DateOfBirth = DateTime.Parse("1998-09-23"), CreatorId = userid,
                TeamID = teams.Single(t => t.Name == "FH Hafnarfjörður").Id},
                new Player { LastName = "Gunnarsson", FirstName = "Hörður Ingi", DateOfBirth = DateTime.Parse("1998-08-14"), CreatorId = userid,
                TeamID = teams.Single(t => t.Name == "FH Hafnarfjörður").Id},

            };

            context.Players.AddRange(players);
            context.SaveChanges();

        }
    }
}
*/