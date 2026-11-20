using Microsoft.EntityFrameworkCore;

using CarRental.Models;



namespace CarRental.Data

{

    public class AppDbContext : DbContext

    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)

        {

        }




        // Mapea la clase Usuario a la tabla 'Usuarios' en MySQL/MariaDB 
            
        public DbSet<Usuario> Usuarios { get; set; }

    }

}