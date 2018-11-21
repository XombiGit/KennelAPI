using KennelAPI.Interfaces;
using KennelAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KennelAPI.Services
{
    public class InMemoryUserRepository : IUserRepository
    {
        List<UserDto> Users = new List<UserDto>();
        int Count;

        public InMemoryUserRepository()
        {
            populateData();
        }

        private void populateData()
        {
            UserDto User1 = new UserDto() { Name = "Bob", Phone = "1234567", Email = "bob@hotmail.com", UserID = 1};
            UserDto User2 = new UserDto() { Name = "Jen", Phone = "1237567", Email = "jen@hotmail.com", UserID = 2 };
            UserDto User3 = new UserDto() { Name = "Neena", Phone = "1834567", Email = "neena@hotmail.com", UserID = 3 };
            UserDto User4 = new UserDto() { Name = "Cleo", Phone = "1230567", Email = "cleo@hotmail.com", UserID = 4 };
            UserDto User5 = new UserDto() { Name = "Dana", Phone = "1234967", Email = "dana@hotmail.com", UserID = 5 };

            Users.Add(User1);
            Users.Add(User2);
            Users.Add(User3);
            Users.Add(User4);
            Users.Add(User5);
            Count = 5;
        }

        public void AddUser(string name, string phone, string email)
        {
            Count++;
            Users.Add(new UserDto() { UserID = Count, Name = name, Phone = phone, Email = email });
        }
        public void DeleteUser(UserDto user)
        {
            Users.Remove(user);
        }

        public UserDto GetUser(int userId)
        {
            return Users.Where(u => u.UserID == userId).FirstOrDefault();
        }

        public void UpdateUser(UserDto userToUpdate)
        {
            var existingUser = GetUser(userToUpdate.UserID);

            if(existingUser != null)
            {
                //todo
            }

            int index = Users.IndexOf(existingUser);

            if(index != -1)
            {
                Users[index] = userToUpdate;
            }
        }
    }
}
