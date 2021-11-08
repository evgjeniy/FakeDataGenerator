using System;
using FakeDataGenerator.Models.UA;
using Microsoft.EntityFrameworkCore;

namespace FakeDataGenerator.Models.Contexts
{
    public class UaRecordsDbContext : DbContext
    {
        public DbSet<UaMaleName> UaMaleNames { get; set; }
        public DbSet<UaFemaleName> UaFemaleNames { get; set; }
        public DbSet<UaSurname> UaSurnames { get; set; }
        public DbSet<UaMalePatronymic> UaMalePatronymics { get; set; }
        public DbSet<UaFemalePatronymic> UaFemalePatronymics { get; set; }

        public UaRecordsDbContext()
        {
            Database.EnsureCreated();
        }
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;user=root;password=375297374828qweR;database=ua_region;",
                new MySqlServerVersion(new Version(8, 0, 27)));
        }
    }
}