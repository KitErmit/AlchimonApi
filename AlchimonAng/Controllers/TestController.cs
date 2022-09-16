using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AlchimonAng.DB.Repository;
using AlchimonAng.Models;
using AlchimonAng.Services;
using AlchimonAng.Utils.Configs;
using AlchimonAng.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AlchimonAng.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IUserService _uService;
        private readonly IPlayerRepository _playerRepository;

        private readonly TestService _testService;
        private readonly AdminConfig _adminConf;

        public TestController(
            ILogger<AdminController> logger,
            IUserService uService,
            IPlayerRepository playerRepository,
            TestService testService,
            IOptions<AdminConfig> adminConf
            )
        {
            _testService = testService;
            _logger = logger;
            _uService = uService;
            _playerRepository = playerRepository;
            _adminConf = adminConf.Value;
        }

        [HttpGet("Foo")]
        public async Task<IActionResult> Foo()
        {
            return Ok(await _testService.Foo(User));
        }
        [HttpGet("E")]
        public IActionResult emails()
        {
            var resp = string.Join(" ", _adminConf.Emails);
            return Ok(resp);
        }


    }
}

