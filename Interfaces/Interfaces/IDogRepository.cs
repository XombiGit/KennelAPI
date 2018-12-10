using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IDogRepository
    {
        Task<IDogEntity> GetDog(string dogId);
        void AddDog(IDogEntity dogEntity);
        void DeleteDog(IDogEntity dogToDelete);
        void UpdateDog(IDogEntity dogToUpdate);
    }
}
