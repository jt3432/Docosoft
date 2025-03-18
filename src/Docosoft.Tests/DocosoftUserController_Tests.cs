using Docosoft.API.Controllers.v1;
using Docosoft.API.Core.Managers;
using Docosoft.API.Core.Models;
using Docosoft.API.Core.Models.DTO;
using Docosoft.API.Models.Request;
using Docosoft.API.Models.Response;
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
        private void Get_ShouldReturnAUserById_ApiResponse(int id)
        {
            //arrange            
            var docosoftUserManagerMock = new Mock<IDocosoftUserManager>();
            docosoftUserManagerMock.Setup(m => m.GetUser(It.IsAny<int>())).ReturnsAsync((int id) => {
                DocosoftUser? user = GetInitiaUsers().FirstOrDefault(u => u.Id == id);
                return new DocosoftUserDTO()
                {
                    User = user,
                    Success = true
                };
             });
            var docosoftUserController = DocosoftUserControllerInit(docosoftUserManagerMock);

            //act
            var result = docosoftUserController.GetUser(id)?.Result;
            var okResult = result as OkObjectResult;
            SuccessResponse<DocosoftUser>? value = okResult?.Value as SuccessResponse<DocosoftUser>;

            //assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.NotNull(value);
            Assert.Equal(id, value.Data?.Id);
        }

        [Fact]
        private void Post_ShouldAddUserToDatabase_ApiResponse()
        {
            //arrange            
            NewDocosoftUserRequest request = new NewDocosoftUserRequest()
            {
                FirstName = "Peter",
                LastName = "Piper",
                Title = "Developer",
                Email = "peter.piper@docosoft.com"
            };
            DocosoftUser newUser = new DocosoftUser()
            {
                Id = 3,
                FirstName = "Peter",
                LastName = "Piper",
                Title = "Developer",
                Email = "peter.piper@docosoft.com"
            };

            var docosoftUserManagerMock = new Mock<IDocosoftUserManager>();
            docosoftUserManagerMock.Setup(m => m.AddUser(It.IsAny<DocosoftUser>())).ReturnsAsync((DocosoftUser user) => { 
                return new DocosoftUserDTO()
                {
                    User = newUser,
                    Success = true
                };
            });
            var docosoftUserController = DocosoftUserControllerInit(docosoftUserManagerMock);

            //act
            var result = docosoftUserController.AddUser(request)?.Result;
            var okResult = result as OkObjectResult;
            SuccessResponse<DocosoftUser>? value = okResult?.Value as SuccessResponse<DocosoftUser>;

            //assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.NotNull(value);
            Assert.True(value.Data?.Id > 0);
            Assert.Equal(3, value.Data.Id);
        }

        [Fact]
        private void Put_ShouldUpdateUserToDatabase_ApiResponse()
        {
            //arrange
            DocosoftUser? updatedUser = GetInitiaUsers().FirstOrDefault(u => u.Id == 2);
            updatedUser!.FirstName = "John";

            var docosoftUserManagerMock = new Mock<IDocosoftUserManager>();
            docosoftUserManagerMock.Setup(m => m.UpdateUser(It.IsAny<DocosoftUser>())).ReturnsAsync((DocosoftUser user) => {
                return new DocosoftUserDTO()
                {
                    User = updatedUser,
                    Success = true
                };
            });
            var docosoftUserController = DocosoftUserControllerInit(docosoftUserManagerMock);

            //act
            var result = docosoftUserController.UpdateUser(updatedUser)?.Result;
            var okResult = result as OkObjectResult;
            SuccessResponse<DocosoftUser>? value = okResult?.Value as SuccessResponse<DocosoftUser>;

            //assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("John", value.Data?.FirstName);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        private void Delete_ShouldRemoveUserToDatabase_ApiResponse(int id)
        {
            //arrange
            Mock<IDocosoftUserManager> docosoftUserManagerMock = new Mock<IDocosoftUserManager>();
            docosoftUserManagerMock.Setup(m => m.DeleteUser(It.IsAny<int>())).ReturnsAsync((int id) => {
                return new DocosoftUserDTO()
                {
                    User = null,
                    Success = true
                };
            });
            DocosoftUserController docosoftUserController = DocosoftUserControllerInit(docosoftUserManagerMock);

            //act
            var result = docosoftUserController.DeleteUser(id)?.Result;
            var okResult = result as OkObjectResult;
            SuccessResponse<DocosoftUser>? value = (SuccessResponse<DocosoftUser>?)okResult?.Value;

            //assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.NotNull(value);
            Assert.True(value.Success);
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
