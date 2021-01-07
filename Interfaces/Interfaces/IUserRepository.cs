using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IUserRepository
    {
        Task<IUserEntity> GetUser(string userId);
        Task AddUser(IUserEntity userEntity);
        void DeleteUser(IUserEntity dogEntity);
        void UpdateUser(IUserEntity dogToUpdate);
    }
}
