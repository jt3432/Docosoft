using Docosoft.API.Controllers.v1;
using Docosoft.API.Core.Managers;
using Docosoft.API.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Docosoft.Tests
{
    public class DocosoftUserController_Tests
    {
        [Fact]
        private void GetAll_ShouldReturnAllUsers_DocosoftUsers()
        {
            //arrange            
            var docosoftUserManagerMock = new Mock<IDocosoftUserManager>();
            docosoftUserManagerMock.Setup(m => m.GetAllUsers()).ReturnsAsync(GetInitiaUsers());
            var docosoftUserController = DocosoftUserControllerInit(docosoftUserManagerMock);

            //act
            var result = docosoftUserController.GetAll()?.Result.Result;
            var okResult = result as OkObjectResult;
            IEnumerable<DocosoftUser>? value = okResult?.Value as IEnumerable<DocosoftUser>;

            //assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.NotNull(value);                      
            Assert.Equal(2, value.Count());
        }



        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        private void Get_ShouldReturnAUserById_DocosoftUser(int id)
        {
            //arrange            
            var docosoftUserManagerMock = new Mock<IDocosoftUserManager>();
            docosoftUserManagerMock.Setup(m => m.GetUser(It.IsAny<int>())).ReturnsAsync(GetInitiaUsers().FirstOrDefault(u => u.Id == id));
            var docosoftUserController = DocosoftUserControllerInit(docosoftUserManagerMock);

            //act
            var result = docosoftUserController.GetUser(id)?.Result.Result;
            var okResult = result as OkObjectResult;
            DocosoftUser? value = okResult?.Value as DocosoftUser;

            //assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.NotNull(value);
            Assert.Equal(id, value.Id);
        }

        [Fact]
        private void Post_ShouldAddUserToDatabase_DocosoftUser()
        {
            //arrange            
            DocosoftUser newUser = new DocosoftUser()
            {
                Id = 0,
                FirstName = "Peter",
                LastName = "Piper",
                Title = "Developer",
                Email = "peter.piper@docosoft.com",
                Created = DateTime.Now
            };

            var docosoftUserManagerMock = new Mock<IDocosoftUserManager>();
            docosoftUserManagerMock.Setup(m => m.AddUser(It.IsAny<DocosoftUser>())).ReturnsAsync((DocosoftUser newUser) => { newUser.Id = 3; return newUser; });
            var docosoftUserController = DocosoftUserControllerInit(docosoftUserManagerMock);

            //act
            var result = docosoftUserController.AddUser(newUser)?.Result.Result;
            var okResult = result as OkObjectResult;
            DocosoftUser? value = okResult?.Value as DocosoftUser;

            //assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.NotNull(value);
            Assert.True(value.Id > 0);
        }

        [Fact]
        private void Put_ShouldUpdateUserToDatabase_DocosoftUser()
        {
            DocosoftUser? updatedUser = GetInitiaUsers().FirstOrDefault(u => u.Id == 2);
            updatedUser.FirstName = "John";

            var docosoftUserManagerMock = new Mock<IDocosoftUserManager>();
            docosoftUserManagerMock.Setup(m => m.UpdateUser(It.IsAny<DocosoftUser>())).ReturnsAsync(updatedUser);
            var docosoftUserController = DocosoftUserControllerInit(docosoftUserManagerMock);

            //act
            var result = docosoftUserController.UpdateUser(updatedUser)?.Result.Result;
            var okResult = result as OkObjectResult;
            DocosoftUser? value = okResult?.Value as DocosoftUser;

            //assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.NotNull(value);
            Assert.Equal(value?.FirstName, "John");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        private void Delete_ShouldRemoveUserToDatabase_bool(int id)
        {
            var docosoftUserManagerMock = new Mock<IDocosoftUserManager>();
            docosoftUserManagerMock.Setup(m => m.DeleteUser(It.IsAny<int>())).ReturnsAsync(true);
            var docosoftUserController = DocosoftUserControllerInit(docosoftUserManagerMock);

            //act
            var result = docosoftUserController.DeleteUser(id)?.Result.Result;
            var okResult = result as OkObjectResult;
            bool? value = (bool)okResult?.Value;

            //assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.NotNull(value);
            Assert.True(value);
        }

        private DocosoftUserController DocosoftUserControllerInit(Mock<IDocosoftUserManager> docosoftUserManagerMock)
        {            
            return new DocosoftUserController(docosoftUserManagerMock.Object);
        }

        private IEnumerable<DocosoftUser> GetInitiaUsers()
        {
            return new List<DocosoftUser>()
            {
                new DocosoftUser() {
                    Id = 1,
                    FirstName = "Jane",
                    LastName = "Doe",
                    Title = "CEO",
                    Email = "jane.doe@docosoft.com",
                    Created = DateTime.Now
                },
                new DocosoftUser() {
                    Id = 2,
                    FirstName = "Jon",
                    LastName = "Doe",
                    Title = "Manager",
                    Email = "john.doe@docosoft.com",
                    Created = DateTime.Now
                }
            };
        }
    }
}
