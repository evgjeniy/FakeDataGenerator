using System;
using FakeDataGenerator.Models.RU;
using Microsoft.EntityFrameworkCore;

namespace FakeDataGenerator.Models.Contexts
{
    public class RuRecordsDbContext : DbContext
    {
        public DbSet<RuMaleName> RuMaleNames { get; set; }
        public DbSet<RuFemaleName> RuFemaleNames { get; set; }
        public DbSet<RuMalePatronymic> RuMalePatronymics { get; set; }
        public DbSet<RuFemalePatronymic> RuFemalePatronymics { get; set; }
        public DbSet<RuSurname> RuSurnames { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;user=root;password=375297374828qweR;database=ru_region;", 
                new MySqlServerVersion(new Version(8, 0, 27)));
        }
    }
}