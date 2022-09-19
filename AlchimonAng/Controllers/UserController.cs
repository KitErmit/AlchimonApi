using Microsoft.AspNetCore.Mvc;
using AlchimonAng.ViewModels;
using AlchimonAng.Services;
using Microsoft.AspNetCore.Authorization;
using AlchimonAng.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;
using System.Security.Claims;
using AlchimonAng.DB.Repository;

namespace AlchimonAng.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : BaseControllerApi
{

    private readonly ILogger<UserController> _logger;
    private readonly IUserService _uService;

    public UserController(ILogger<UserController> logger, IUserService uService)
    {
        _logger = logger;
        _uService = uService;
    }


    [HttpPost("Registration")]
    public async Task<IActionResult> Registration([FromBody] UserViewModel player)
    {
        return await MakeResponse(_uService.Registration(ModelState, player));
    }


    [HttpPost("Authorize")]
    public async Task<IActionResult> Authorize([FromBody] UserViewModel user)
    {
        return await MakeResponse(_uService.Authentication(user.Email, user.Password));
    }

    [HttpGet("AuthValid")]
    public async Task<IActionResult> TokenCheck()
    {
        return await MakeResponse(_uService.TokenCheck(User));
    }


    [Authorize]
    [HttpGet("getplayer")]
    public async Task<IActionResult> PlInform()
    {
        return await MakeResponse(_uService.GetPlayer(User));
        
    }

    [Authorize]
    [HttpPut("put_nik")]
    public async Task<IActionResult> PutPlayer([FromBody] Player player)
    {
        return await MakeResponse(_uService.PutPlayer(player));
    }



}

