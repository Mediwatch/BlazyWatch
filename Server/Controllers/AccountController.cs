using Mediwatch.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;


namespace Mediwatch.Server.Controllers
{
    [ApiController]
    [Route("/[controller]/[action]")]
    public class AccountController : ControllerBase
    {

        [HttpPost]
        public IActionResult Login(LoginForm login)
        {

            return Ok();
        }

    }
}