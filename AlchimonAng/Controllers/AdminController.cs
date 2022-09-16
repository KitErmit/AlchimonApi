using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AlchimonAng.Models;
using AlchimonAng.Services;
using AlchimonAng.Utils.Constans;
using AlchimonAng.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AlchimonAng.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : BaseControllerApi
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IAdminService _adminService;

        public AdminController(
            ILogger<AdminController> logger,
            IAdminService adminService
            )
        {
            _adminService = adminService;
            _logger = logger;
        }

        [HttpGet("rolecheck")]
        public async Task<IActionResult> Rolecheck()
        {
            return await MakeResponse(_adminService.RoleCheck(User));
        }

        [Authorize(Roles = PlayerRoleConsts.God)] 
        [HttpGet("GetRoster")]
        public async Task<IActionResult> GitRoster()
        {
            return await MakeResponse(_adminService.GetRoster());
        }

        [Authorize(Roles = PlayerRoleConsts.God)] 
        [HttpPost("delet")]
        public async Task<IActionResult> DeletPlaye([FromBody] BoolTextRespViewModel req)
        {
            return await MakeResponse(_adminService.DeletePlayer(req.Text));
        }

        


    }
}

