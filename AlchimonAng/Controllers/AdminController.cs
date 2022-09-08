using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlchimonAng.Services;
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

        public AdminController(
            ILogger<AdminController> logger,
            IUserService uService,
            IPlayerRepository playerRepository
            )
        {
            _logger = logger;
            _uService = uService;
            _playerRepository = playerRepository;
        }

        [HttpGet("rolecheck")]
        public async Task<IActionResult> Rolecheck()
        {

        }
        
    }
}

