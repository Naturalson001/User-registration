using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


namespace User_Registration.Data
{
	public class ApplicationDbContext:DbContext
	{
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(string connectionString) 
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;Database=Mydatabase username= root;port=3306;password=Ayevbosaimade1");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ensure email addresses are unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}

