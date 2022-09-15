using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AlchimonAng.Models;
using AlchimonAng.ViewModels;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AlchimonAng.Services
{
    public interface IUserService
    {
        Task<BoolTextRespViewModel> Registration(ModelStateDictionary modelState, UserViewModel newPlayer);
        Task<BoolTextRespViewModel> Authentication(string nik, string password);
        Task<IList<Player>> GetRoster();
        Player GetPlayer(string id);
        Task<Player> PutPlayer(Player player);
    }

    public class SimpleUserService : IUserService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly HashService _hashService;
        private readonly RegistrationService _registrationService;
        private readonly JwtBuilderService _jwtBuilder;


        public SimpleUserService
            (
            IPlayerRepository playerRepository,
            HashService hashService,
            RegistrationService registrationService,
            JwtBuilderService jwtBuilder
            )
        {
            _playerRepository = playerRepository;
            _hashService = hashService;
            _registrationService = registrationService;
            _jwtBuilder = jwtBuilder;
        }

        public async Task<BoolTextRespViewModel> Registration(ModelStateDictionary modelState, UserViewModel newPlayerr)
        {
            return await _registrationService.TryCreateNewPlayer(modelState, newPlayerr);
        }

        public async Task<BoolTextRespViewModel> Authentication(string email, string password)
        {
            password = _hashService.StringToHash(password);
            Player? player = _playerRepository.GetList().Result.FirstOrDefault(p => p.Email.ToLower() == email.ToLower() && p.Password == password);
            if (player is null) return new BoolTextRespViewModel { Good = false, Text = "Неверный логин или пароль" };


            var encodedJwt = await _jwtBuilder.BuildToken(player);

            return new BoolTextRespViewModel { Good = true, Text = encodedJwt };
        }

        public async Task<IList<Player>> GetRoster()
        {
            var list = await _playerRepository.GetList();
            var gods = (IList<Player>)list.Where(p => p.role == RoleConsts.God).ToList();
            var players = (IList<Player>)list.Where(p => p.role == RoleConsts.Player).ToList();
            list = gods.Concat(players).ToList(); ;
            
            return list;
        }

        public Player GetPlayer(string id)
        {
            var player = _playerRepository.GetOne(id).Result;
            if (player is null) throw new Exception($"Игрок не найден id: {id}");
            return player;
        }

        public async Task<Player> PutPlayer(Player player)
        {
            return await _playerRepository.Update(player);
        }


        

    }
}

