using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;

        }

        [HttpPost("register")]

        public async Task<IActionResult> Register(UserForRegisterDtos UserForRegisterDtos) // Iactions allows to retun http request as on OK
        {

            // cz pass and username the resp is http resp so we make dto data transdfer object.
            // from the other class as we name it UserForRegisterDtos

            UserForRegisterDtos.UserName = UserForRegisterDtos.UserName.ToLower();
            if (await _repo.UserExists(UserForRegisterDtos.UserName))
                return BadRequest("usrename is ex");

            var userToCreate = new User
            {
                Username = UserForRegisterDtos.UserName
            };
            var createdUser = await _repo.Register(userToCreate, UserForRegisterDtos.Password);
            return StatusCode(201);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto UserLoginDto) // Iactions allows to retun http request as on OK
        {

            throw new Exception("Computer says No!");
            var userFromRepo = await _repo.Login(UserLoginDto.Username.ToLower(), UserLoginDto.Password); // check if the user is exits

            if (userFromRepo == null)
                return Unauthorized();

            // here we bult the token, contains userId, ueranname we can add more// we can 
            // verfiy without look in the database 
            // the token created contains 2 clamins


            var claims = new[] // claims 
            {
                   new Claim (ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                    new Claim (ClaimTypes.Name, userFromRepo.Username)

               };

            // we also need a key to assign our token and we need to encode it
            // we need to store in the app settings 
            // App setting should be in the saved in the app settings.jsn
            // in order to know the token is valid token when it back ffrom the server, need to assign this token  as below
            var key = new SymmetricSecurityKey(Encoding.UTF8.
            GetBytes(_config.GetSection("AppSettings:Token").Value));

            // here we can generate sining credintails 

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // after that we can create the desctrpotro that can have 
            // claims, expry date , creds 

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.Now.AddDays(1),

                SigningCredentials = creds


            };

            // here we need the token hander 
            var tokenHandler = new JwtSecurityTokenHandler();  // final to create the token, handler is used to allow to create token based on above

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new
            {
                token = tokenHandler.WriteToken(token) // here to retun token as object. 
                                                       //  return Ok("Here is it");
            });
        }
    }
}
