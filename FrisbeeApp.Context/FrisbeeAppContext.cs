using FrisbeeApp.DatabaseModels.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FrisbeeApp.Context
{
    public class FrisbeeAppContext : IdentityDbContext<User,Role,Guid>
    {
        public FrisbeeAppContext(DbContextOptions<FrisbeeAppContext> options) : base(options)  { }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Training> Trainings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Role>().HasData(
                new Role()
                {
                    Id = new Guid ("79f4438c-5a0f-4ed2-aa0b-5d3bfb5de3a6"),
                    Name = "Player",
                    ConcurrencyStamp="1",
                    NormalizedName="Player"
                },
                new Role()
                {
                    Id = new Guid("89f4438c-5a0f-4ed2-aa0b-5d3bfb5de3a6"),
                    Name = "Coach",
                    ConcurrencyStamp = "2",
                    NormalizedName = "Coach"
                },
                new Role()
                {
                    Id = new Guid("99f4438c-5a0f-4ed2-aa0b-5d3bfb5de3a6"),
                    Name = "Admin",
                    ConcurrencyStamp = "3",
                    NormalizedName = "Admin"
                }

                );
        }

    }

}
