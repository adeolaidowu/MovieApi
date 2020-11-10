using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MovieApi.DTOs;
using MovieApi.Helper;
using MovieApi.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //private fields
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthController> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;

        //constructor
        public AuthController(IConfiguration configuration, SignInManager<User> signInManager, UserManager<User> userManager, ILogger<AuthController> logger)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = configuration;
        }

        //register user
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto model)
        {
            var userToRegister = _userManager.Users.FirstOrDefault(x => x.Email == model.Email);

            if (userToRegister != null)
                return BadRequest("Email already exist");

            var user = new User
            {
                UserName = model.Email,
                LastName = model.LastName,
                FirstName = model.FirstName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return BadRequest(ModelState);
            }

            return Ok("User Successfully created");

        }

        //User login
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserToLoginDTO model)
        {
            if (ModelState.IsValid)
            {
                //get user by email
                var user = _userManager.Users.FirstOrDefault(x => x.Email == model.Email);

                //check if user exist
                if (user == null)
                    return BadRequest("User does not exist");

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    var getToken = JwtTokenConfig.GetToken(user, _config);
                    var Id = user.Id;
                    var tokenResponse = new TokenResponseDTO
                    {
                        Token = getToken
                    };
                    return Ok(tokenResponse);
                }
                return Unauthorized("Invalid credentials");
            }

            return BadRequest(model);
        }

        // User Logout
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logged out successfully");
        }

    }
}
