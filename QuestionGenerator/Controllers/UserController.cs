using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Models.UserModel;

namespace QuestionGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            var user = await _userService.GetUser(id);
            if (!user.Status)
                return NotFound(new { message = user.Message });

            return Ok(new { user = user.Value });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var user = await _userService.RemoveUser(id);
            if(!user.Status)
                return NotFound(new { message = user.Message });

            return Ok(new { message = user.Message });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UpdateUserRequest request)
        {
            var user = await _userService.UpdateUser(id, request);
            if (user.Status)
                return Ok(new { message = user.Message });

            if(user.Message!.Contains("Email"))
                return Conflict(new {message =  user.Message});
            return NotFound(new { message = user.Message });
        }
    }
}
