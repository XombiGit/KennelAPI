using KennelAPI.Interfaces;
using KennelAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KennelAPI.Services
{
    public class InMemoryDogRepository : IDogRepository
    {
        List<DogDto> Dogs = new List<DogDto>();
        int Count;

        public InMemoryDogRepository()
        {
            populateData();
        }

        private void populateData()
        {
            DogDto Dog1 = new DogDto() { Name = "Scooby", Breed = "Great Dane", Phone = "1234567",
                                         Email = "bob@hotmail.com", SpecialNotes = "Scares easily",
                                         XCoord = 23, YCoord = 25, ImageURL = "Testig not sure",
                                         DogID = 1 };
            DogDto Dog2 = new DogDto() { Name = "Snowy", Breed = "Wire Fox Terrier", Phone = "1237567",
                                         Email = "jen@hotmail.com", SpecialNotes = "Very funny",
                                         XCoord = 89, YCoord = 77, ImageURL = "Hope this works",
                                         Reward = 100, DogID = 2 };
            DogDto Dog3 = new DogDto() { Name = "Snoopy", Breed = "Beagle", Phone = "1834567",
                                         Email = "neena@hotmail.com", SpecialNotes = "Can do a happy dance",
                                         XCoord = 34, YCoord = 90, ImageURL = "Must research this",
                                         Reward = 380, DogID = 3 };
            DogDto Dog4 = new DogDto() { Name = "Santa's Little Helper", Breed = "Greyhound", Phone = "1230567",
                                         Email = "cleo@hotmail.com", SpecialNotes = "Eats a lot of crap",
                                         XCoord = 53, YCoord = 83, ImageURL = "Should do a get call",
                                         DogID = 4 };
            DogDto Dog5 = new DogDto() { Name = "Cosmo", Breed = "Labrador Retriever", Phone = "1234967",
                                         Email = "dana@hotmail.com", SpecialNotes = "Can read your mind",
                                         XCoord = 39, YCoord = 76, ImageURL = "Where to store image",
                                         Reward = 888, DogID = 5 };

            Dogs.Add(Dog1);
            Dogs.Add(Dog2);
            Dogs.Add(Dog3);
            Dogs.Add(Dog4);
            Dogs.Add(Dog5);
            Count = 5;
        }
        public void AddDog(string name, string breed, string phone, string email, string notes, int x, int y, int reward, string imageURL)
        {
            Count++;
            Dogs.Add(new DogDto() { DogID = Count, Name = name, Phone = phone, Email = email,
                                    SpecialNotes = notes, XCoord = x, YCoord = y, Breed = breed,
                                    Reward = reward, ImageURL = imageURL});
        }

        public void DeleteDog(DogDto dogToDelete)
        {
            Dogs.Remove(dogToDelete);
        }

        public DogDto GetDog(int dogId)
        {
            return Dogs.Where(u => u.DogID == dogId).FirstOrDefault();
        }

        public void UpdateDog(DogDto dogToUpdate)
        {
            var existingDog = GetDog(dogToUpdate.DogID);

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
