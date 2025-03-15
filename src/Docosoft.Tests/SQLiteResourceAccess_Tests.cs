using Docosoft.API.Core.Data;
using Docosoft.API.Core.Models;
using Docosoft.API.Core.ResourceAccess;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;

namespace Docosoft.Tests
{
    public class SQLiteResourceAccess_Tests
    {
        private readonly ISQLiteResourceAccess _sqliteResourceAccess;

        public SQLiteResourceAccess_Tests()
        {
            //arrange
            DbContextMock<DataContext> dataContextMock = GetDbContext(GetInitiaUsers());
            _sqliteResourceAccess = new SQLiteResourceAccess(dataContextMock.Object);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        private void UserExists_ShouldReturnTrueIfUserExists_bool(int id)
        {
            //arrange

            //act
            var exists = _sqliteResourceAccess.UserExists(id);

            //assert
            Assert.True(exists);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        private void FindUser_ShouldReturnUserById_DocosoftUser(int id)
        {
            //arrange

            //act
            DocosoftUser? user = _sqliteResourceAccess.FindUser(id)?.Result;

            //assert
            Assert.Equal(id, user?.Id);
        }

        [Fact]
        private void AddUser_ShouldAddUserToDatabase_DocosoftUser()
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

            //act
            DocosoftUser? user = _sqliteResourceAccess.AddUser(newUser)?.Result;

            //assert
            Assert.NotNull(user);
            Assert.Equal("Peter", user.FirstName);
        }

        [Fact]
        private void UpdateUser_ShouldSaveChangesToUserToDatabase_DocosoftUser()
        {
            //arrange            
            DocosoftUser? userToUpdate = _sqliteResourceAccess.FindUser(2)?.Result;
            userToUpdate!.FirstName = "John";

            //act
            DocosoftUser? updatedUser = _sqliteResourceAccess.UpdateUser(userToUpdate)?.Result;

            //assert
            Assert.NotNull(updatedUser);
            Assert.Equal("John", updatedUser.FirstName);

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        private void RemoveUser_ShouldRemoveUserFromDatabase_bool(int id)
        {
            //arrange            
            DocosoftUser? userToRemove = _sqliteResourceAccess.FindUser(id)?.Result;

            //act
            _sqliteResourceAccess.RemoveUser(userToRemove!);
            DocosoftUser? user = _sqliteResourceAccess.FindUser(id)?.Result;


            //assert
            Assert.NotNull(userToRemove);
            Assert.Null(user);
        }

        private DbContextMock<DataContext> GetDbContext(DocosoftUser[] initiaUsers)
        {
            DbContextMock<DataContext> dbContextMock = new DbContextMock<DataContext>(new DbContextOptionsBuilder<DataContext>().Options);
            DbSetMock<DocosoftUser> dbSet = dbContextMock.CreateDbSetMock(x => x.DocosoftUsers, initiaUsers);
            return dbContextMock;
        }

        private DocosoftUser[] GetInitiaUsers()
        {
            return new DocosoftUser[]
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
