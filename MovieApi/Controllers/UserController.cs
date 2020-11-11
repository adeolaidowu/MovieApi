using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Services;

namespace MovieApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet("allusers")]
        public async Task<IActionResult> getAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            if(users == null)
            {
                return BadRequest("There is no user registered");
            }
            return Ok(users);
        }
    }
}
