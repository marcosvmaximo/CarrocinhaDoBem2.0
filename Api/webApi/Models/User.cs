using System.ComponentModel.DataAnnotations;
using webApi.Models.Base;
using Microsoft.AspNetCore.Identity;

namespace webApi.Models;

public class User : ModelBase
{
    // userID INT PRIMARY KEY AUTO_INCREMENT,
    // userName VARCHAR(100) NOT NULL,
    // email VARCHAR(100) NOT NULL,
    // password VARCHAR(50) NOT NULL,
    // phone VARCHAR(14) NOT NULL,
    // address VARCHAR(100),
    // userType VARCHAR(20) NOT NULL,
    // avatar BLOB,
    // birthDate DATE NOT NULL,
    // UNIQUE(email)

    [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome de usuário não pode ter mais de 100 caracteres.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O e-mail não está em um formato válido.")]
    [StringLength(100, ErrorMessage = "O e-mail não pode ter mais de 100 caracteres.")]
    public string Email { get; set; }
    public string PasswordHash { get; set; }

    // [Required(ErrorMessage = "O telefone é obrigatório.")]
    [Phone(ErrorMessage = "O telefone não está em um formato válido.")]
    [StringLength(14, ErrorMessage = "O telefone não pode ter mais de 14 caracteres.")]
    public string? Phone { get; set; }

    [StringLength(100, ErrorMessage = "O endereço não pode ter mais de 100 caracteres.")]
    public string? Address { get; set; }

    // [Required(ErrorMessage = "O tipo de usuário é obrigatório.")]
    [StringLength(20, ErrorMessage = "O tipo de usuário não pode ter mais de 20 caracteres.")]
    public string? UserType { get; set; }

    public byte[]? Avatar { get; set; }

    // [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? BirthDate { get; set; }
}

public class UpdateUser
{

  [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
  [StringLength(100, ErrorMessage = "O nome de usuário não pode ter mais de 100 caracteres.")]
  public string UserName { get; set; }

  [Required(ErrorMessage = "O e-mail é obrigatório.")]
  [EmailAddress(ErrorMessage = "O e-mail não está em um formato válido.")]
  [StringLength(100, ErrorMessage = "O e-mail não pode ter mais de 100 caracteres.")]
  public string Email { get; set; }

  // [Required(ErrorMessage = "O telefone é obrigatório.")]
  [Phone(ErrorMessage = "O telefone não está em um formato válido.")]
  [StringLength(14, ErrorMessage = "O telefone não pode ter mais de 14 caracteres.")]
  public string? Phone { get; set; }

  [StringLength(100, ErrorMessage = "O endereço não pode ter mais de 100 caracteres.")]
  public string? Address { get; set; }

  // [Required(ErrorMessage = "O tipo de usuário é obrigatório.")]
  [StringLength(20, ErrorMessage = "O tipo de usuário não pode ter mais de 20 caracteres.")]
  public string? UserType { get; set; }

  public string? Avatar { get; set; }

  // [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
  [DataType(DataType.Date)]
  [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
  public DateTime? BirthDate { get; set; }
}
public class CreateUserRequest
{
    [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome de usuário não pode ter mais de 100 caracteres.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O e-mail não está em um formato válido.")]
    [StringLength(100, ErrorMessage = "O e-mail não pode ter mais de 100 caracteres.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [StringLength(50, ErrorMessage = "A senha não pode ter mais de 50 caracteres.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
    [StringLength(50, ErrorMessage = "A confirmação de senha não pode ter mais de 50 caracteres.")]
    public string ConfirmPassword { get; set; }
}

public class LoginUserRequest
{
  [Required(ErrorMessage = "O e-mail é obrigatório.")]
  [EmailAddress(ErrorMessage = "O e-mail não está em um formato válido.")]
  [StringLength(100, ErrorMessage = "O e-mail não pode ter mais de 100 caracteres.")]
  public string Email { get; set; }

  [Required(ErrorMessage = "A senha é obrigatória.")]
  [StringLength(50, ErrorMessage = "A senha não pode ter mais de 50 caracteres.")]
  public string Password { get; set; }
}
