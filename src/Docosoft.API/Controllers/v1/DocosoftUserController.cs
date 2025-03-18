using Docosoft.API.Core.Managers;
using Docosoft.API.Core.Models;
using Docosoft.API.Core.Models.DTO;
using Docosoft.API.Factories;
using Docosoft.API.Models.Request;
using Docosoft.API.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Docosoft.API.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DocosoftUserController(IDocosoftUserManager docosoftUserManager) : ControllerBase
    {
        private readonly IDocosoftUserManager _docosoftUserManager = docosoftUserManager;

        [HttpGet]
        async public Task<ActionResult<IEnumerable<DocosoftUser>>> GetAll()
        {
            IEnumerable<DocosoftUser> users = await _docosoftUserManager.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        async public Task<IActionResult> GetUser(int id)
        {
            DocosoftUserDTO userDTO = await _docosoftUserManager.GetUser(id);

            if (userDTO.Success)
            {
                return Ok(ApiResponseFactory.CreateSuccess<DocosoftUser>(userDTO.User));                
            }

            return NotFound(ApiResponseFactory.CreateError(userDTO.ErrorMessage));
        }

        [HttpPost]
        async public Task<IActionResult> AddUser([FromBody]NewDocosoftUserRequest request)
        {
            // Add AutoMapper 
            DocosoftUser newUser = new DocosoftUser()
            {                 
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Title = request.Title
            };

            DocosoftUserDTO? userDTO = await _docosoftUserManager.AddUser(newUser);

            if (userDTO.Success)
            {
                return Ok(ApiResponseFactory.CreateSuccess<DocosoftUser>(userDTO.User));
            }

            return UnprocessableEntity(ApiResponseFactory.CreateError(userDTO.ErrorMessage));            
        }

        [HttpPut]
        async public Task<IActionResult> UpdateUser([FromBody]DocosoftUser updatedUser)
        {
            DocosoftUserDTO userDTO = await _docosoftUserManager.UpdateUser(updatedUser);

            if (userDTO.Success)
            {
                return Ok(ApiResponseFactory.CreateSuccess<DocosoftUser>(userDTO.User));
                
            }

            return UnprocessableEntity(ApiResponseFactory.CreateError(userDTO.ErrorMessage));
        }

        [HttpDelete("{id}")]
        async public Task<IActionResult> DeleteUser(int id)
        {
            DocosoftUserDTO userDto = await _docosoftUserManager.DeleteUser(id);

            if (userDto.Success)
            {
                return Ok(ApiResponseFactory.CreateSuccess<DocosoftUser>(null, "User deleted."));                
            }

            return NotFound(ApiResponseFactory.CreateError(userDto.ErrorMessage));
        }
    }
}
