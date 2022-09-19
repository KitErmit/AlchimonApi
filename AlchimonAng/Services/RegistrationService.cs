using System;
using AlchimonAng.Models;
using AlchimonAng.Utils.Configs;
using AlchimonAng.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System.Linq;
using AlchimonAng.Utils.Constans;
using AlchimonAng.DB.Repository;
using AlchimonAng.Helpers;
using AlchimonAng.Providers;

namespace AlchimonAng.Services
{
    public class RegistrationService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly HashHepler _hashService;
        private readonly JwtProvider _jwtBuilder;
        private readonly AdminConfig _adminConf;

        public RegistrationService(IPlayerRepository playerRepository, HashHepler hashService, JwtProvider jwtBuilder, IOptions<AdminConfig> adminConf)
        {
            _playerRepository = playerRepository;
            _hashService = hashService;
            _jwtBuilder = jwtBuilder;
            _adminConf = adminConf.Value;
        }

        public async Task<BoolTextRespViewModel> TryCreateNewPlayer(ModelStateDictionary modelState, UserViewModel newPlayer)
        {
            
            
                

            await RepeatEmailCheck(newPlayer.Email);

            ValidCheck(modelState);
            Player? playerDone = await FromVModelToPlayer(newPlayer);
            var create = _playerRepository.Create(playerDone);
            var jwtBuild = _jwtBuilder.BuildToken(playerDone);
            var CheakPl = await create;
            string? Jwt = null;
            if (playerDone.Email == CheakPl.Email && playerDone.Id == CheakPl.Id && playerDone.Nik == CheakPl.Nik)
                Jwt = await jwtBuild;
#if DEBUG
                Console.WriteLine($"JWT: {Jwt}");
#endif

            if (Jwt is not null) return new BoolTextRespViewModel { Good = true, Text = $"PlayerID: {playerDone.Id} JWT: " + Jwt };
            else throw new Exception("Jwt is null in RegistrationService ");

        }

        public void ValidCheck(ModelStateDictionary modelState)
        {
            string errorMessages = "";
            if (!modelState.IsValid)
            {
                foreach (var item in modelState)
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
                throw new Exception(errorMessages);
            }
        }

        public async Task RepeatEmailCheck(string email)
        {
            var roster = await _playerRepository.GetList();
            var exist = roster.FirstOrDefault(p => p.Email == email);
            if (exist is not null) throw new Exception("Пользователь с таким Email уже зарегистрирован");
        }

        public async Task<Player> FromVModelToPlayer (UserViewModel newPlayer)
        {
            string role = _adminConf.Emails.Any(e => e == newPlayer.Email)
                ? PlayerRoleConsts.God : PlayerRoleConsts.Player;

            return new Player
            {
                Id = Guid.NewGuid().ToString(),
                Email = newPlayer.Email.ToLower(),
                Nik = "Игрок",
                Password = _hashService.StringToSha256(newPlayer.Password),
                role = role,
                Money = role == PlayerRoleConsts.God ? 1_000_000 : 100,
                Karman = new Dictionary<int, Alchemon>(),
            };
        }

        
    }
}

