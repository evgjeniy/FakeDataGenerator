using System;
using FakeDataGenerator.Models.BY;
using Microsoft.EntityFrameworkCore;

namespace FakeDataGenerator.Models.Contexts
{
    public class ByRecordsDbContext : DbContext
    {
        public DbSet<ByMaleName> ByMaleNames { get; set; }
        public DbSet<ByFemaleName> ByFemaleNames { get; set; }
        public DbSet<ByMalePatronymic> ByMalePatronymics { get; set; }
        public DbSet<ByFemalePatronymic> ByFemalePatronymics { get; set; }
        public DbSet<BySurname> BySurnames { get; set; }

        public ByRecordsDbContext()
        {
            Database.EnsureCreated();
        }
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;user=root;password=375297374828qweR;database=by_region;",
                new MySqlServerVersion(new Version(8, 0, 27)));
        }
    }
}