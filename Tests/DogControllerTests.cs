using AutoMapper;
using Common.Entities;
using Common.Interfaces;
using Common.Interfaces.Services;
using KennelAPI.Controllers;
using KennelAPI.Models;
using KennelAPI.Services;
using Microsoft.AspNetCore.Mvc;
using MongoPersistence.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class DogControllerFixture : IDisposable
    {
        public readonly Mock<IDogRepository> dogRepository = new Mock<IDogRepository>();

        public DogControllerFixture()
        {
            
            var emailService = new Mock<IMailService>();
            DogController = new DogController(dogRepository.Object, emailService.Object);

            dogRepository.Setup(x => x.GetDog("")).Returns(Task.FromResult<IDogEntity>(null));
            dogRepository.Setup(x => x.GetDog("1")).Returns(Task.FromResult<IDogEntity>(dogs[0]));
            dogRepository.Setup(x => x.GetDog("-1")).Returns(Task.FromResult<IDogEntity>(null));

            dogRepository.Setup(x => x.AddDog(null)).Returns(Task.FromResult<IDogEntity>(null));

            initializeMapper();
        }

        private void initializeMapper()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DogDtoCreation, DogEntity>();
                cfg.CreateMap<DogDtoCreation, InMemoryDogEntity>();
            });
        }


        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public DogController DogController { get; private set; }

        private void setupAutoMapper()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DogDtoCreation, DogEntity>();
            });
        }

        public List<IDogEntity> dogs = new List<IDogEntity>()
        {
             new InMemoryDogEntity()
            {
                Name = "Scooby",
                Breed = "Great Dane",
                Phone = "1234567",
                Email = "bob@hotmail.com",
                SpecialNotes = "Scares easily",
                XCoord = 23,
                YCoord = 25,
                ImageURL = "Testig not sure",
                DogID = "1"
            }
        };
    }

    public class DogControllerTests : IClassFixture<DogControllerFixture>
    {
        DogControllerFixture fixture;

        public DogControllerTests(DogControllerFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task GetDog_IdIsValidAndExists_ReturnOk()
        {
            string dogID = "1";

            var result = await fixture.DogController.GetDog(dogID);
            var okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetDog_IdIsNull_ReturnNotFoundAsync()
        {
            string dogID = null;

            var result = await fixture.DogController.GetDog(dogID);
            var notFoundResult = result as NotFoundResult;

            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task PostDog_ObjectIsNull_ReturnNotFoundAsync()
        {
            fixture.dogRepository.Setup(x => x.AddDog(It.IsAny<IDogEntity>())).Returns(Task.CompletedTask);

            //var dogCreation = new DogDtoCreation();
            var results = await fixture.DogController.PostDog(null);
            var badRequestResult = results as BadRequestResult;

            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        //Invalid post data

        [Fact]
        public async Task PostDog_ObjectIsValid_ReturnOk()
        {
            var dogCreation = new DogDtoCreation { Name = "Jack", Phone = "56940466", Email = "hello@owrod.com", ImageURL = "helslsdffk", OwnerID = 43645 };
            var dogEntity = Mapper.Map<InMemoryDogEntity>(dogCreation);
            fixture.dogRepository.Setup(repo => repo.AddDog(It.IsAny<IDogEntity>()))
                .Callback<IDogEntity>((p) =>
                    {
                        dogEntity.DogID = p.DogID;
                        fixture.dogs.Add(dogEntity);
                    })
                .Returns(Task.CompletedTask);

            var results = await fixture.DogController.PostDog(dogCreation);
            var okObjectResult = results as OkObjectResult;

            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);

            Assert.NotNull(dogEntity.DogID);
            Assert.Contains(dogEntity, fixture.dogs);
        }

        //Test all put scenarios

        //Test all delete scenario
    }
}
