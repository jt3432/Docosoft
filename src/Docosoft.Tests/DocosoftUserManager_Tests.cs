using System;
using System.Collections.Generic;
using Docosoft.API.Core.Managers;
using Docosoft.API.Core.Models;
using Docosoft.API.Core.ResourceAccess;
using Moq;

namespace Docosoft.Tests
{
    public class DocosoftUserManager_Tests
    {
        [Theory]
        [InlineData(1)]
        public void GetUser_ShouldReturnUserWhenGivenAValueId_DocosoftUser(int id)
        {
            //arrange
            var sqliteResourceAccess = new Mock<ISQLiteResourceAccess>();
            sqliteResourceAccess.Setup(ra => ra.FindUser(It.IsAny<int>())).ReturnsAsync(GetInitiaUsers().FirstOrDefault(u => u.Id == id));
            IDocosoftUserManager _docosoftUserManager = new DocosoftUserManager(sqliteResourceAccess.Object);

            //act
            var user = _docosoftUserManager.GetUser(id)?.Result;

            //assert
            Assert.NotNull(user);
            Assert.Equal(id, user.Id);
        }

        [Theory]
        [InlineData(100)]
        public void GetUser_ShouldReturnNullWhenGivenAnInvalidId_null(int id)
        {
            //arrange
            var sqliteResourceAccess = new Mock<ISQLiteResourceAccess>();
            sqliteResourceAccess.Setup(ra => ra.FindUser(It.IsAny<int>())).ReturnsAsync(GetInitiaUsers().FirstOrDefault(u => u.Id == id));
            IDocosoftUserManager _docosoftUserManager = new DocosoftUserManager(sqliteResourceAccess.Object);

            //act
            var user = _docosoftUserManager.GetUser(id)?.Result;

            //assert
            Assert.Null(user);
        }


        [Fact]
        public void GetAllUsers_ShouldReturnAListOfAllUsersInDatabase_IEnumerableOfDocosoftUser()
        {
            //arrange
            var sqliteResourceAccess = new Mock<ISQLiteResourceAccess>();
            sqliteResourceAccess.Setup(ra => ra.FindUsers()).ReturnsAsync(GetInitiaUsers());
            IDocosoftUserManager _docosoftUserManager = new DocosoftUserManager(sqliteResourceAccess.Object);

            //act
            var users = _docosoftUserManager.GetAllUsers()?.Result;

            //assert
            Assert.NotNull(users);
            Assert.Equal(2, users.Count());
        }

        [Fact]
        public void AddUser_ShouldAddANewUserToDatabase_DocosoftUser()
        {
            // arrange
            DocosoftUser newUser = new DocosoftUser()
            {
                Id = 0,
                FirstName = "Peter",
                LastName = "Piper",
                Title = "Developer",
                Email = "peter.piper@docosoft.com",
                Created = DateTime.Now
            };

            var sqliteResourceAccess = new Mock<ISQLiteResourceAccess>();
            sqliteResourceAccess.Setup(ra => ra.AddUser(It.IsAny<DocosoftUser>())).ReturnsAsync((DocosoftUser newUser) => { newUser.Id = 3; return newUser; });
            IDocosoftUserManager _docosoftUserManager = new DocosoftUserManager(sqliteResourceAccess.Object);

            //act
            DocosoftUser? user = _docosoftUserManager.AddUser(newUser)?.Result;

            //assert
            Assert.NotNull(user);
            Assert.Equal("Peter", user.FirstName);
            Assert.Equal(3, user.Id);
        }

        [Fact]
        public void AddUser_ShouldNullIfEmailIsNotUnique_null()
        {
            // arrange
            DocosoftUser newUser = new DocosoftUser()
            {
                Id = 0,
                FirstName = "Peter",
                LastName = "Piper",
                Title = "Developer",
                Email = "jane.doe@docosoft.com",
                Created = DateTime.Now
            };

            var sqliteResourceAccess = new Mock<ISQLiteResourceAccess>();
            sqliteResourceAccess.Setup(ra => ra.UserExists(It.IsAny<string>())).Returns(GetInitiaUsers().Any(u => u.Email.Equals(newUser.Email)));
            sqliteResourceAccess.Setup(ra => ra.AddUser(It.IsAny<DocosoftUser>())).ReturnsAsync((DocosoftUser newUser) => { newUser.Id = 3; return newUser; });
            IDocosoftUserManager _docosoftUserManager = new DocosoftUserManager(sqliteResourceAccess.Object);

            //act
            DocosoftUser? user = _docosoftUserManager.AddUser(newUser)?.Result;

            //assert
            Assert.Null(user);
        }

        [Fact]
        public void UpdateUser_ShouldUpdateExistingUserToDatabase_DocosoftUser()
        {
            // arrange
            DocosoftUser userToUpdate = new DocosoftUser()
            {
                Id = 2,
                FirstName = "John",
                LastName = "Doe",
                Title = "Manager",
                Email = "john.doe@docosoft.com",
                Created = DateTime.Now
            };

            var sqliteResourceAccess = new Mock<ISQLiteResourceAccess>();
            sqliteResourceAccess.Setup(ra => ra.FindUser(It.IsAny<int>())).ReturnsAsync(GetInitiaUsers().FirstOrDefault(u => u.Id.Equals(userToUpdate.Id)));
            sqliteResourceAccess.Setup(ra => ra.UserExists(It.IsAny<string>())).Returns(GetInitiaUsers().Any(u => u.Email.Equals(userToUpdate.Email)));
            sqliteResourceAccess.Setup(ra => ra.UpdateUser(It.IsAny<DocosoftUser>())).ReturnsAsync(userToUpdate);
            IDocosoftUserManager _docosoftUserManager = new DocosoftUserManager(sqliteResourceAccess.Object);

            //act
            DocosoftUser? user = _docosoftUserManager.UpdateUser(userToUpdate)?.Result;

            //assert
            Assert.NotNull(user);
            Assert.Equal("John", user.FirstName);
            Assert.Equal(2, user.Id);
        }

        [Fact]
        public void UpdateUser_ShouldReturnNullIfEmailIsDuplicate_Null()
        {
            // arrange
            DocosoftUser userToUpdate = new DocosoftUser()
            {
                Id = 2,
                FirstName = "John",
                LastName = "Doe",
                Title = "Manager",
                Email = "jane.doe@docosoft.com",
                Created = DateTime.Now
            };

            var sqliteResourceAccess = new Mock<ISQLiteResourceAccess>();
            sqliteResourceAccess.Setup(ra => ra.FindUser(It.IsAny<int>())).ReturnsAsync(GetInitiaUsers().FirstOrDefault(u => u.Id.Equals(userToUpdate.Id)));
            sqliteResourceAccess.Setup(ra => ra.UserExists(It.IsAny<string>())).Returns(GetInitiaUsers().Any(u => u.Email.Equals(userToUpdate.Email)));
            sqliteResourceAccess.Setup(ra => ra.UpdateUser(It.IsAny<DocosoftUser>())).ReturnsAsync(userToUpdate);
            IDocosoftUserManager _docosoftUserManager = new DocosoftUserManager(sqliteResourceAccess.Object);

            //act
            DocosoftUser? user = _docosoftUserManager.UpdateUser(userToUpdate)?.Result;

            //assert
            Assert.Null(user);
        }

        [Fact]
        public void UpdateUser_ShouldReturnNullIfIdIsInvalid_Null()
        {
            // arrange
            DocosoftUser userToUpdate = new DocosoftUser()
            {
                Id = 100,
                FirstName = "John",
                LastName = "Doe",
                Title = "Manager",
                Email = "john.doe@docosoft.com",
                Created = DateTime.Now
            };

            var sqliteResourceAccess = new Mock<ISQLiteResourceAccess>();
            sqliteResourceAccess.Setup(ra => ra.FindUser(It.IsAny<int>())).ReturnsAsync(GetInitiaUsers().FirstOrDefault(u => u.Id.Equals(userToUpdate.Id)));
            sqliteResourceAccess.Setup(ra => ra.UserExists(It.IsAny<string>())).Returns(GetInitiaUsers().Any(u => u.Email.Equals(userToUpdate.Email)));
            sqliteResourceAccess.Setup(ra => ra.UpdateUser(It.IsAny<DocosoftUser>())).ReturnsAsync(userToUpdate);
            IDocosoftUserManager _docosoftUserManager = new DocosoftUserManager(sqliteResourceAccess.Object);

            //act
            DocosoftUser? user = _docosoftUserManager.UpdateUser(userToUpdate)?.Result;

            //assert
            Assert.Null(user);
        }

        [Theory]
        [InlineData(2)]
        public void DeleteUser_ShouldRemoveUserFromDatabase_bool(int id)
        {
            //arrange
            List<DocosoftUser> users = new List<DocosoftUser>()
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
            
            var sqliteResourceAccess = new Mock<ISQLiteResourceAccess>();
            sqliteResourceAccess.Setup(ra => ra.FindUser(It.IsAny<int>())).ReturnsAsync(users.FirstOrDefault(u => u.Id.Equals(id)));
            sqliteResourceAccess.Setup(ra => ra.RemoveUser(It.IsAny<DocosoftUser>())).ReturnsAsync((DocosoftUser newUser) => { users.Remove(newUser); return !users.Any(u => u.Id.Equals(id)); });
            IDocosoftUserManager _docosoftUserManager = new DocosoftUserManager(sqliteResourceAccess.Object);

            //act
            bool? success = _docosoftUserManager.DeleteUser(id)?.Result;
            DocosoftUser? user = _docosoftUserManager.GetUser(id)?.Result;


            //assert
            Assert.True(success);
            Assert.DoesNotContain(users, u => u.Equals(id));
        }

        [Theory]
        [InlineData(100)]
        public void DeleteUser_ShouldReturnFalseIfUserDoesNotExist_bool(int id)
        {
            //arrange
            List<DocosoftUser> users = new List<DocosoftUser>()
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

            var sqliteResourceAccess = new Mock<ISQLiteResourceAccess>();
            sqliteResourceAccess.Setup(ra => ra.FindUser(It.IsAny<int>())).ReturnsAsync(users.FirstOrDefault(u => u.Id.Equals(id)));
            sqliteResourceAccess.Setup(ra => ra.RemoveUser(It.IsAny<DocosoftUser>())).ReturnsAsync((DocosoftUser newUser) => { users.Remove(newUser); return !users.Any(u => u.Id.Equals(id)); });
            IDocosoftUserManager _docosoftUserManager = new DocosoftUserManager(sqliteResourceAccess.Object);

            //act
            bool? success = _docosoftUserManager.DeleteUser(id)?.Result;
            DocosoftUser? user = _docosoftUserManager.GetUser(id)?.Result;


            //assert
            Assert.False(success);
            Assert.DoesNotContain(users, u => u.Equals(id));
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
