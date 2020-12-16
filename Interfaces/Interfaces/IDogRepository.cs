using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IDogRepository
    {
        Task<List<IDogEntity>> GetAllDogs(string ownerId);
        Task<IDogEntity> GetDog(string dogId);
        Task AddDog(IDogEntity dogEntity);
        Task<bool> DeleteDog(IDogEntity dogEntity);
        void UpdateDog(IDogEntity dogToUpdate);
    }
}
