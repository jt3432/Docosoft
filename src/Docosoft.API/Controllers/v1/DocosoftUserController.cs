using Docosoft.API.Core.Managers;
using Docosoft.API.Core.Models;
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
        async public Task<ActionResult<DocosoftUser>> GetUser(int id)
        {
            DocosoftUser? user = await _docosoftUserManager.GetUser(id);

            if (user is null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpPost]
        async public Task<ActionResult<DocosoftUser>> AddUser([FromBody]DocosoftUser newUser)
        {
            if(newUser.Id != 0)
            {
                return BadRequest("User ID must be zero for new users!");
            }

            DocosoftUser user = await _docosoftUserManager.AddUser(newUser);
            return Ok(user);
        }

        [HttpPut]
        async public Task<ActionResult<DocosoftUser?>> UpdateUser([FromBody]DocosoftUser updatedUser)
        {
            DocosoftUser? user = await _docosoftUserManager.UpdateUser(updatedUser);

            if (user is null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpDelete("{id}")]
        async public Task<ActionResult<bool>> DeleteUser(int id)
        {
            bool success = await _docosoftUserManager.DeleteUser(id);

            if (!success)
            {
                return NotFound("User not found");
            }

            return Ok(success);
        }
    }
}
