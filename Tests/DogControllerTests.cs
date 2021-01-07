using AutoMapper;
using Common.Entities;
using Common.Interfaces;
using Common.Interfaces.Services;
using KennelAPI;
using KennelAPI.Controllers;
using KennelAPI.Models;
using KennelAPI.Services;
using Microsoft.AspNetCore.Http;
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

            dogRepository.Setup(x => x.GetDog(It.IsAny<string>())).Returns((string id) => Task.FromResult<IDogEntity>(dogs.Find(d => d.DogID == id)));
            dogRepository.Setup(x => x.AddDog(null)).Returns(Task.FromResult<IDogEntity>(null));

            dogRepository.Setup(repo => repo.AddDog(It.IsAny<IDogEntity>()))
                .Callback<IDogEntity>((p) =>
                {
                    dogs.Add(p);
                })
                .Returns(Task.CompletedTask);

            dogRepository.Setup(repo => repo.DeleteDog(It.IsAny<IDogEntity>()))
                  .Callback<IDogEntity>((p) =>
                  {
                      dogs.Remove(dogs.Find(dog => dog.DogID == p.DogID));
                  });

            dogRepository.Setup(repo => repo.UpdateDog(It.IsAny<IDogEntity>()))
                  .Callback<IDogEntity>((p) =>
                  {
                      dogs.Remove(dogs.Find(dog => dog.DogID == p.DogID));
                      dogs.Add(p);
                  });


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
                DogID = "1",
                OwnerID = "b7296932-ebca-40e5-be65-46db59823b78",
            },
        
            new InMemoryDogEntity()
            {
                Name = "Goose",
                Breed = "Flerken",
                Phone = "9090231789",
                Email = "caroldanvers@hotmail.com",
                SpecialNotes = "Caution: keep back",
                XCoord = 95,
                YCoord = 75,
                ImageURL = "Won't work for now",
                DogID = "21",
                OwnerID = "",
            }
        };
    }

    public class DogControllerTests : IClassFixture<DogControllerFixture>
    {
        DogControllerFixture fixture;

        string correctToken = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiJiNzI5NjkzMi1lYmNhLTQwZTUtYmU2NS00NmRiNTk4MjNiNzgiLCJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkJhciIsImlhdCI6MTUxNjIzOTAyMn0.heN0pJcdyuTzqb7-J9CGKw8PfpqQLvYVFI-UBJot1Ds";
        //huh ?
        string wrongToken = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiIxMjMiLCJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.4QHFhPNXB9eeEDMh0secAgB4KURbdvh1i_OOAYxf_Hw";

        public DogControllerTests(DogControllerFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task GetDog_TokenAuthorizedIdIsValidAndExists_ReturnOk()
        {
            string dogID = "1";

            SetContext(correctToken);

            var result = await fixture.DogController.GetDog(dogID);
            var okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
        }
        
        [Fact]
        public async Task GetDog_TokenUnauthorizedIdIsMismatched_ReturnUnauthorized()
        {
            string dogID = "1";

            SetContext(wrongToken);

            var result = await fixture.DogController.GetDog(dogID);
            var unauthorizedResult = result as UnauthorizedResult;

            Assert.NotNull(unauthorizedResult);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        //need to create a token with null userID but how ?
        [Fact]
        public async Task GetDog_IdIsNull_ReturnUnauthorized()
        {
            string dogID = "21";

            SetContext(correctToken);

            var result = await fixture.DogController.GetDog(dogID);
            var unauthorizedResult = result as UnauthorizedResult;

            Assert.NotNull(unauthorizedResult);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        [Fact]
        public async Task GetDog_ObjectIsNull_ReturnNotFoundAsync()
        {
            string dogID = "-1";

            SetContext(correctToken);

            var result = await fixture.DogController.GetDog(dogID);
            var notFoundResult = result as NotFoundResult;

            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        /*[Fact] Does Post need to check the token ?
        public async Task PostDog_TokenUnauthorizedIdIsMismatched_ReturnUnauthorized()
        {
            var dogCreation = new DogDtoCreation { Name = "Jill", Phone = "56940466", Email = "hello@owrod.com", ImageURL = "helslsdffk", OwnerID = Guid.NewGuid().ToString() };
            var dogEntity = Mapper.Map<InMemoryDogEntity>(dogCreation);

            SetContext(wrongToken);

            var result = await fixture.DogController.PostDog(dogCreation);
            var unauthorizedResult = result as UnauthorizedResult;

            Assert.NotNull(unauthorizedResult);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }*/

        [Fact]
        public async Task PostDog_ObjectIsNull_ReturnBadRequest()
        {
            SetContext(correctToken);

            var results = await fixture.DogController.PostDog(null);
            var badRequestResult = results as BadRequestResult;

            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task PostDog_EmailIsNull_ReturnBadRequest()
        {
            SetContext(correctToken);

            var dogToBeCreated = new DogDtoCreation()
            {
                Name = "Thisisover10characters",
                Breed = "UpdatedBreed",
                Phone = "UpdatedPhone",
                Email = "",
                SpecialNotes = "UpdatedNotes",
                XCoord = 99,
                YCoord = 99,
                ImageURL = "UpdatedImage"
            };

            fixture.DogController.ModelState.AddModelError("Email", "EmailRequired");
            var results = await fixture.DogController.PostDog(dogToBeCreated);
            fixture.DogController.ModelState.Clear();

            var badRequestResultObject = results as BadRequestResult;

            Assert.NotNull(badRequestResultObject);
            Assert.Equal(400, badRequestResultObject.StatusCode);
        }

        [Fact]
        public async Task PostDog_ObjectIsValid_ReturnOk()
        {
            SetContext(correctToken);

            var dogCreation = new DogDtoCreation { Name = "Jack", Phone = "56940466", Email = "hello@owrod.com", ImageURL = "helslsdffk", OwnerID = Guid.NewGuid().ToString() };
            var dogEntity = Mapper.Map<InMemoryDogEntity>(dogCreation);
         
            var results = await fixture.DogController.PostDog(dogCreation);
            var okObjectResult = results as OkObjectResult;

            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);

            var dogEntityResult = (IDogEntity)okObjectResult.Value;

            Assert.NotNull(dogEntityResult);
            Assert.Contains(dogEntityResult, fixture.dogs);
        }

        [Fact]
        public async Task DeleteDog_TokenUnauthorizedIdIsMismatched_ReturnUnauthorized()
        {
            string dogID = "1";

            SetContext(wrongToken);

            var result = await fixture.DogController.DeleteDog(dogID);
            var unauthorizedResult = result as UnauthorizedResult;

            Assert.NotNull(unauthorizedResult);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        [Fact]
        public async Task DeleteDog_ObjectIsValid_ReturnNoContentResult()
        {
            SetContext(correctToken);

            var dogToBeDeleted = new InMemoryDogEntity()
            {
                Name = "Test",
                Breed = "Test",
                Phone = "123",
                Email = "test@hotmail.com",
                SpecialNotes = "Scares easily",
                XCoord = 23,
                YCoord = 25,
                ImageURL = "Testing not sure",
                DogID = "999",
                OwnerID = "b7296932-ebca-40e5-be65-46db59823b78"
            };
            fixture.dogs.Add(dogToBeDeleted);

            var results = await fixture.DogController.DeleteDog("999");
            var NoContentObjectResult = results as NoContentResult;

            Assert.NotNull(NoContentObjectResult);
            Assert.Equal(204, NoContentObjectResult.StatusCode);

            Assert.DoesNotContain(dogToBeDeleted, fixture.dogs);
        }

        [Fact]
        public async Task DeleteDog_IdIsNull_ReturnBadRequest()
        {
            SetContext(correctToken);

            var results = await fixture.DogController.DeleteDog(null);
            var badRequestResult = results as BadRequestResult;

            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task DeleteDog_ObjectIsNull_ReturnNotFound()
        {
            SetContext(correctToken);

            var results = await fixture.DogController.DeleteDog("103");
            var NotFoundResult = results as NotFoundResult;

            Assert.NotNull(NotFoundResult);
            Assert.Equal(404, NotFoundResult.StatusCode);
        }

        [Fact]
        public async Task PutDog_TokenUnauthorizedIdIsMismatched_ReturnUnauthorized()
        {
            var dogToBeUpdated = new DogDtoUpdate()
            {
                Name = "UpdatedName",
                Breed = "UpdatedBreed",
                Phone = "UpdatedPhone",
                Email = "UpdatedEmail",
                SpecialNotes = "UpdatedNotes",
                XCoord = 99,
                YCoord = 99,
                ImageURL = "UpdatedImage"
            };

            SetContext(wrongToken);

            var result = await fixture.DogController.PutDog("1", dogToBeUpdated);
            var unauthorizedResult = result as UnauthorizedResult;

            Assert.NotNull(unauthorizedResult);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        [Fact]
        public async Task PutDog_ObjectIsValid_ReturnNoContent()
        {
            SetContext(correctToken);

            var dogToBeAdded = new InMemoryDogEntity()
            {
                Name = "Silver",
                Breed = "Shih-Tzu",
                Phone = "902-294-2036",
                Email = "silver@hotmail.com",
                SpecialNotes = "good boy",
                XCoord = 10,
                YCoord = 10,
                ImageURL = "old photos",
                DogID = "100",
                OwnerID = "b7296932-ebca-40e5-be65-46db59823b78"
            };
            fixture.dogs.Add(dogToBeAdded);

            var dogToBeUpdated = new DogDtoUpdate()
            {
                Name = "UpdatedName",
                Breed = "UpdatedBreed",
                Phone = "UpdatedPhone",
                Email = "UpdatedEmail",
                SpecialNotes = "UpdatedNotes",
                XCoord = 99,
                YCoord = 99,
                ImageURL = "UpdatedImage"
            };

            var results = await fixture.DogController.PutDog("100", dogToBeUpdated);
            var NoContentResultObject = results as NoContentResult;

            Assert.NotNull(NoContentResultObject);
            Assert.Equal(204, NoContentResultObject.StatusCode);

            var result =  fixture.dogs.Find(dog => dog.DogID == "100");

            Assert.NotNull(result);
            Assert.Equal("UpdatedName", result.Name);
            Assert.Equal("UpdatedBreed", result.Breed);
        }

        [Fact]
        public async Task PutDog_ObjectIsNull_ReturnBadRequest()
        {
            SetContext(correctToken);

            var results = await fixture.DogController.PutDog("101", null);
            var badRequestResultObject = results as BadRequestResult;

            Assert.NotNull(badRequestResultObject);
            Assert.Equal(400, badRequestResultObject.StatusCode);
        }

        [Fact]
        public async Task PutDog_ObjectIsNull_ReturnNotFound()
        {
            SetContext(correctToken);

            var dogToBeUpdated = new DogDtoUpdate()
            {
                Name = "UpdatedName",
                Breed = "UpdatedBreed",
                Phone = "UpdatedPhone",
                Email = "UpdatedEmail",
                SpecialNotes = "UpdatedNotes",
                XCoord = 99,
                YCoord = 99,
                ImageURL = "UpdatedImage"
            };

            var results = await fixture.DogController.PutDog("102", dogToBeUpdated);
            var notFoundResultObject = results as NotFoundResult;

            Assert.NotNull(notFoundResultObject);
            Assert.Equal(404, notFoundResultObject.StatusCode);
        }

        [Fact]
        public async Task PutDog_IdIsNull_ReturnBadRequest()
        {
            SetContext(correctToken);

            var dogToBeUpdated = new DogDtoUpdate()
            {
                Name = "UpdatedName",
                Breed = "UpdatedBreed",
                Phone = "UpdatedPhone",
                Email = "UpdatedEmail",
                SpecialNotes = "UpdatedNotes",
                XCoord = 99,
                YCoord = 99,
                ImageURL = "UpdatedImage"
            };

            var results = await fixture.DogController.PutDog(null, dogToBeUpdated);
            var badRequestResultObject = results as BadRequestResult;

            Assert.NotNull(badRequestResultObject);
            Assert.Equal(400, badRequestResultObject.StatusCode);
        }


        [Fact]
        public async Task PutDog_NameBreaksMaxLength_ReturnBadRequest()
        {
            SetContext(correctToken);

            var dogToBeUpdated = new DogDtoUpdate()
            {
                Name = "Thisisover10characters",
                Breed = "UpdatedBreed",
                Phone = "UpdatedPhone",
                Email = "UpdatedEmail",
                SpecialNotes = "UpdatedNotes",
                XCoord = 99,
                YCoord = 99,
                ImageURL = "UpdatedImage"
            };

            fixture.DogController.ModelState.AddModelError("Name", "MaxLengthExceeded");
            var results = await fixture.DogController.PutDog("100", dogToBeUpdated);
            fixture.DogController.ModelState.Clear();

            var badRequestResultObject = results as BadRequestResult;

            Assert.NotNull(badRequestResultObject);
            Assert.Equal(400, badRequestResultObject.StatusCode);
        }

        private void SetContext(string token)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = token;

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            fixture.DogController.ControllerContext = controllerContext;
        }
    }
}
