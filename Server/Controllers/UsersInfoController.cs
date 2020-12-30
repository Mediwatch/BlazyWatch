using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// using Mediwatch.Server.Areas.Identity.Data;
using System.Security.Claims;
using Mediwatch.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Server;

namespace Mediwatch.Server.Controllers {
    [ApiController]
    [Route ("[controller]")]
    public class UsersController : ControllerBase {
        private UserPublic createUserPublic (IdentityUser<Guid> elem) {
            UserPublic node = new UserPublic ();
            node.Id = elem.Id;
            node.Name = elem.UserName;
            node.Email = elem.Email;
            node.PhoneNumber = elem.PhoneNumber;
            return node;
        }
        private List<UserPublic> ConvertInPublicInfo (ref List<IdentityUser<Guid>> dataToChange) {
            List<UserPublic> PublicUserInfo = new List<UserPublic> ();

            foreach (IdentityUser<Guid> elem in dataToChange) {
                var node = createUserPublic (elem);
                PublicUserInfo.Add (node);
            }
            return PublicUserInfo;
        }

        private void SetUserInfoFromUserPublic (UserPublic user) {
            return;
        }
        private readonly UserManager<IdentityUser<Guid>> _userManager;

        private readonly DbContextMediwatch _context;

        public UsersController (UserManager<IdentityUser<Guid>> userManager,
            DbContextMediwatch context) {
            _userManager = userManager;
            _context = context;
        }

        //GET: /Users/listUser
        //list all user
        [HttpGet ("listUser")]
        [Authorize (Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserPublic>>> GetUserList () {
            var rawInfo = await _userManager.Users.ToListAsync ();
            var publicUsers = ConvertInPublicInfo (ref rawInfo);
            return publicUsers;
        }

        [Authorize]
        [HttpGet("info")]
        public async Task<ActionResult<UserPublic>> GetMyUser() {
            var info = await _userManager.FindByIdAsync (User.FindFirstValue (ClaimTypes.NameIdentifier));
            return createUserPublic (info);
        }

        //GET: /Users/info/{id user}
        // get info of one user
        [HttpGet ("info/{id}")]
        [Authorize]
        public async Task<ActionResult<UserPublic>> GetUser (String id) {
            var info = await _userManager.FindByIdAsync (User.FindFirstValue (ClaimTypes.NameIdentifier));
            Guid x;
            if (info.Id.ToString () == id)
                return createUserPublic (info);
            else if ((await _userManager.GetRolesAsync (info))[0] != "Admin" || !Guid.TryParse (id, out x))
                return NotFound ();
            var rawInfo = await _userManager.FindByIdAsync (id);
            var publicUser = createUserPublic (rawInfo);
            if (rawInfo == null) {
                return NotFound ();
            }
            return publicUser;
        }

        [HttpPost ("setInfo")]
        [Authorize]
        public async Task<ActionResult> SetUser (UserPublic user) {
            var info = await _userManager.FindByIdAsync (User.FindFirstValue (ClaimTypes.NameIdentifier));
            Guid x;
            if (info.Id == user.Id)
                SetUserInfoFromUserPublic (user);
            else if ((await _userManager.GetRolesAsync (info))[0] != "Admin")
                return NotFound ();
            var rawInfo = await _userManager.FindByIdAsync(user.Id.ToString());
            if (rawInfo == null) {
                return NotFound ();
            }
            SetUserInfoFromUserPublic(user);
            return Ok ();
        }

        //GET: /Users/formation/{id user}
        // get all formation of one user
        [HttpGet ("formation/{id}")]
        public async Task<ActionResult<IEnumerable<applicant_session>>> GetUserFormation (String id) {
            var AllApplicantSessions = await _context.applicant_sessions.ToListAsync ();
            var ApplicantSessionsFilterById = AllApplicantSessions.FindAll (elem => elem.id.Equals (id));

            return AllApplicantSessions;
        }

        // //PUT: /User/session/{id}
        // // subscribe a session of one formation
        // [HttpGet("session/{id}")]
        // public  async Task<ActionResult<applicant_session>> PutUserSession(String id){

        // }

    }

}