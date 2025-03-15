using Docosoft.API.Core.Models;
using Docosoft.API.Core.ResourceAccess;

namespace Docosoft.API.Core.Managers
{
    public interface IDocosoftUserManager
    {
        Task<DocosoftUser?> GetUser(int id);
        Task<IEnumerable<DocosoftUser>> GetAllUsers();
        Task<DocosoftUser?> AddUser(DocosoftUser user);
        Task<DocosoftUser?> UpdateUser(DocosoftUser user);
        Task<bool> DeleteUser(int id);
    }

    public class DocosoftUserManager(ISQLiteResourceAccess sqliteResourceAccess) : IDocosoftUserManager
    {
        private readonly ISQLiteResourceAccess _sqliteResourceAccess = sqliteResourceAccess;

        async public Task<DocosoftUser?> GetUser(int id)
        {
            return await _sqliteResourceAccess.FindUser(id);
        }

        async public Task<IEnumerable<DocosoftUser>> GetAllUsers()
        {
            return await _sqliteResourceAccess.FindUsers();
        }

        async public Task<DocosoftUser?> AddUser(DocosoftUser newUser)
        {
            DocosoftUser? user = default;

            bool exists = _sqliteResourceAccess.UserExists(newUser.Email);

            if (exists)
            {
                return user;
            }

            user = await _sqliteResourceAccess.AddUser(newUser);
            return user;
        }

        async public Task<DocosoftUser?> UpdateUser(DocosoftUser updatedUser)
        {
            DocosoftUser? user = await _sqliteResourceAccess.FindUser(updatedUser.Id);

            if (user is null)
            {
                return null;
            }

            if (!user!.Email.Equals(updatedUser.Email) && _sqliteResourceAccess.UserExists(updatedUser.Email))
            {
                return null;
            }

            // TODO: Add AutoMapper to project
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Title = updatedUser.Title;
            user.Email = updatedUser.Email;

            user = await _sqliteResourceAccess.UpdateUser(user);

            return user;
        }

        async public Task<bool> DeleteUser(int id)
        {
            DocosoftUser? user = await _sqliteResourceAccess.FindUser(id);

            if (user is null)
            {
                return false;
            }

            return await _sqliteResourceAccess.RemoveUser(user);
        }
    }
}
