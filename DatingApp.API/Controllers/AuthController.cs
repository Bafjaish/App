using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        public AuthController(IAuthRepository repo)
        {
            _repo = repo;

        }

      [HttpPost("reigster")]

      public async Task<IActionResult>  Reigster (UserForRegisterDtos UserForRegisterDtos) // Iactions allows to retun http request as on OK
        {

            // cz pass and username the resp is http resp so we make dto data transdfer object.
            // from the other class as we name it UserForRegisterDtos
     
            UserForRegisterDtos.UserName = UserForRegisterDtos.UserName.ToLower();
            if (await _repo.UserExists(UserForRegisterDtos.UserName))
                return BadRequest("usrename is ex");

            var userToCreate =  new User
            {   
                       Username = UserForRegisterDtos.UserName
            };
             var createdUser = await _repo.Register(userToCreate, UserForRegisterDtos.Password);
             return StatusCode(201);
            } 
        }     


    }
