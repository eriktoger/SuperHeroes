using Microsoft.AspNetCore.Mvc;
using MongoExample.Services;
using MongoExample.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace SuperHeroAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    public static User user = new User();
    private readonly IConfiguration _configuration;
    private readonly MongoDBService _mongoDBService;

    public AuthController(IConfiguration configuration, MongoDBService mongoDBService)
    {
        _configuration = configuration;
        _mongoDBService = mongoDBService;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<string>> Register(UserDto request)
    {
        var user = await _mongoDBService.GetOneUser(request.Username);

        if (user != null)
        {
            return BadRequest("Username is taken");
        }

        var salt = System.Text.Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Salt").Value
        );
        var hash = CreatePasswordHash(request.Password, salt);

        UserDto newUser = new UserDto { Username = request.Username, Password = hash };
        await _mongoDBService.RegisterUser(newUser);
        string token = CreateToken(newUser.Username);
        return Ok(token);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<string>> Login(UserDto request)
    {
        var salt = System.Text.Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Salt").Value
        );
        var hash = CreatePasswordHash(request.Password, salt);
        var user = await _mongoDBService.LoginUser(request.Username, hash);
        if (user == null)
        {
            return BadRequest("User not found");
        }

        string token = CreateToken(user.Username);
        return Ok(token);
    }

    private string CreateToken(string username)
    {
        List<Claim> claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
        var key = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value)
        );
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    private string CreatePasswordHash(string password, byte[] salt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(salt))
        {
            var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return System.Text.Encoding.UTF8.GetString(passwordHash);
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
        {
            var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computeHash.SequenceEqual(passwordHash);
        }
    }
}
