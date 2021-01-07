using Common.Entities;
using Common.Interfaces;
using Common.Interfaces.Services;
using KennelAPI.Controllers;
using KennelAPI.Models;
using KennelAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoPersistence.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class UserControllerFixture : IDisposable
    {
        public readonly Mock<IUserRepository> userRepository = new Mock<IUserRepository>();

        public UserControllerFixture()
        {
            var emailService = new Mock<IMailService>();
            UserController = new UserController(userRepository.Object);

            userRepository.Setup(x => x.GetUser(It.IsAny<string>())).Returns((string id) => Task.FromResult<IUserEntity>(users.Find(d => d.UserID == id)));
            userRepository.Setup(x => x.AddUser(null)).Returns(Task.FromResult<IUserEntity>(null));

            userRepository.Setup(repo => repo.AddUser(It.IsAny<IUserEntity>()))
                .Callback<IUserEntity>((p) =>
                {
                    users.Add(p);
                })
                .Returns(Task.CompletedTask);

            userRepository.Setup(repo => repo.DeleteUser(It.IsAny<IUserEntity>()))
                  .Callback<IUserEntity>((p) =>
                  {
                      users.Remove(users.Find(user => user.UserID == p.UserID));
                  });

            userRepository.Setup(repo => repo.UpdateUser(It.IsAny<IUserEntity>()))
                  .Callback<IUserEntity>((p) =>
                  {
                      users.Remove(users.Find(user => user.UserID == p.UserID));
                      users.Add(p);
                  });


            initializeMapper();
        }

        private void initializeMapper()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<UserDtoCreation, UserEntity>();
                cfg.CreateMap<UserDtoCreation, InMemoryUserEntity>();
            });
        }


        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public UserController UserController { get; private set; }

        private void setupAutoMapper()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<UserDtoCreation, UserEntity>();
            });
        }

        public List<IUserEntity> users = new List<IUserEntity>()
        {
            new InMemoryUserEntity()
            {
                UserID = "1",
                Name = "Daphne",
                Phone = "9092093123",
                Email = "daphneblake@hotmail.com",
            },

            new InMemoryUserEntity()
            {
                UserID = "b7296932-ebca-40e5-be65-46db59823b78",
                Name = "Carol",
                Phone = "9090233413",
                Email = "caroldanvers@hotmail.com"
            }
        };
    }
    public class UserControllerTests : IClassFixture<UserControllerFixture>
    {
        UserControllerFixture fixture;

        const string correctToken = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiJiNzI5NjkzMi1lYmNhLTQwZTUtYmU2NS00NmRiNTk4MjNiNzgiLCJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkJhciIsImlhdCI6MTUxNjIzOTAyMn0.heN0pJcdyuTzqb7-J9CGKw8PfpqQLvYVFI-UBJot1Ds";
        const string nullToken = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiIiLCJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkJhciIsImlhdCI6MTUxNjIzOTAyMn0.44WLOM4P8YhXgbn1incJlLnhpcF_9TuBkBa9OIpicLI";
        const string wrongToken = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiIxMjMiLCJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.4QHFhPNXB9eeEDMh0secAgB4KURbdvh1i_OOAYxf_Hw";
        const string correctTokenNullUser = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiJ4OTg3ODUyMS1hbG9lLTM4djktZ2gzNC01MG5tODk2Nzh4OTAiLCJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkJhciIsImlhdCI6MTUxNjIzOTAyMn0.lueTui5E4waFU_fJg_q-_I7VbkuKKLjkRWxQUiQTm9o";

        static readonly UserDtoCreation userCreationInvalidModelState = new UserDtoCreation()
        {
            Name = null,
            Phone = "UpdatedPhone",
            Email = "hello@hotmail.com"
        };

        static readonly UserDtoCreation userCreationValidModelState = new UserDtoCreation()
        {
            Name = "Greer Nelson",
            Phone = "9092352234",
            Email = "tigra@hotmail.com"
        };

        static readonly UserDtoUpdate userUpdateValidModelState = new UserDtoUpdate()
        {
            Name = "Greer Nelson",
            Phone = "3330093210",
            Email = "tigra@hotmail.com"
        };

        public UserControllerTests(UserControllerFixture fixture)
        {
            this.fixture = fixture;
        }

        [Theory]
        [InlineData(null, typeof(UnauthorizedObjectResult), 401)]
        [InlineData(nullToken, typeof(UnauthorizedObjectResult), 401)]
        [InlineData(correctToken, typeof(OkObjectResult), 200)]
        [InlineData(wrongToken, typeof(UnauthorizedObjectResult), 401)]
        public async Task GetUser_TokenAuthorizedIdIsValidAndExists_ReturnOk(string token, Type resultType, int status)
        {
            string userID = "b7296932-ebca-40e5-be65-46db59823b78";
    
            SetContext(token);

            var result = await fixture.UserController.GetUser(userID);
            Assert.IsType(resultType, result);

            var finalResult = result as ObjectResult;
            Assert.Equal(status, finalResult.StatusCode);
        }

        [Fact]
        public async Task GetUser_ValidUserTokenDoesNotExist_ReturnNotFound()
        {
            string userID = "x9878521-aloe-38v9-gh34-50nm89678x90";

            SetContext(correctTokenNullUser);

            var result = await fixture.UserController.GetUser(userID);
            Assert.IsType<NotFoundResult>(result);

            var finalResult = result as NotFoundResult;
            Assert.Equal(404, finalResult.StatusCode);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task AddUser_TokenAuthorizedIdIsValidAndExists_ReturnOk(UserDtoCreation userCreated, Type resultType, int status)
        {
            //string userID = "b7296932-ebca-40e5-be65-46db59823b78";

            //SetContext(userCreated);

            var result = await fixture.UserController.AddUser(userCreated);
            Assert.IsType(resultType, result);

            var finalResult = result as ObjectResult;
            Assert.Equal(status, finalResult.StatusCode);
        }

        public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            //expecting NotFoundObject but get NotFound, can't return NotFoundObjectResult
            //new object[] { null, typeof(NotFoundObjectResult), 404 },
            new object[] { userCreationValidModelState, typeof(OkObjectResult), 200 },
        };

        [Fact]
        public async Task AddUser_TokenAuthorizedIdIsValidAndInvalidModelState_ReturnBadRequest()
        {
            //string userID = "b7296932-ebca-40e5-be65-46db59823b78";

            //SetContext(userCreated);

            fixture.UserController.ModelState.AddModelError("Name", "NameRequired");
            var result = await fixture.UserController.AddUser(userCreationInvalidModelState);
            fixture.UserController.ModelState.Clear();
            Assert.IsType<BadRequestObjectResult>(result);

            var finalResult = result as ObjectResult;
            Assert.Equal(400, finalResult.StatusCode);
        }

        [Theory]
        [InlineData(null, typeof(UnauthorizedObjectResult), 401)]
        [InlineData(nullToken, typeof(UnauthorizedObjectResult), 401)]
        //[InlineData(correctToken, typeof(NoContentResult), 204)]
        [InlineData(wrongToken, typeof(UnauthorizedObjectResult), 401)]
        public async Task PutUser_TokenAuthorizedIdIsValidAndExists_ReturnOk(string token, Type resultType, int status)
        {
            string userID = "b7296932-ebca-40e5-be65-46db59823b78";

            SetContext(token);

            var result = await fixture.UserController.UpdateUsers(userID, userUpdateValidModelState);
            Assert.IsType(resultType, result);

            var finalResult = result as ObjectResult;
            Assert.Equal(status, finalResult.StatusCode);
        }

        [Theory]
        [MemberData(nameof(PutData))]
        public async Task PutUser_TokenAuthorizedIdIsValidAndValidModelState_ReturnBadRequest(UserDtoUpdate userUpdated, Type resultType, int status)
        {
            string userID = "b7296932-ebca-40e5-be65-46db59823b78";
            //How to set Context from userDto object 
            SetContext(correctToken);

            var result = await fixture.UserController.UpdateUsers(userID, userUpdated);
            Assert.IsType(resultType, result);

            var finalResult = result as ObjectResult;
            Assert.Equal(status, finalResult.StatusCode);
        }

        public static IEnumerable<object[]> PutData =>
        new List<object[]>
        {
            new object[] { null, typeof(BadRequestObjectResult), 400 },
            //NoContentObjectResult does not exist
            //new object[] { userUpdateValidModelState, typeof(NoContentObjectResult), 204 },
        };

        [Fact]
        public async Task PutUser_TokenAuthorizedIdIsValidAndInvalidModelState_ReturnBadRequest()
        {
            string userID = "b7296932-ebca-40e5-be65-46db59823b78";

            SetContext(correctToken);

            fixture.UserController.ModelState.AddModelError("Phone", "MaxLengthExceeded");
            var result = await fixture.UserController.UpdateUsers(userID, userUpdateValidModelState);
            fixture.UserController.ModelState.Clear();
            Assert.IsType<BadRequestObjectResult>(result);

            var finalResult = result as ObjectResult;
            Assert.Equal(400, finalResult.StatusCode);
        }

        [Theory]
        [MemberData(nameof(DeleteData))]
        public async Task DeleteUser_TokenAuthorizedIdIsValidAndValidModelState_ReturnBadRequest(string userID, Type resultType, int status)
        {
            //string userID = "b7296932-ebca-40e5-be65-46db59823b78";
            //How to set Context from userDto object 
            //SetContext(correctToken);

            var result = await fixture.UserController.DeleteUser(userID);
            Assert.IsType(resultType, result);

            var finalResult = result as ObjectResult;
            Assert.Equal(status, finalResult.StatusCode);
        }

        public static IEnumerable<object[]> DeleteData =>
        new List<object[]>
        {
            //Is null bad request or not found ?
            new object[] { null, typeof(BadRequestObjectResult), 400 },
            //NoContentObjectResult does not exist
            new object[] { "no-user-associated", typeof(UnauthorizedObjectResult), 401 },
        };

        private void SetContext(string token)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = token;

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            fixture.UserController.ControllerContext = controllerContext;
        }
    }
}
