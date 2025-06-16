using webApi.Services;
using Xunit;

public class PasswordServiceTests
{
    private readonly IPasswordService _passwordService;

    public PasswordServiceTests()
    {
        _passwordService = new PasswordService();
    }

    [Fact]
    [Trait("Service", "PasswordService")]
    public void CreatePasswordHash_ShouldGenerateHashAndSalt()
    {
        var password = "a_strong_password";

        _passwordService.CreatePasswordHash(password, out byte[] generatedHash, out byte[] generatedSalt);

        Assert.NotNull(generatedHash);
        Assert.NotNull(generatedSalt);
        Assert.NotEmpty(generatedHash);
        Assert.NotEmpty(generatedSalt);
    }

    [Fact]
    [Trait("Service", "PasswordService")]
    public void VerifyPasswordHash_ShouldReturnTrue_ForCorrectPassword()
    {
        var password = "a_strong_password";
        _passwordService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        var result = _passwordService.VerifyPasswordHash(password, passwordHash, passwordSalt);

        Assert.True(result);
    }

    [Fact]
    [Trait("Service", "PasswordService")]
    public void VerifyPasswordHash_ShouldReturnFalse_ForIncorrectPassword()
    {
        var correctPassword = "a_strong_password";
        var incorrectPassword = "not_the_password";
        _passwordService.CreatePasswordHash(correctPassword, out byte[] passwordHash, out byte[] passwordSalt);

        var result = _passwordService.VerifyPasswordHash(incorrectPassword, passwordHash, passwordSalt);

        Assert.False(result);
    }

    [Fact]
    [Trait("Service", "PasswordService")]
    public void VerifyPasswordHash_ShouldReturnFalse_WhenSaltIsIncorrect()
    {
        var password = "a_strong_password";
        _passwordService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
        _passwordService.CreatePasswordHash("another_password", out byte[] _, out byte[] wrongSalt);

        var result = _passwordService.VerifyPasswordHash(password, passwordHash, wrongSalt);

        Assert.False(result);
    }
}