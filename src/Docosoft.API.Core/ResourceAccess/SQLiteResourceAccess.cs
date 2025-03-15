using Docosoft.API.Core.Data;
using Docosoft.API.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Docosoft.API.Core.ResourceAccess
{
    public interface ISQLiteResourceAccess
    {
        bool UserExists(int id);
        Task<DocosoftUser?> FindUser(int id);
        Task<IEnumerable<DocosoftUser>> FindUsers();
        Task<DocosoftUser> AddUser(DocosoftUser user);
        Task<DocosoftUser> UpdateUser(DocosoftUser user);
        Task<bool> RemoveUser(DocosoftUser user);

    }

    public class SQLiteResourceAccess(DataContext dataContext) : ISQLiteResourceAccess
    {
        private readonly DataContext _dataContext = dataContext;

        public bool UserExists(int id)
        {
            bool exists = _dataContext.DocosoftUsers.Any(u => u.Id == id);
            return exists;
        }

        async public Task<DocosoftUser?> FindUser(int id)
        {
            DocosoftUser? user = await _dataContext.DocosoftUsers.FindAsync(id);
            return user;            
        }

        async public Task<IEnumerable<DocosoftUser>> FindUsers()
        {
            IEnumerable<DocosoftUser> users = await _dataContext.DocosoftUsers.ToListAsync();
            return users;
        }

        async public Task<DocosoftUser> AddUser(DocosoftUser user)
        {
            _dataContext.Add<DocosoftUser>(user);
            await _dataContext.SaveChangesAsync();
            return user;
        }

        async public Task<DocosoftUser> UpdateUser(DocosoftUser user)
        {
            _dataContext.Update<DocosoftUser>(user);
            await _dataContext.SaveChangesAsync();
            return user;
        }

        async public Task<bool> RemoveUser(DocosoftUser user)
        {
            _dataContext.Remove<DocosoftUser>(user);
            int value = await _dataContext.SaveChangesAsync();
            return value > 0;
        }
    }
}
