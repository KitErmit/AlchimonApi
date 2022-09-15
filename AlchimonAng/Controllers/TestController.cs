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

        public TestController(
            ILogger<AdminController> logger,
            IUserService uService,
            IPlayerRepository playerRepository,
            TestService testService
            )
        {
            _testService = testService;
            _logger = logger;
            _uService = uService;
            _playerRepository = playerRepository;
        }

        [HttpGet("Foo")]
        public async Task<IActionResult> Foo()
        {
            return Ok(await _testService.Foo(User));
        }


    }
}

