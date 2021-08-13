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

using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;



/// <summary>
/// Fichier avec les fonctions relatif aux controleurs des UserInfo.
/// Cela permet d'avoir les information publique
/// </summary>
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
        /// convertir User de la base de données en user public pour l'api
        /// </summary>
        /// <param name="elem"></param>
        /// <returns>retourne new user comme UserPublic</returns>
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
        /// Convertir la liste d'User dans la base de données en UserPublic pour l'api
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
        /// modifier les informations User dans la base de données
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
        /// lister tous les Users si vous êtes administrateur
        /// GET: /Users/listUser
        /// </summary>
        /// <returns>retourne "non autorisé" si le rôle d'User n'est pas admin et une liste d'UserPublic quand cela fonctionne</returns>
        [HttpGet ("listUser")]
        [Authorize (Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserPublic>>> GetUserList () {
            var rawInfo = await _userManager.Users.ToListAsync ();
            var publicUsers = await ConvertInPublicInfo (rawInfo);
            return publicUsers;
        }

        /// <summary>
        /// Obtenir les informations de l'User connecté
        /// API GET : /Users/info
        /// </summary>
        /// <returns>retourner un User public avec les informations de l'User connecté</returns>
        [Authorize]
        [HttpGet("info")]
        public async Task<ActionResult<UserPublic>> GetMyUser() {
            var info = await _userManager.FindByIdAsync (User.FindFirstValue (ClaimTypes.NameIdentifier));
            return await createUserPublic (info);
        }
 
        /// <summary>
        /// obtenir des informations sur un User
        /// GET: /Users/info/{id user}
        /// </summary>
        /// <param name="id">the id of the user</param>
        /// <returns>renvoie un UserPublic si l'id correspond à l'id de l'User connecté.
        ///  Retour "NotFound" en cas d'id pas en base 64, ou si l'ID n'est pas trouvé dans la base de données, ou si vous n'êtes pas administrateur sinon
        /// il renverra un UserPublic avec les informations correspondant à l'id du User</returns>
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
        /// définir les informations d'un User
        /// POST: /Users/setInfo/
        /// BODY: raw/json :UserPublic
        /// </summary>
        /// <param name="user">informations à définir</param>
        /// <returns>renvoie OK si l'id correspond à l'id de l'utilisateur connecté.
        ///  Retour "NotFound" en cas d'id pas en base 64, ou si vous n'êtes pas administrateur sinon
        /// cela vous retournera "OK"</returns>
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
        /// obtenir toute la formation d'un User
        /// </summary>
        /// <param name="id">id user</param>
        /// <returns>Retourne une liste de formation</returns>
        [HttpGet("formation/{id}")]
        public async Task<ActionResult<IEnumerable<formation>>> GetUserFormation(String id)
        {
            var _ApplicantSessionController = new ApplicantSessionController(_context);
            var _formationController = new FormationController(_context);
            IEnumerable<applicant_session> getApplicantSession = await _ApplicantSessionController.GetApplicantSession(id);
            List<formation> Result = new List<formation>();
            foreach (var item in getApplicantSession.ToList())
            {
                Result.Add(_formationController
                .GetFormation(item.idFormation)
                .Result
                .Value);
            }
            return Result;
        }

        /// <summary>
        /// POST: /Users/registeruserformation/
        /// BODY: raw/json :applicant_session,
        /// créer inscrire un User à une formation
        /// </summary>
        /// <param name="applicant_session">avec l'id d'une formation et l'id de l'Utilisateur</param>
        /// <returns>retourner une applicant_session</returns>
        [HttpPost ("registeruserformation")] 
        public async Task<ActionResult<applicant_session>> RegisterUserformation (applicant_session RegisterUser) {
            FormationController _formationController = new FormationController(_context);
            ApplicantSessionController _sessionController = new ApplicantSessionController(_context);
            var searchedForm = await _formationController.GetFormation(RegisterUser.idFormation);
            if (searchedForm == null)
                return NotFound();
            return await _sessionController.PostApplicantSession(RegisterUser);
        }
        

        /// <summary>
        /// DEL: /Users/deleteuserformation/
        /// BODY: raw/json :applicant_session,
        /// Avec id ApplicantSession et IdFormation
        /// Supprime ApplicantSession d'une  Formation
        /// </summary>
        /// <param name="body.id">id de l'applicantSession</param>
        /// <param name="body.idUser">idUser de l'applicantSession</param>
        /// <returns>retourne une applicant_session</returns>
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

        [HttpGet ("deleteUser")]
        [Authorize]
        public async Task<ActionResult> DeleteUser ([FromQuery]string id) {
            var info = await _userManager.FindByIdAsync (User.FindFirstValue (ClaimTypes.NameIdentifier));
            if (info.Id.ToString() == id)
                {
                    await _userManager.DeleteAsync(info);
                    return Ok ("working");
                }
            else if ((await _userManager.GetRolesAsync (info))[0] != "Admin")
                return NotFound ();
            var rawInfo = await _userManager.FindByIdAsync(id);
            if (rawInfo == null) {
                return NotFound ();
            }
            await _userManager.DeleteAsync(info);
            return Ok ("erase");
        }

        // Ajout de l'export des formations sous format ics
        [HttpGet("exportCalendar/{id}/calendar.ics")]
        [Route("exportCalendar/{id}/calendar.ics")]
        public async Task<IActionResult> ExportCalendar(string id)
        {
            var _ApplicantSessionController = new ApplicantSessionController(_context);
            var _formationController = new FormationController(_context);
            IEnumerable<applicant_session> getApplicantSession = await _ApplicantSessionController.GetApplicantSession(id);

            var calendar = new Calendar();
            foreach (var item in getApplicantSession.ToList())
            {
                var f = _formationController.GetFormation(item.idFormation).Result.Value;
                calendar.Events.Add(new CalendarEvent
                {
                    Class = "PUBLIC",
                    Summary = f.Name,
                    Created = new CalDateTime(DateTime.Now),
                    Start = new CalDateTime(f.StartDate),
                    End = new CalDateTime(f.EndDate),
                    Description = f.Description,
                    Location = f.Location,
                    Organizer = new Organizer
                    {
                        CommonName = f.OrganizationName + " : " + f.Former,
                    },
                });
            }

            var serializer = new CalendarSerializer();
            var serializedCalendar = serializer.SerializeToString(calendar);
            var bytesCalendar = System.Text.Encoding.UTF8.GetBytes(serializedCalendar);

            return File(bytesCalendar, "application/octet-stream", "calendar.ics");
        }

        #endregion
    }

}