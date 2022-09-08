using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AlchimonAng.Models;
using AlchimonAng.Services;
using AlchimonAng.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AlchimonAng.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IUserService _uService;
        private readonly IPlayerRepository _playerRepository;

        private readonly IAdminService _adminService;

        public AdminController(
            ILogger<AdminController> logger,
            IUserService uService,
            IPlayerRepository playerRepository,
            IAdminService adminService
            )
        {
            _adminService = adminService;
            _logger = logger;
            _uService = uService;
            _playerRepository = playerRepository;
        }

        [HttpGet("rolecheck")]
        public async Task<IActionResult> Rolecheck()
        {
            ClaimsIdentity? identity = User.Identity as ClaimsIdentity;
            if (!identity.IsAuthenticated) return Ok(new BoolTextRespViewModel { Good = false, Text = "Клаймс ID в Rolecheck пусты" });
            if (identity is null) return Ok(new BoolTextRespViewModel { Good = false, Text = "Клаймс ID в Rolecheck пусты" });
            string? role = identity.FindFirst(ClaimTypes.Role).Value;
            if (role is null) return Ok(new BoolTextRespViewModel { Good = false, Text = "Клаймс ID в Rolecheck пусты" });
            if (role == RoleConsts.God) return Ok(new BoolTextRespViewModel { Good = true, Text = "ok" });
            else return Ok(new BoolTextRespViewModel { Good = false, Text = "не бог" });
        }

        [Authorize(Roles = RoleConsts.God)] // 
        [HttpGet("GetRoster")]
        public async Task<IActionResult> GitRoster()
        {
            return Ok(await _uService.GetRoster());
        }
        [Authorize(Roles = RoleConsts.God)] // 
        [HttpPost("delet")]
        public async Task<IActionResult> DeletPlaye([FromBody] BoolTextRespViewModel req)
        {

            try
            {
                await _adminService.DeletePlayer(req.Text);
            }
            catch(Exception ex)
            {
                return Ok(new BoolTextRespViewModel { Good = false, Text = ex.Message });
            }
            return Ok(new BoolTextRespViewModel { Good = true, Text = "Ошибок нет" });


        }
    }
}

