using Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface ILocationRepository
    {
        Task<List<ILocationEntity>> GetNearbyDogs(string dogId);
        void UpdateLocation(ILocationEntity locationToUpdate);
    }
}
