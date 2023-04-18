using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace User_Registration.Data
{
    public class ApplicationDbContext : DbContext
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        { }


        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(string connectionString) 
        {
        }


        public void ConfigureServices(IServiceCollection services)
            => services.AddDbContext<ApplicationDbContext>();

     
        
    }
}

