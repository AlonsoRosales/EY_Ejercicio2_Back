using Microsoft.EntityFrameworkCore;

namespace Back.Models
{
    public class ApiDbContext:DbContext
    {   
        public ApiDbContext(DbContextOptions options):base(options) { 
            
        
        }

        public DbSet<Proveedor> Proveedors { get; set;}
       
    }
}
