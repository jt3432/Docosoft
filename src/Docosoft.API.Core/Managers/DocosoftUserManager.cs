using Docosoft.API.Core.Models;
using Docosoft.API.Core.Models.DTO;
using Docosoft.API.Core.ResourceAccess;

namespace Docosoft.API.Core.Managers
{
    public interface IDocosoftUserManager
    {
        Task<DocosoftUserDTO> GetUser(int id);
        Task<IEnumerable<DocosoftUser>> GetAllUsers();
        Task<DocosoftUserDTO> AddUser(DocosoftUser user);
        Task<DocosoftUserDTO> UpdateUser(DocosoftUser user);
        Task<DocosoftUserDTO> DeleteUser(int id);
    }

    public class DocosoftUserManager(ISQLiteResourceAccess sqliteResourceAccess) : IDocosoftUserManager
    {
        private readonly ISQLiteResourceAccess _sqliteResourceAccess = sqliteResourceAccess;

        async public Task<DocosoftUserDTO> GetUser(int id)
        {
            DocosoftUser? user = await _sqliteResourceAccess.FindUser(id);

            return new DocosoftUserDTO()
            {
                User = user,
                ErrorMessage = user is null ? "User not found." : String.Empty,
                Success = user is not null,
            };
        }

        async public Task<IEnumerable<DocosoftUser>> GetAllUsers()
        {
            return await _sqliteResourceAccess.FindUsers();
        }

        async public Task<DocosoftUserDTO> AddUser(DocosoftUser newUser)
        {
            DocosoftUser? user = default;
            DocosoftUserDTO userDTO = new DocosoftUserDTO() {
                Success = false
            };

            bool exists = _sqliteResourceAccess.UserExists(newUser.Email);

            if (exists)
            {
                userDTO.ErrorMessage = "Duplicate user email. Emails must be unique.";
                return userDTO;
            }

            user = await _sqliteResourceAccess.AddUser(newUser);
            userDTO.User = user;
            userDTO.Success = true;

            return userDTO;
        }

        async public Task<DocosoftUserDTO> UpdateUser(DocosoftUser updatedUser)
        {
            var userDTO = new DocosoftUserDTO()
            {
                Success = false,
            };

            DocosoftUser? user = await _sqliteResourceAccess.FindUser(updatedUser.Id);

            if (user is null)
            {
                userDTO.ErrorMessage = "User not found!";
                return userDTO;
            }

            if (!user!.Email.Equals(updatedUser.Email) && _sqliteResourceAccess.UserExists(updatedUser.Email))
            {
                userDTO.ErrorMessage = "Duplicate email, email must be unique!";
                return userDTO;
            }

            // TODO: Add AutoMapper to project
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Title = updatedUser.Title;
            user.Email = updatedUser.Email;

            userDTO.User = await _sqliteResourceAccess.UpdateUser(user);

            userDTO.Success = true;

            return userDTO;
        }

        async public Task<DocosoftUserDTO> DeleteUser(int id)
        {
            var userDTO = new DocosoftUserDTO()
            {
                Success = false,
            };

            DocosoftUser? user = await _sqliteResourceAccess.FindUser(id);

            if (user is null)
            {
                userDTO.ErrorMessage = "User not found!";
                return userDTO;
            }

            bool success = await _sqliteResourceAccess.RemoveUser(user);

            userDTO.Success = success;
            userDTO.ErrorMessage = success ? String.Empty : "User not removed.";

            return userDTO;
        }
    }
}
