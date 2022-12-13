using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zawody.Models;

namespace Zawody.Data
{
    public class ApplicationDbContext : IdentityDbContext<ZawodyUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Trener> Trenerzy { get; set; }
        public DbSet<Stadion> Stadiony { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<ImageModel> Images { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
            {

            base.OnModelCreating(builder);
                builder.HasDefaultSchema("Identity");
                builder.Entity<ZawodyUser>(entity =>
                {
                    entity.ToTable(name: "User");
                });

                builder.Entity<IdentityRole>(entity =>
                {
                    entity.ToTable(name: "Role");
                });
                builder.Entity<IdentityUserRole<string>>(entity =>
                {
                    entity.ToTable("UserRoles");
                });

                builder.Entity<IdentityUserClaim<string>>(entity =>
                {
                    entity.ToTable("UserClaims");
                });

                builder.Entity<IdentityUserLogin<string>>(entity =>
                {
                    entity.ToTable("UserLogins");
                });

                builder.Entity<IdentityRoleClaim<string>>(entity =>
                {
                    entity.ToTable("RoleClaims");

                });

                builder.Entity<IdentityUserToken<string>>(entity =>
                {
                    entity.ToTable("UserTokens");
                });
                builder.Entity<Player>().ToTable("Person");
                builder.Entity<Address>().ToTable("Address");
                builder.Entity<Trener>().ToTable("Person");
                builder.Entity<Stadion>().ToTable("Stadion");
                builder.Entity<Team>().ToTable("Team");
                builder.Entity<Person>().ToTable("Person");
                builder.Entity<ImageModel>().ToTable("Images");

            builder.Entity<Person>()
                .HasOne<ZawodyUser>()
                .WithMany()
                .HasForeignKey(z => z.CreatedById)
                .HasPrincipalKey(x => x.Id);

            /*table.ForeignKey(
                        name: "FK_Person_User_CreatorId",
                        column: x => x.CreatorId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id");*/
        }
    }
}