using Microsoft.EntityFrameworkCore;
using PimpMyRide.Core.Data.Models;

namespace PimpMyRide.Core.Data.Context
{
    public class PimpMyRideDbContext : DbContext
    {
        public PimpMyRideDbContext(DbContextOptions<PimpMyRideDbContext> options) : base(options) { }

        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<EngineType> EngineTypes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<RentCar> RentCars { get; set; }
    }
}
