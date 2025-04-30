using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using TrackingSystemAPI.Data;
using TrackingSystemAPI.Models;
using TrackingSystemAPI.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;



namespace TrackingSystemAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        private readonly AppDbContext dbContext;
        private readonly IConfiguration configuration;

        public UserController(AppDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginDto loginDto)
        {
            var user = dbContext.Users.FirstOrDefault(x => x.email == loginDto.email
            && x.password == loginDto.password);

            if (user != null)
            {
                var subject = configuration["Jwt:Subject"] ?? throw new ArgumentNullException("Jwt:Subject");
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("id", user.user_id.ToString()),
                    new Claim("email", user.email.ToString()),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key")));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    configuration["Jwt:Issuer"],
                    configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(60),
                    signingCredentials: signIn
                    );
                string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new { Token = tokenValue, User = user });
            }

            return NoContent();
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = dbContext.Users.ToList();
            return Ok(users);
        }


        [HttpGet]
        [Route("{user_id:int}")]  
        public IActionResult GetUserById(int user_id)
        {
            var user = dbContext.Users.Find(user_id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }



        [HttpPost]
        public IActionResult AddUser(AddUserDto addUserDto)
        {

            var userEntity = new User
            {
                user_name = addUserDto.user_name,
                email = addUserDto.email,
                password = addUserDto.password
            };

            dbContext.Users.Add(userEntity);
            dbContext.SaveChanges();
            return Ok(userEntity);
        }


        [HttpPut]
        [Route("{user_id:int}")]
        public IActionResult UpdateUser(int user_id, [FromBody] UpdateUserDto updateUserDto)
        {
            var user = dbContext.Users.Find(user_id);
            if (user == null)
            {
                return NotFound();
            }
            user.user_name = updateUserDto.user_name;
            user.email = updateUserDto.email;
            user.password = updateUserDto.password;
            dbContext.SaveChanges();
            return Ok(user);
        } 

    }
}
