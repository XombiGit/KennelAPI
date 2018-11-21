using KennelAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KennelAPI.Interfaces
{
    public interface IUserRepository
    {
        UserDto GetUser(int userId);
        void DeleteUser(UserDto userToDelete);
        void UpdateUser(UserDto userToUpdate);
        void AddUser(string name, string phone, string email);
    }
}
