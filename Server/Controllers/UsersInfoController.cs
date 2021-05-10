using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Mediwatch.Shared.Models;
using Mediwatch.Server.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Server;


namespace Mediwatch.Server.Controllers {
    [ApiController]
    [Route ("[controller]")]
    public class UsersController : ControllerBase {

        #region //Config Part
        private readonly UserManager<UserCustom> _userManager;
        private readonly DbContextMediwatch _context;
        public UsersController (UserManager<UserCustom> userManager,
            DbContextMediwatch context) {
            _userManager = userManager;
            _context = context;
        }
       #endregion

        #region //Utilitaire

        /// <summary>
        /// convert user in data base to user public for api
        /// </summary>
        /// <param name="elem"></param>
        /// <returns>return new user as public</returns>
        private async Task<UserPublic> createUserPublic (UserCustom elem) {
            UserPublic node = new UserPublic ();
            node.Id = elem.Id;
            node.Name = elem.UserName;
            node.Email = elem.Email;
            node.PhoneNumber = elem.PhoneNumber;
            node.Role =  (await _userManager.GetRolesAsync (elem))[0];
            return node;
        }
        
        /// <summary>
        /// Convert list of user in data base to user public for api
        /// </summary>
        /// <param name="dataToChange"></param>
        /// <returns></returns>
        private async Task<List<UserPublic>> ConvertInPublicInfo (List<UserCustom> dataToChange) {
            List<UserPublic> PublicUserInfo = new List<UserPublic> ();

            foreach (UserCustom elem in dataToChange) {
                var node = createUserPublic (elem);
                PublicUserInfo.Add (await node);
            }
            return PublicUserInfo;
        }

        /// <summary>
        /// modify user information in database
        /// </summary>
        /// <param name="user"></param>
        private async Task SetUserInfoFromUserPublic (UserPublic user, UserCustom userData) {
            userData.UserName = user.Name;
            userData.Email = user.Email;
            await _userManager.RemoveFromRoleAsync(userData, (await _userManager.GetRolesAsync(userData))[0]);
            await _userManager.AddToRoleAsync(userData, user.Role);
            await _userManager.UpdateAsync(userData);
            return;
        }
        #endregion
        
        #region //Authorize Part
        /// <summary>
        /// list all users if you are admin 
        /// GET: /Users/listUser
        /// </summary>
        /// <returns>return unauthorized if the user role is not admin and a list of public user when it work</returns>
        [HttpGet ("listUser")]
        [Authorize (Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserPublic>>> GetUserList () {
            var rawInfo = await _userManager.Users.ToListAsync ();
            var publicUsers = await ConvertInPublicInfo (rawInfo);
            return publicUsers;
        }

        /// <summary>
        /// Get the information of the connected user
        /// API GET : /Users/info
        /// </summary>
        /// <returns>return a user public user with the information of the connected user</returns>
        [Authorize]
        [HttpGet("info")]
        public async Task<ActionResult<UserPublic>> GetMyUser() {
            var info = await _userManager.FindByIdAsync (User.FindFirstValue (ClaimTypes.NameIdentifier));
            return await createUserPublic (info);
        }
 
        /// <summary>
        /// get info of one user
        /// GET: /Users/info/{id user}
        /// </summary>
        /// <param name="id">the id of the user</param>
        /// <returns>return a  user public if the id correspond to the id of the connected user.
        ///  Return not found in case of not base 64 id, the id is not found in the data base or if you're not admin else
        /// it will return a user public with the information corresponding to the user id</returns>
        [HttpGet ("info/{id}")]
        [Authorize]
        public async Task<ActionResult<UserPublic>> GetUser (String id) {
            var info = await _userManager.FindByIdAsync (User.FindFirstValue (ClaimTypes.NameIdentifier));
            Guid x;
            if (info.Id.ToString () == id)
                return await createUserPublic (info);
            else if ((await _userManager.GetRolesAsync (info))[0] != "Admin" || !Guid.TryParse (id, out x))
                return NotFound ();
            var rawInfo = await _userManager.FindByIdAsync (id);
            var publicUser = createUserPublic (rawInfo);
            if (rawInfo == null) {
                return NotFound ();
            }
            return await publicUser;
        }

        /// <summary>
        /// set info of one user
        /// POST: /Users/setInfo/
        /// BODY: raw/json :UserPublic
        /// </summary>
        /// <param name="user">information to set</param>
        /// <returns>return OK if the id correspond to the id of the connected user.
        ///  Return not found in case of not base 64 id, the id is not found in the data base or if you're not admin else
        /// it will return OK</returns>
        [HttpPost ("setInfo")]
        [Authorize]
        public async Task<ActionResult> SetUser (UserPublic user) {
            var info = await _userManager.FindByIdAsync (User.FindFirstValue (ClaimTypes.NameIdentifier));
            if (info.Id == user.Id)
                await SetUserInfoFromUserPublic (user, info);
            else if ((await _userManager.GetRolesAsync (info))[0] != "Admin")
                return NotFound ();
            var rawInfo = await _userManager.FindByIdAsync(user.Id.ToString());
            if (rawInfo == null) {
                return NotFound ();
            }
            await SetUserInfoFromUserPublic(user, rawInfo);
            return Ok ();
        }
        #endregion

        #region //User Formation User
        /// <summary>
        /// GET: /Users/formation/{id user}
        /// get all formation of one user
        /// </summary>
        /// <param name="id">id user</param>
        /// <returns>return the list of applicant session</returns>
        [HttpGet("formation/{id}")]
        public async Task<ActionResult<IEnumerable<applicant_session>>> GetUserFormation(String id)
        {
            var AllApplicantSessions = await _context.applicant_sessions.ToListAsync();
            var ApplicantSessionsFilterById = AllApplicantSessions.FindAll(elem => elem.id.Equals(id));

            return AllApplicantSessions;
        }

        /// <summary>
        /// POST: /Users/registeruserformation/
        /// BODY: raw/json :applicant_session,
        /// create register user to a Formation
        /// </summary>
        /// <param name="idFormation">id of the formation</param>
        /// <returns>return one applicant session</returns>
        [HttpPost ("registeruserformation")] 
        public async Task<ActionResult<applicant_session>> RegisterUserformation (applicant_session RegisterUser) {
            FormationController _formationController = new FormationController(_context);
            ApplicantSessionController _sessionController = new ApplicantSessionController(_context);
            var searchedForm = await _formationController.GetFormation((int)RegisterUser.idFormation);
            if (searchedForm != null){
                return await _sessionController.PostApplicantSession(RegisterUser);
            }
            return NotFound();
        }
        

        //  formation
        /// <summary>
        /// DEL: /Users/deleteuserformation/
        /// BODY: raw/json :applicant_session,
        /// with id ApplicantSession and IdFormation
        /// create register user to a Formation
        /// </summary>
        /// <param name="body.id">id of the applicantSession</param>
        /// <param name="body.idUser">idUser of the applicantSession</param>
        /// <returns>return one applicant session</returns>
        [HttpDelete("deleteuserformation")]
        public async Task<ActionResult<applicant_session>> DeleteUserFormation(applicant_session body)
        {
            var AllApplicantSessions = await _context.applicant_sessions.ToListAsync();
            var applicantSessionsFilterById = AllApplicantSessions.Find(elem => elem.id.Equals(body.id) && elem.idUser.Equals(body.idUser));
            if (applicantSessionsFilterById == null)
            {
                return NotFound();
            }
            _context.applicant_sessions.Remove(applicantSessionsFilterById);
            await _context.SaveChangesAsync();
            return Ok();
        }

        #endregion
    }

}