using System.Threading;
using System.Threading.Tasks;
using PimpMyRide.Core.Data.Context;
using PimpMyRide.Core.Data.Models;
using PimpMyRide.Core.Data.Repository;

namespace PimpMyRide.Core.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PimpMyRideDbContext _context;

        private Repository<Car> _carRepository;
        private Repository<Manufacturer> _manufacturerRepository;
        private Repository<EngineType> _engineTypeRepository;

        public IRepository<Car> CarRepository => _carRepository ?? (_carRepository = new Repository<Car>(_context));
        public IRepository<Manufacturer> ManufacturerRepository => _manufacturerRepository ?? (_manufacturerRepository = new Repository<Manufacturer>(_context));
        public IRepository<EngineType> EngineTypeRepository => _engineTypeRepository ?? (_engineTypeRepository = new Repository<EngineType>(_context));

        public UnitOfWork(PimpMyRideDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
