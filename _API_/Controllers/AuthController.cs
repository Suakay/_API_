using _21_MVC_API.Context;
using _21_MVC_API.DTO;
using _21_MVC_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _21_MVC_API.Controllers
{
   

    [Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
 
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistractionDto userDTO)
    {
        var result = _context.Users.SingleOrDefault(x => x.Email == userDTO.Email);
 
        if (result == null)
        {
            User user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                UserName = userDTO.UserName,
                NormalizedUserName = userDTO.UserName.ToUpper(),
                NormalizedEmail = userDTO.Email.ToUpper(),
                EmailConfirmed = true,
                PasswordHash = userDTO.Password,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                PhoneNumber = userDTO.PhoneNumber,
                PhoneNumberConfirmed = true
            };
            _context.Users.Add(user);
            _context.SaveChanges();
 
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }
    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
    {
        // Kullanıcı var mı kontrolü yapıyorum.
 
      var login =  _context.Users.SingleOrDefault(x=>x.Email == userLogin.Email && x.PasswordHash == userLogin.Password);
 
        // Kullanıcı varsa JWT Token işlemi yapılıyor.
 
 
        if (login != null)
        {
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, userLogin.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
 
            var token = GetToken(authClaims);
 
            return Ok(
                new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                }
                );
        }
 
        else
        {
            return Unauthorized();
        }
 
 
    }
    // JWT Token oluşturmak için bir metot hazırlıyoruz. Bu metot configuration ayarlarını appsettings.json dosyasından alır.
    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var key =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:secretKey"]));
 
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
 
        var token = new JwtSecurityToken(
            _configuration["JwtSettings:validIssuer"],
            _configuration["JwtSettings:validAudience"],
            authClaims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: signIn
            );
 
        return token;
 
    }
}
}
