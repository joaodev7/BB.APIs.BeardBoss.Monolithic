using BB.APIs.BeardBoss.Monolithic.DTOs.Auth;
using BB.APIs.BeardBoss.Monolithic.Models;
using BB.APIs.BeardBoss.Monolithic.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BB.APIs.BeardBoss.Monolithic.Services.Implementations
{

    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;        
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<JwtTokenResult> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            // Verifica se o CPF já está cadastrado
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.CPF == registerUserDto.CPF);

            if (existingUser is null)
                throw new Exception("Usuário já está cadastrado no sistema. Não é permitido CPF duplicado.");


            var user = new ApplicationUser
            {
                UserName = registerUserDto.Email,
                Email = registerUserDto.Email,
                Name = registerUserDto.Name,
                Role = registerUserDto.Role,
                CPF = registerUserDto.CPF
            };

            var result = await _userManager.CreateAsync(user, registerUserDto.Password);

            if (!result.Succeeded)
            {
                throw new Exception("Erro ao registrar usuário.");
            }

            return await GenerateJwtTokenAsync(user);
        }

        public async Task<JwtTokenResult> LoginUserAsync(LoginUserDto loginUserDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.CPF == loginUserDto.CPF);

            if (user is null)
                throw new Exception("Usuário não está cadastrado no sistema.");

            if (user == null || !(await _userManager.CheckPasswordAsync(user, loginUserDto.Password)))
            {
                throw new Exception("Credenciais inválidas.");
            }

            return await GenerateJwtTokenAsync(user);
        }

        private async Task<JwtTokenResult> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiresInMinutes"])),
                signingCredentials: creds);

            return new JwtTokenResult
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };
        }
    }
}
