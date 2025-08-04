using LogiTrack.Dto;
using LogiTrack.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LogiTrack.Controllers
{
    [ApiController]
    [Route("api/role")]
    public class RoleController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public RoleController(UserManager<ApplicationUser> User, RoleManager<IdentityRole> Role)
        {
            _userManager = User;
            _roleManager = Role;
        }


        // Create a new Role.
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }


        // Add Roles to Users.
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("add-role-to-user")]
        public async Task<IActionResult> AddRolesToUsers(AddRoleToUserDTO roleToUser)
        {
            var user = await _userManager.FindByEmailAsync(roleToUser.Email);
            if (user == null || roleToUser.Role == null)
            {
                return NotFound();
            }

            await _userManager.AddToRoleAsync(user, roleToUser.Role);
            return Ok();
        }



    }
}