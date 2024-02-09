using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WatchStore.Domain;
using WatchStore.Service;

namespace WatchStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {

        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;

        public AuthController(IJwtService jwtService, IUserService userService)
        {
            _jwtService = jwtService;
            _userService = userService;
        }
        
        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            var user = new User
            {
                Id = new Guid(),
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                UserCart = new ShoppingCart()
            };
            return Created("success", _userService.addNewUser(user));
        }


        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _userService.GetUserByEmail(dto.Email);

            if (user == null) return BadRequest(new { message = "Invalid Credentials" });

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }
            var jwt = _jwtService.Generate(user.Id);
            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true
            });
            return Ok(new
            {
                message = "success"

            });
        }


        [HttpGet("user")]
        public IActionResult User()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];

                var token = _jwtService.Verify(jwt);

                Guid userId = Guid.Parse(token.Issuer);

                var user = _userService.GetUserById(userId);

                return Ok(user);
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {

            Response.Cookies.Delete("jwt", new CookieOptions
            {
                SameSite = SameSiteMode.None,
                Secure = true
            });

            return Ok(new
            {
                message = "success"
            });
        }
    }
 }

