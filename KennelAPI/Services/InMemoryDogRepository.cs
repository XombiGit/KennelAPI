using Common.Entities;
using Common.Interfaces;
using KennelAPI.Models;
using KennelAPI.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KennelAPI.Services
{
    public class InMemoryDogRepository : IDogRepository
    {
        List<IDogEntity> Dogs = new List<IDogEntity>();
        int Count;

        public InMemoryDogRepository()
        {
            populateData();
        }

        private void populateData()
        {
            var Dog1 = new InMemoryDogEntity() { Name = "Scooby", Breed = "Great Dane", Phone = "1234567",
                                         Email = "bob@hotmail.com", SpecialNotes = "Scares easily",
                                         XCoord = 23, YCoord = 25, ImageURL = "Testig not sure",
                                         DogID = "1" };
            var Dog2 = new InMemoryDogEntity() { Name = "Snowy", Breed = "Wire Fox Terrier", Phone = "1237567",
                                         Email = "jen@hotmail.com", SpecialNotes = "Very funny",
                                         XCoord = 89, YCoord = 77, ImageURL = "Hope this works",
                                         Reward = 100, DogID = "2" };
            var Dog3 = new InMemoryDogEntity() { Name = "Snoopy", Breed = "Beagle", Phone = "1834567",
                                         Email = "neena@hotmail.com", SpecialNotes = "Can do a happy dance",
                                         XCoord = 34, YCoord = 90, ImageURL = "Must research this",
                                         Reward = 380, DogID = "3" };
            var Dog4 = new InMemoryDogEntity() { Name = "Santa's Little Helper", Breed = "Greyhound", Phone = "1230567",
                                         Email = "cleo@hotmail.com", SpecialNotes = "Eats a lot of crap",
                                         XCoord = 53, YCoord = 83, ImageURL = "Should do a get call",
                                         DogID = "4" };
            var Dog5 = new InMemoryDogEntity() { Name = "Cosmo", Breed = "Labrador Retriever", Phone = "1234967",
                                         Email = "dana@hotmail.com", SpecialNotes = "Can read your mind",
                                         XCoord = 39, YCoord = 76, ImageURL = "Where to store image",
                                         Reward = 888, DogID = "5" };

            Dogs.Add(Dog1);
            Dogs.Add(Dog2);
            Dogs.Add(Dog3);
            Dogs.Add(Dog4);
            Dogs.Add(Dog5);
            Count = 5;
        }
        public void AddDog(IDogEntity dogEntity)
        {
            Count++;
            Dogs.Add(dogEntity);
        }

        public void DeleteDog(IDogEntity dogToDelete)
        {
            Dogs.Remove(dogToDelete);
        }

        public async Task<IDogEntity> GetDog(string dogId)
        {
            var dog = Dogs.Where(u => u.DogID == dogId).FirstOrDefault();
            return await Task.FromResult(dog);
        }

        public async void UpdateDog(IDogEntity dogToUpdate)
        {
            var existingDog = await GetDog(dogToUpdate.DogID);

            if (existingDog != null)
            {
                //todo
            }

            int index = Dogs.IndexOf(existingDog);

            if (index != -1)
            {
                Dogs[index] = dogToUpdate;
            }
        }
    }
}
