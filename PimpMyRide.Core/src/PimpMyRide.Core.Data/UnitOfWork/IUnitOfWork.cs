using System;
using System.Threading;
using System.Threading.Tasks;
using PimpMyRide.Core.Data.Models;
using PimpMyRide.Core.Data.Repository;

namespace PimpMyRide.Core.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Car> CarRepository { get; }
        IRepository<Manufacturer> ManufacturerRepository { get; }
        IRepository<EngineType> EngineTypeRepository { get; }
        void Save();
        Task SaveAsync(CancellationToken cancellationToken);
    }
}
