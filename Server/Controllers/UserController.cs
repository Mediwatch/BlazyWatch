using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
// using Mediwatch.Server.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;


namespace Mediwatch.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]


    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        //GET: /ApplicantSession
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> GetApplicantSession(){
                return await _userManager.Users.ToListAsync();
        }
    }
}