using Microsoft.AspNetCore.Mvc;
using AlchimonAng.ViewModels;
using AlchimonAng.Services;
using Microsoft.AspNetCore.Authorization;
using AlchimonAng.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;
using System.Security.Claims;


//Что если авторизацию и регистрацию встроить ngif'ом в nav-menu, а router-outlet в router-outlet отображать, если авторизация прошла
// Не забыть поменять адрес у запроса на регистрацию


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

    
    [HttpPost]
    public async Task<AuthenticationRespViewModel> RegConfarm([FromBody] Player player)
    {
        var validble = true;
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
            return new AuthenticationRespViewModel { Good=false, Text = errorMessages };
        }

        AuthenticationRespViewModel regPlayer;
        try
        {
            regPlayer = await _uService.Registration(player);
            return regPlayer;
        }
        catch (Exception e)
        {
            return new AuthenticationRespViewModel { Good = false, Text = e.Message };
        }
    }


    [HttpPost("Authorize")]
    public async Task<AuthenticationRespViewModel> Authorize([FromBody] UserViewModel user)
    {
        Console.WriteLine("Зашли в autorization");
        var resp = await (new StreamReader(Request.Body)).ReadToEndAsync();
        return await _uService.Authentication(user.Email, user.Password);
    }

    [Authorize(Roles = RoleConsts.God)] // 
    [HttpGet("GetRoster")]
    public string GitRoster()
    {
        return _uService.GetRoster();
    }

    [Authorize]
    [HttpGet("play")]
    public Player PlInform()
    {
        var identity = User.Identity as ClaimsIdentity;
        if (identity is null) return new Player { Email = "клаймсы пусты" };
        string? id = identity.FindFirst(ClaimTypes.Country).Value;
        if (id is null) return new Player { Email = "клаймс ID пуст" };
        return _uService.GitPlayer(id);
    }


    
}

