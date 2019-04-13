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
                      dogs.Remove(p);
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
            }
        };
    }

    public class DogControllerTests : IClassFixture<DogControllerFixture>
    {
        DogControllerFixture fixture;

        string token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiJiNzI5NjkzMi1lYmNhLTQwZTUtYmU2NS00NmRiNTk4MjNiNzgiLCJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkJhciIsImlhdCI6MTUxNjIzOTAyMn0.heN0pJcdyuTzqb7-J9CGKw8PfpqQLvYVFI-UBJot1Ds";
        string wrongUserIdToken = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiIxMjMiLCJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.4QHFhPNXB9eeEDMh0secAgB4KURbdvh1i_OOAYxf_Hw";

        public DogControllerTests(DogControllerFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task GetDog_TokenAuthorizedIdIsValidAndExists_ReturnOk()
        {
            string dogID = "1";

            //var mockedAccessor = new Mock<IAccessor>();
            var httpContext = new DefaultHttpContext(); // or mock a `HttpContext`
            httpContext.Request.Headers["Authorization"] = token; //Set header
                                                                      //Controller needs a controller context 
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            fixture.DogController.ControllerContext = controllerContext;

            var result = await fixture.DogController.GetDog(dogID);
            var okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetDog_TokenUnauthorizedIdIsValidAndExists_ReturnOk()
        {
            string dogID = "1";

            //var mockedAccessor = new Mock<IAccessor>();
            var httpContext = new DefaultHttpContext(); // or mock a `HttpContext`
            httpContext.Request.Headers["Authorization"] = wrongUserIdToken; //Set header
                                                                  //Controller needs a controller context 
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            fixture.DogController.ControllerContext = controllerContext;

            var result = await fixture.DogController.GetDog(dogID);
            var unauthorizedResult = result as UnauthorizedResult;

            Assert.NotNull(unauthorizedResult);
            Assert.Equal(401, unauthorizedResult.StatusCode);
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
        public async Task GetDog_IdIsInvalid_ReturnNotFoundAsync()
        {
            string dogID = "-1";

            var result = await fixture.DogController.GetDog(dogID);
            var notFoundResult = result as NotFoundResult;

            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task PostDog_ObjectIsNull_ReturnNotFoundAsync()
        {
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
        public async Task DeleteDog_ObjectIsValid_ReturnNoContentResult()
        {
            var dogToBeDeleted = new InMemoryDogEntity()
            {
                Name = "Test",
                Breed = "Test",
                Phone = "123",
                Email = "test@hotmail.com",
                SpecialNotes = "Scares easily",
                XCoord = 23,
                YCoord = 25,
                ImageURL = "Testig not sure",
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
        public async Task DeleteDog_ObjectIsInvalid_ReturnNotFound()
        {
            /*var dogToBeDeleted = new InMemoryDogEntity()
            {
                Name = "Test",
                Breed = "Test",
                Phone = "123",
                Email = "test@hotmail.com",
                SpecialNotes = "Scares easily",
                XCoord = 23,
                YCoord = 25,
                ImageURL = "Testig not sure",
                DogID = "999"
            };*/
            //fixture.dogs.Add(dogToBeDeleted);

            var results = await fixture.DogController.DeleteDog("100");
            var NotFoundResult = results as NotFoundResult;

            Assert.NotNull(NotFoundResult);
            Assert.Equal(404, NotFoundResult.StatusCode);

            //Should this be fixed
            //Assert.DoesNotContain(dogToBeDeleted, fixture.dogs);
        }
        //Test all put scenarios
        [Fact]
        public async Task PutDog_ObjectIsValid_ReturnNoContent()
        {
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
                DogID = "100"
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


            /*var results = await fixture.DogController.("999");
            var NoContentObjectResult = results as NoContentResult;

            Assert.NotNull(NoContentObjectResult);
            Assert.Equal(204, NoContentObjectResult.StatusCode);

            Assert.DoesNotContain(dogToBeDeleted, fixture.dogs);*/
        }

        [Fact]
        public async Task PutDog_ObjectIsNull_ReturnBadRequest()
        {
            var results = await fixture.DogController.PutDog("101", null);
            var badRequestResultObject = results as BadRequestResult;

            Assert.NotNull(badRequestResultObject);
            Assert.Equal(400, badRequestResultObject.StatusCode);
        }

        [Fact]
        public async Task PutDog_ObjectIsNull_ReturnNotFound()
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

            var results = await fixture.DogController.PutDog("102", dogToBeUpdated);
            var notFoundResultObject = results as NotFoundResult;

            Assert.NotNull(notFoundResultObject);
            Assert.Equal(404, notFoundResultObject.StatusCode);
        }

        [Fact]
        public async Task PutDog_IdIsNull_ReturnBadRequest()
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

            var results = await fixture.DogController.PutDog(null, dogToBeUpdated);
            var badRequestResultObject = results as BadRequestResult;

            Assert.NotNull(badRequestResultObject);
            Assert.Equal(400, badRequestResultObject.StatusCode);
        }


        [Fact]
        public async Task PutDog_NameBreaksMaxLength_ReturnBadRequest()
        {
            //Do we check the lenght or does the DogDtoUpate Entity ?
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
    }
}
