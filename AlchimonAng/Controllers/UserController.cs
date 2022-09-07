using Microsoft.AspNetCore.Mvc;
using AlchimonAng.ViewModels;
using AlchimonAng.Services;
using Microsoft.AspNetCore.Authorization;
using AlchimonAng.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;
using System.Security.Claims;


//Что если авторизацию и регистрацию встроить ngif'ом в nav-menu, а router-outlet в router-outlet отображать, если авторизация прошла


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
        string errorMessages = "";

        if (!ModelState.IsValid)
        {
            foreach (var item in ModelState)
            {
                if (item.Value.ValidationState == ModelValidationState.Invalid)
                {
                    errorMessages = $"{errorMessages}\nОшибки для свойства {item.Key}:\n";
                    // пробегаемся по всем ошибкам
                    foreach (var error in item.Value.Errors)
                    {
                        errorMessages = $"{errorMessages}{error.ErrorMessage}\n";
                    }
                }
            }
            return Ok(new AuthenticationRespViewModel { Good=false, Text = errorMessages });
        }

        AuthenticationRespViewModel regPlayer;
        try
        {
            Player newplayer = new Player { Email=player.Email, Password=player.Password};
            regPlayer = await _uService.Registration(newplayer);
            return Ok(regPlayer);
        }
        catch (Exception e)
        {
            return Ok(new AuthenticationRespViewModel { Good = false, Text = e.Message });
        }
    }


    [HttpPost("Authorize")]
    public async Task<AuthenticationRespViewModel> Authorize([FromBody] UserViewModel user)
    {
        Console.WriteLine("Зашли в autorization");
        return await _uService.Authentication(user.Email, user.Password);
    }

    [HttpGet("AuthValid")]
    public async Task<IActionResult> AuthValid()
    {
        ClaimsIdentity? identity = User.Identity as ClaimsIdentity;
        if(!identity.IsAuthenticated) return Ok(new AuthenticationRespViewModel { Good = false, Text = "Клаймс ID в AuthValid пусты" });
        if (identity is null) return Ok(new AuthenticationRespViewModel { Good = false, Text = "Клаймс ID в AuthValid пусты" });
        string? id = identity.FindFirst(ClaimTypes.Country).Value;
        if (id is null) return Ok(new AuthenticationRespViewModel { Good = false, Text = "Клаймс ID в AuthValid пусты" });
        else if (id.Length > 1)
        {
            try
            {
                var pl = _uService.GetPlayer(id);
                if(pl is not null) return Ok(new AuthenticationRespViewModel { Good = true, Text = pl.Nik});
            }
            catch(Exception ex)
            {
                return Ok(new AuthenticationRespViewModel { Good = false, Text = $"{ex.Message}" });
            }
            return Ok(new AuthenticationRespViewModel { Good = false, Text = $"Не знаю, как оно вышло за пределы трайкэч в AuthValid" });
        } 
        else return Ok(new AuthenticationRespViewModel { Good = false, Text = "Что-то не так в AuthValid" });
    }

    [Authorize(Roles = RoleConsts.God)] // 
    [HttpGet("GetRoster")]
    public string GitRoster()
    {
        return _uService.GetRoster();
    }

    [Authorize]
    [HttpGet("getplayer")]
    public Player PlInform()
    {
        var identity = User.Identity as ClaimsIdentity;
        if (identity is null) return new Player { Email = "клаймсы пусты" };
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
        if (identity is null) throw new Exception("Клаймсы в PutPlayer пусты");
        string? id = identity.FindFirst(ClaimTypes.Country).Value;
        if (id is null) return Ok(new AuthenticationRespViewModel { Good = true, Text = "Клаймс ID в PutPlayer пусты" });
        Player newpl = _uService.GetPlayer(id);
        newpl.Nik = player.Nik;
        await _uService.PutPlayer(newpl);
        newpl = _uService.GetPlayer(newpl.Id);
        return Ok(new AuthenticationRespViewModel { Good=true, Text = $"Готово. {newpl.Nik}"});
    }


    
}

