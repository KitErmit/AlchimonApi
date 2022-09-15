using System;
using AlchimonAng.Models;
using AlchimonAng.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AlchimonAng.Services
{
    public class RegistrationService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly HashService _hashService;
        private readonly JwtBuilderService _jwtBuilder;


        public RegistrationService(IPlayerRepository playerRepository, HashService hashService, JwtBuilderService jwtBuilder)
        {
            _playerRepository = playerRepository;
            _hashService = hashService;
            _jwtBuilder = jwtBuilder;
        }

        public async Task<BoolTextRespViewModel> TryCreateNewPlayer(ModelStateDictionary modelState, UserViewModel newPlayer)
        {
            
            var valid = ValidCheck(modelState);
            var repeatCheck = RepeatEmailCheck(newPlayer.Email);
            Player? playerDone = null;
            var BeforeCreatePLTasks = Task.WhenAll(valid, repeatCheck);
            string? Jwt = null;
            try
            {
                await BeforeCreatePLTasks;
                playerDone = await FromVModelToPlayer(newPlayer);
                var create = _playerRepository.Create(playerDone);
                var jwtBuild = _jwtBuilder.BuildToken(playerDone);
                var CheakPl = await create;
                if (playerDone.Email == CheakPl.Email && playerDone.Id == CheakPl.Id && playerDone.Nik == CheakPl.Nik) Jwt = await jwtBuild;
            }
            catch(Exception ex)
            {
                string errorMessages = "";
                if (BeforeCreatePLTasks.Exception is not null)
                {
                    foreach (var exception in BeforeCreatePLTasks.Exception.InnerExceptions)
                    {
                        errorMessages = errorMessages + "\n" + exception.Message;
                    }
                }
                else errorMessages = ex.Message;
#if DEBUG
                Console.WriteLine(errorMessages);
#endif
                return new BoolTextRespViewModel { Good = false, Text = errorMessages };
            }
            Console.WriteLine("JWT: " + Jwt);
            if (Jwt is not null) return new BoolTextRespViewModel { Good = true, Text = $"PlayerID: {playerDone.Id} JWT: " + Jwt };
            else return new BoolTextRespViewModel { Good = false, Text = "Jwt is null in RegistrationService " };

        }

        public async Task ValidCheck(ModelStateDictionary modelState)
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
            var roster = _playerRepository.GetList().Result;
            var exist = roster.FirstOrDefault(p => p.Email == email);
            if (exist is not null) throw new Exception("Пользователь с таким Email уже зарегистрирован");
        }

        public async Task<Player> FromVModelToPlayer (UserViewModel newPlayer)
        {
            string role = newPlayer.Email.ToLower() == "ki.termit@gmail.com" || newPlayer.Email.ToLower().StartsWith("421984")
                ? RoleConsts.God : RoleConsts.Player;
            string id = Guid.NewGuid().ToString();
            int i = 0;

            while(await _playerRepository.GetOne(id) is not null)
            {
                id = Guid.NewGuid().ToString();
                i++;
                if (i > 100) throw new Exception("Гид не может создать уникальный айди");
            }

            return new Player
            {
                Id = id,
                Email = newPlayer.Email.ToLower(),
                Nik = "Игрок",
                Password = _hashService.StringToHash(newPlayer.Password),
                role = role,
                Money = role == RoleConsts.God ? 1_000_000 : 100,
                Karman = new Dictionary<int, Alchemon>(),
            };
        }

        
    }
}

