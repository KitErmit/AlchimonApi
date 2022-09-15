using Microsoft.AspNetCore.Mvc;
using AlchimonAng.ViewModels;
using AlchimonAng.Services;
using Microsoft.AspNetCore.Authorization;
using AlchimonAng.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;
using System.Security.Claims;



namespace AlchimonAng.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{

    private readonly ILogger<UserController> _logger;
    private readonly IUserService _uService;
    private readonly IPlayerRepository _playerRepository;

    public UserController(ILogger<UserController> logger, IUserService uService, IPlayerRepository playerRepository)
    {
        _logger = logger;
        _uService = uService;
        _playerRepository = playerRepository;
    }


    [HttpPost("Registration")]
    public async Task<IActionResult> Registration([FromBody] UserViewModel player)
    {
        return Ok(await _uService.Registration(ModelState, player));
    }


    [HttpPost("Authorize")]
    public async Task<BoolTextRespViewModel> Authorize([FromBody] UserViewModel user)
    {
        Console.WriteLine("Зашли в autorization");
        return await _uService.Authentication(user.Email, user.Password);
    }

    [HttpGet("AuthValid")]
    public async Task<IActionResult> AuthValid()
    {
        ClaimsIdentity? identity = User.Identity as ClaimsIdentity;
        if(!identity.IsAuthenticated) return Ok(new BoolTextRespViewModel { Good = false, Text = "Клаймс ID в AuthValid пусты" });
        string? id = identity.FindFirst(ClaimTypes.Country).Value;
        if (id is null) return Ok(new BoolTextRespViewModel { Good = false, Text = "Клаймс ID в AuthValid пусты" });
        else if (id.Length > 1)
        {
            try
            {
                var pl = _uService.GetPlayer(id);
                if(pl is not null) return Ok(new BoolTextRespViewModel { Good = true, Text = pl.Nik});
            }
            catch(Exception ex)
            {
                return Ok(new BoolTextRespViewModel { Good = false, Text = $"{ex.Message}" });
            }
            return Ok(new BoolTextRespViewModel { Good = false, Text = $"Не знаю, как оно вышло за пределы трайкэч в AuthValid" });
        } 
        else return Ok(new BoolTextRespViewModel { Good = false, Text = "Что-то не так в AuthValid" });
    }


    [Authorize]
    [HttpGet("getplayer")]
    public Player PlInform()
    {
        var identity = User.Identity as ClaimsIdentity;
        string? id = identity.FindFirst(ClaimTypes.Country).Value;
        if (id is null) return new Player { Email = "клаймс ID пуст" };
        return _uService.GetPlayer(id);
    }

    [Authorize]
    [HttpPut("put_nik")]
    public async Task<IActionResult> PutPlayer([FromBody] Player player)
    {
        Console.WriteLine(JsonSerializer.Serialize(HttpContext.Request.Headers));
        var identity = User.Identity as ClaimsIdentity;
        string? id = identity.FindFirst(ClaimTypes.Country).Value;
        if (id is null) return Ok(new BoolTextRespViewModel { Good = true, Text = "Клаймс ID в PutPlayer пусты" });
        Player newpl = _uService.GetPlayer(id);
        newpl.Nik = player.Nik;
        var serResp = await _uService.PutPlayer(newpl);
        newpl = _uService.GetPlayer(newpl.Id);
        return Ok(new BoolTextRespViewModel { Good=true, Text = $"Готово. {serResp.Nik} id: {serResp.Id}" });
    }



}

