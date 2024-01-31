using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Data
{

    public class ApplicationDbContext : DbContext 
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa{
                    Id=1,
                    Name="Royal Villa",
                    Details = "",
                    ImageUrl = "",
                    Occupancy = 5,
                    Rate = 200,
                    SqFt = 550,
                    Amenity = ""
                }
            );
        }

    }

}