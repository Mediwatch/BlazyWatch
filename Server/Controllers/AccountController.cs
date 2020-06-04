using Mediwatch.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Google;


namespace Mediwatch.Server.Controllers
{
    [ApiController]
    [Route("/[controller]/[action]")]
    public class AccountController : ControllerBase
    {

        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        
        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        
        [HttpPost]
        async public Task<IActionResult> Login(LoginForm login)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState.Values.SelectMany(state => state.Errors)
                .Select(error => error.ErrorMessage).FirstOrDefault());

            var signUser = await userManager.FindByNameAsync(login.UserName);
            if (signUser == null)
                return BadRequest("User Name or Password is'nt valid");
            var pasword = await signInManager.CheckPasswordSignInAsync(signUser, login.Password, false);
            if (!pasword.Succeeded)
                return BadRequest("User Name or Password is'nt valid");
            
            await signInManager.SignInAsync(signUser, login.RememberMe);
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        async public Task<IActionResult> AddTutor(string username)
        {
            if (username == "Admin" || username == "")
                return BadRequest("Can't modify his role");
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
                return BadRequest("User Name or Password is'nt valid");
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
                return RedirectToPage("Can't find externall login");

            var signIn = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);
            if (signIn.Succeeded)
                return Redirect("~/");
            
            var userEmail = info.Principal.FindFirstValue(ClaimTypes.Email);
            var userName = info.Principal.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(userEmail))
                return BadRequest($"{info.ProviderDisplayName} has no email");
            if (string.IsNullOrEmpty(userName))
                return BadRequest($"{info.ProviderDisplayName} has no username");

            var user = new IdentityUser{ Id = System.Guid.NewGuid().ToString(), UserName = userName, Email = userEmail};
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

        [HttpPost]
        async public Task<IActionResult> Register(RegisterForm register)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState.Values.SelectMany(state => state.Errors)
                    .Select(error => error.ErrorMessage).FirstOrDefault());
            
            var registerUser = new IdentityUser();
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