using LogiTrack.Data;
using LogiTrack.Dto;
using LogiTrack.Entities;
using LogiTrack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace LogiTrack.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenService _tokenService;


        public AuthController(UserManager<ApplicationUser> User, SignInManager<ApplicationUser> SignIn, TokenService Token)
        {
            _userManager = User;
            _signInManager = SignIn;
            _tokenService = Token;
        }

        [Authorize(Policy = "EditorPolicy")]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserDTO registerUser)
        {
            var user = new ApplicationUser
            {
                UserName = registerUser.Username,
                FullName = registerUser.FullName,
                Email = registerUser.Email
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
            {
                return Ok();
            }

            foreach (var error in result.Errors)
            {
                Console.WriteLine(error.Description + "\n\n");
            }

            return NotFound();

        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginUser)
        {
            var user = await _userManager.FindByNameAsync(loginUser.Username);
            if (user == null)
            {
                return NoContent();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginUser.Password, false);

            if (result.Succeeded)
                {
                    var roleList = await _userManager.GetRolesAsync(user);
                    string role = string.Join("", roleList);

                    var token = _tokenService.GenerateToken(user.UserName, role);

                    return Ok(new { Token = token });
                }

                else
                {
                    return NotFound("Sign-in failed.");
                }

        }
    }
}