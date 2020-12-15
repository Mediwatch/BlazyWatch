using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
// using Mediwatch.Server.Areas.Identity.Data;
using Mediwatch.Shared.Models;
using Microsoft.AspNetCore.Identity;


namespace Mediwatch.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private UserPublic createUserPublic(IdentityUser<Guid> elem) {
                UserPublic node = new UserPublic();
                node.Id = elem.Id;
                node.Name = elem.UserName;
                node.Email = elem.Email;
                node.PhoneNumber = elem.PhoneNumber;
            return node;
        }
        private List<UserPublic> ConvertInPublicInfo(ref List<IdentityUser<Guid>> dataToChange){
            List<UserPublic> PublicUserInfo = new List<UserPublic>();
            
            foreach(IdentityUser<Guid> elem in dataToChange){
                var node = createUserPublic(elem);
                PublicUserInfo.Add(node);
            }
            return PublicUserInfo;
        }
        private readonly UserManager<IdentityUser<Guid>> _userManager;

        public UsersController(UserManager<IdentityUser<Guid>> userManager)
        {
            _userManager = userManager;
        }

        //GET: /Users

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserPublic>>> GetUserList()
        {
            var rawInfo = await _userManager.Users.ToListAsync();
            var publicUsers = ConvertInPublicInfo(ref rawInfo);
            return publicUsers;
        }

        //GET: /Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserPublic>> GetUser(String id)
        {
            Guid x;
            if (!Guid.TryParse(id, out x))
                return NotFound();
            IdentityUser<Guid> rawInfo;
            rawInfo = await _userManager.FindByIdAsync(id);
            var publicUser = createUserPublic(rawInfo);
            if (rawInfo == null)
            {
                return NotFound();
            }
            return publicUser;

        }


    }
}