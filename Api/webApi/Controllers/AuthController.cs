using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using CarrocinhaDoBem.Api.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApi.Models;
using webApi.Services;


namespace webApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
  private readonly DataContext _context;
  private readonly IPasswordService _service;

  public AuthController(DataContext context, IPasswordService service)
  {
    _context = context;
    _service = service;
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register(CreateUserRequest request)
  {
    if (!ModelState.IsValid) return BadRequest("Usuário ou senha inválida.");
    if(request.Password != request.ConfirmPassword) return BadRequest("Senhas não coincidem.");

    if (!IsCpfValid(request.Cpf))
      return BadRequest("CPF inválido.");
    
    var user = new User
    {
      Email = request.Email,
      Cpf = request.Cpf,
      UserName = request.UserName,
      UserType = "costumer"
    };

    if (request.UserName == "admin")
    {
      user = new User
      {
        Email = request.Email,
        Cpf = request.Cpf,
        UserName = request.UserName,
        UserType = "admin"
      };
    }

    user.PasswordHash = _service.HashPassword(user, request.Password);

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    user.PasswordHash = null;
    return Created("", user);
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login(LoginUserRequest request)
  {
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

    if (user == null) return BadRequest(new { Message = "Email ou senha inválidos."});

    var isPasswordValid = _service.VerifyPassword(user, request.Password);

    user.PasswordHash = null;
    return isPasswordValid ? Ok(new {sucess = true, message = "Login realizado com sucesso.", data = user}) : BadRequest("Email ou senha inválidos.");
  }
  private bool IsCpfValid(string cpf)
  {
    if (string.IsNullOrWhiteSpace(cpf))
      return false;

    cpf = cpf.Replace(".", "").Replace("-", "").Trim();

    if (!Regex.IsMatch(cpf, @"^\d{11}$"))
      return false;

    if (new string(cpf[0], 11) == cpf)
      return false;

    var tempCpf = cpf.Substring(0, 9);
    var firstDigit = CalculateDigit(tempCpf, new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 });
    var secondDigit = CalculateDigit(tempCpf + firstDigit, new int[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 });

    return cpf.EndsWith(firstDigit.ToString() + secondDigit.ToString());
  }

  private int CalculateDigit(string cpf, int[] weight)
  {
    int sum = 0;
    for (int i = 0; i < weight.Length; i++)
      sum += int.Parse(cpf[i].ToString()) * weight[i];

    int remainder = sum % 11;
    return remainder < 2 ? 0 : 11 - remainder;
  }
  
}
