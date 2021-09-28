using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Server.Services.Interface;
using Mediwatch.Shared;

namespace Server.Controllers
{
    [ApiController]
    [Route("/[controller]/[action]")]
    public class AdminManagerController : ControllerBase
    {
        private AdminService adminService;
        public AdminManagerController(IAdminServiceAccessor accessor) {
            adminService = accessor.Service;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
         public IActionResult SetAdmin(AdminInfo info) {
            adminService.isInDemo = info.isInDemo;
            adminService.canPay = info.canPay;
            return Ok();
        }
        [HttpGet]
         public AdminInfo GetAdmin() {
            var info = new AdminInfo();
            info.isInDemo=adminService.isInDemo;
            info.canPay= adminService.canPay;
            return info;
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
         public IActionResult SetArctileNews(string[] article) {
            adminService.articleNew = article; 
            return Ok();
        }
        [HttpGet]
         public string[] GetArctileNews() {
            if (adminService.articleNew != null)
                return adminService.articleNew;
            else return new string[1];
        }

    }
}