using Microsoft.AspNetCore.Mvc;

namespace UserRegistrationApi.Controllers
{
    public class UserController : ControllerBase
    {

        public UserController()
        {
        }
        [HttpPost("Register")]
        public ActionResult Register(string username, string password )
        {
            return Ok();
        }
        [HttpGet("Login")]
        public ActionResult Login(string username, string password)
        {
            return Unauthorized();
        }
    }
}
