using System;
using FakeDataGenerator.Models.USA;
using Microsoft.EntityFrameworkCore;

namespace FakeDataGenerator.Models.Contexts
{
    public class UsaRecordsDbContext : DbContext
    {
        public DbSet<UsaMaleName> UsaMaleNames { get; set; }
        public DbSet<UsaFemaleName> UsaFemaleNames { get; set; }
        public DbSet<UsaSurname> UsaSurnames { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;user=root;password=375297374828qweR;database=usa_region;",
                new MySqlServerVersion(new Version(8, 0, 27)));
        }
    }
}