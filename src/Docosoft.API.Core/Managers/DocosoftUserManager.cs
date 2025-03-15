using Docosoft.API.Core.Models;
using Docosoft.API.Core.ResourceAccess;

namespace Docosoft.API.Core.Managers
{
    public interface IDocosoftUserManager
    {        
        Task<DocosoftUser?> GetUser(int id);
        Task<IEnumerable<DocosoftUser>> GetAllUsers();
        Task<DocosoftUser> AddUser(DocosoftUser user);
        Task<DocosoftUser?> UpdateUser(DocosoftUser user);
        Task<bool> DeleteUser(int id);
    }

    public class DocosoftUserManager(ISQLiteResourceAccess sqliteResourceAccess) : IDocosoftUserManager
    {
        private readonly ISQLiteResourceAccess _sqliteResourceAccess = sqliteResourceAccess;

        async public Task<DocosoftUser> AddUser(DocosoftUser newUser)
        {
            DocosoftUser user = await _sqliteResourceAccess.AddUser(newUser);
            return user;
        }        

        async public Task<IEnumerable<DocosoftUser>> GetAllUsers()
        {
            return await _sqliteResourceAccess.FindUsers();
        }

        async public Task<DocosoftUser?> GetUser(int id)
        {
            return await _sqliteResourceAccess.FindUser(id);
        }

        async public Task<DocosoftUser?> UpdateUser(DocosoftUser updatedUser)
        {            
            DocosoftUser? user = default;
            bool exist = _sqliteResourceAccess.UserExists(updatedUser.Id);

            if (exist)
            {
                user = await _sqliteResourceAccess.UpdateUser(updatedUser);
            }

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
