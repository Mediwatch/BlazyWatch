using Mediwatch.Shared;
using Mediwatch.Server.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Google;
using System;

namespace Mediwatch.Server.Controllers
{
    [ApiController]
    [Route("/[controller]/[action]")]
    public class AccountController : ControllerBase
    {

        private readonly SignInManager<UserCustom> signInManager;
        private readonly UserManager<UserCustom> userManager;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        
        public AccountController(SignInManager<UserCustom> signInManager, UserManager<UserCustom> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        /// <summary>
        /// Connectez l'utilisateur à son compte 
        /// API POST: /Account/Login
        /// </summary>
        /// <param name="login"> formulaire contenant les informations de connexion </param>
        /// <returns>Ok 200 quand cela fonctionne avec le cookie Auth et erreur dans les autres cas avec l'erreur associée</returns>
        
        [HttpPost]
        async public Task<IActionResult> Login(LoginForm login)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState.Values.SelectMany(state => state.Errors)
                .Select(error => error.ErrorMessage).FirstOrDefault());

            var signUser = await userManager.FindByNameAsync(login.UserName);
            if (signUser == null)
                return BadRequest("User Name or Password isn't valid");
            var pasword = await signInManager.CheckPasswordSignInAsync(signUser, login.Password, false);
            if (!pasword.Succeeded)
                return BadRequest("User Name or Password isn't valid");
            
            await signInManager.SignInAsync(signUser, true);
            return Ok();
        }

        /// <summary>
        /// Ajouter un tuteur lorsque vous êtes administrateur
        /// API GET: /Account/AddTutor
        /// </summary>
        /// <param name="id">id du compte à modifier</param>
        /// <returns>ok 200 quand ça marche</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        async public Task<IActionResult> AddTutor(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
                return BadRequest("User Name or Password isn't valid");
            foreach (var roleName in new string[] {"Admin", "Member"})
            {
                var deletionResult = await userManager.RemoveFromRoleAsync(user, roleName);
            }
            var role = await userManager.AddToRoleAsync(user, "Tutor");
            if (!role.Succeeded)
                return BadRequest(role.Errors.FirstOrDefault());
            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LoginExternal(string hey)
        {
            var redirectUrl = Url.Action("LoginExtCallback", "Account");
            
            var properties = signInManager.ConfigureExternalAuthenticationProperties(GoogleDefaults.AuthenticationScheme, redirectUrl);
            return new ChallengeResult(GoogleDefaults.AuthenticationScheme, properties);
        }

        public async Task<IActionResult> LoginExtCallback(string returnUrl,string remoteError = null)
        {

            if (remoteError != null)
                return BadRequest(remoteError);

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToPage("Can't find external login");

            var signIn = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);
            if (signIn.Succeeded)
                return Redirect("~/");
            
            var userEmail = info.Principal.FindFirstValue(ClaimTypes.Email);
            var userName = info.Principal.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(userEmail))
                return BadRequest($"{info.ProviderDisplayName} has no email");
            if (string.IsNullOrEmpty(userName))
                return BadRequest($"{info.ProviderDisplayName} has no username");

            var user = new UserCustom{ Id = System.Guid.NewGuid(), UserName = userName, Email = userEmail};
            var dbUser = await userManager.CreateAsync(user);

            if (!dbUser.Succeeded)
                return BadRequest(dbUser.Errors.FirstOrDefault());
            
            var role = await userManager.AddToRoleAsync(user, "Member");
            if (!role.Succeeded)
                return BadRequest(role.Errors.FirstOrDefault());

            await userManager.AddClaimAsync(user, new Claim("Creation", System.DateTime.Now.ToString()));
            dbUser = await userManager.AddLoginAsync(user, info);
            if (!dbUser.Succeeded)
                return BadRequest(dbUser.Errors.FirstOrDefault());
            await signInManager.SignInAsync(user, true);
            return LocalRedirect("~/");
        }

        /// <summary>
        /// Enregistrer un utilisateur
        /// API POST /Account/Register
        /// </summary>
        /// <param name="register">informations pour l'enregistrement de l'utilisateur</param>
        /// <returns>OK 200 quand ça marche</returns>
        [HttpPost]
        async public Task<IActionResult> Register(RegisterForm register)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState.Values.SelectMany(state => state.Errors)
                    .Select(error => error.ErrorMessage).FirstOrDefault());
            
            var registerUser = new UserCustom();
            registerUser.UserName = register.UserName;
            registerUser.Email = register.EmailAddress;

            var IsUserCreated = await userManager.CreateAsync(registerUser, register.Password);
            if (!IsUserCreated.Succeeded)
                return BadRequest(IsUserCreated.Errors.FirstOrDefault());
            var role = await userManager.AddToRoleAsync(registerUser, "Member");
            if (!role.Succeeded)
                return BadRequest(role.Errors.FirstOrDefault());
            var loginForm = new LoginForm {UserName = register.UserName, Password = register.Password};
            return await Login(loginForm);
        }
        
        /// <summary>
        /// Donner des informations de base sur l'utilisateur connecté avec cookie
        /// </summary>
        /// <returns>les informations de l'utilisateur avec authentifié à faux si ce n'est pas le cas</returns>
        [HttpGet]
        public UserInformation UserInfo()
        {
            
            return new UserInformation
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
                Claims = User.Claims
                    .ToDictionary(c => c.Type, c => c.Value),
            };
        }

    }
}