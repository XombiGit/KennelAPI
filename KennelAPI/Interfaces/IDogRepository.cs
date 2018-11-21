using KennelAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KennelAPI.Interfaces
{
    public interface IDogRepository
    {
        DogDto GetDog(int dogId);
        void AddDog(string name, string phone, string email, string breed, string notes, int x, int y, int reward, string imageURL);
        void DeleteDog(DogDto dogToDelete);
        void UpdateDog(DogDto dogToUpdate);
    }
}
