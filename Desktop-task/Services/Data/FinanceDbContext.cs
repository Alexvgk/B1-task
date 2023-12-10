using Desktop_task.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_task.Services.Data
{
    public class FinanceDbContext : DbContext
    {
        public DbSet<Model.File> Files { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Finance> Finances { get; set; }
        public DbSet<Model.Data> Data { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Bank> Banks { get; set; }

        public FinanceDbContext() { }

        public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Model.Data>()
     .Property(d => d.ActivIncomSaldo)
     .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Model.Data>()
                .Property(d => d.PassivIncomSaldo)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Model.Data>()
                .Property(d => d.Debit)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Model.Data>()
                .Property(d => d.Credit)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Model.Data>()
                .Property(d => d.ActivOutcomSaldo)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Model.Data>()
                .Property(d => d.PassOutcomSaldo)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
