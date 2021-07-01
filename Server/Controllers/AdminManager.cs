using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Server.Services.Interface;
using Shared;

namespace Server.Controllers
{
    [ApiController]
    [Route("/[controller]/[action]")]
    public class AdminManager : ControllerBase
    {
        private AdminService adminService;
        AdminManager(IAdminServiceAccessor accessor) {
            adminService = accessor.Service;
        }

        [HttpPut]
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
        [HttpPut]
        [Authorize(Roles = "Admin")]
         public IActionResult SetArctileNews(string[] article) {
            adminService.articleNew = article; 
            return Ok();
        }
        [HttpGet]
         public string[] GetArctileNews() {
            return adminService.articleNew;
        }

    }
}