using BB.APIs.BeardBoss.Monolithic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BB.APIs.BeardBoss.Monolithic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")] // Permite apenas usuários com a role Admin
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // Endpoint para criar uma nova role
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest("Role name cannot be empty.");
            }

            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (roleExist)
            {
                return Conflict("Role already exists.");
            }

            var role = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return Ok($"Role '{roleName}' created successfully.");
            }

            return BadRequest("Error while creating role.");
        }

        // Endpoint para listar todas as roles
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> ListRoles()
        {
            var roles = _roleManager.Roles;
            return Ok(roles);
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRoleToUserByCpf([FromBody] AssignRoleRequest request)
        {
            // Encontra o usuário pelo CPF
            var user =  _userManager.Users.FirstOrDefault(u => u.CPF == request.CPF);
            if (user == null)
            {
                return NotFound($"User with CPF {request.CPF} not found.");
            }

            // Verifica se a role existe
            if (!await _roleManager.RoleExistsAsync(request.Role))
            {
                return BadRequest($"Role {request.Role} does not exist.");
            }

            // Adiciona a role ao usuário
            var result = await _userManager.AddToRoleAsync(user, request.Role);
            if (!result.Succeeded)
            {
                return BadRequest($"Failed to assign role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return Ok($"Role {request.Role} assigned to user with CPF {request.CPF}.");
        }
    }
}
