using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using webApi.Context;
using webApi.Models;
using webApi.Services;

namespace webApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IPasswordService _passwordService;
        private readonly IConfiguration _configuration;

        public AuthController(DataContext context, IPasswordService passwordService, IConfiguration configuration)
        {
            _context = context;
            _passwordService = passwordService;
            _configuration = configuration;
        }

        // Usa o DTO 'CreateUserRequest' que você já tem em User.cs
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(CreateUserRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest("Este e-mail já está em uso.");
            }
            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest("As senhas não conferem.");
            }

            _passwordService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // CORREÇÃO: Mapeia para as propriedades corretas da sua entidade User
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                Cpf = request.Cpf,
                PasswordHash = passwordHash, // Usa o byte[] do PasswordService
                PasswordSalt = passwordSalt, // Usa o byte[] do PasswordService
                UserType = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { user.Id, user.UserName, user.Email });
        }

        // Usa o DTO 'LoginUserRequest' que você já tem em User.cs
        [HttpPost("login")]
                public async Task<ActionResult<string>> Login(LoginUserRequest request)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                    if (user == null)
                    {
                        return BadRequest("Usuário ou senha inválidos.");
                    }

                    if (!_passwordService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                    {
                        return BadRequest("Usuário ou senha inválidos.");
                    }

                    string token = CreateToken(user);

                    // CORREÇÃO: Retorna tanto o token quanto os dados básicos do usuário
                    var userResponse = new {
                        user.Id,
                        user.UserName,
                        user.Email,
                        user.UserType
                    };

                    return Ok(new { token, user = userResponse });
                }

        private string CreateToken(User user)
        {
            // CORREÇÃO: Usa as propriedades corretas da sua entidade User
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName), // Usa a propriedade 'UserName'
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.UserType ?? "User") // Usa a propriedade 'UserType'
            };

            var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
            if (string.IsNullOrEmpty(appSettingsToken))
                throw new Exception("Chave do Token não encontrada em appsettings.json");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettingsToken));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
