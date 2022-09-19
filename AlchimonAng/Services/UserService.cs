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
using AlchimonAng.Utils.Constans;
using AlchimonAng.DB.Repository;
using AlchimonAng.Helpers;
using AlchimonAng.Providers;
using Microsoft.AspNetCore.Mvc;
using AlchimonAng.Accessors;

namespace AlchimonAng.Services
{
    public interface IUserService
    {
        Task<BoolTextRespViewModel> Registration(ModelStateDictionary modelState, UserViewModel newPlayer);
        Task<BoolTextRespViewModel> Authentication(string nik, string password);
        Task<Player> GetPlayer(ClaimsPrincipal user);
        Task<BoolTextRespViewModel> PutPlayer(Player updatedPlayer);
        Task<BoolTextRespViewModel> TokenCheck(ClaimsPrincipal user);
    }

    public class SimpleUserService : IUserService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly HashHepler _hashService;
        private readonly RegistrationService _registrationService;
        private readonly JwtProvider _jwtService;
        private readonly UserContextAccessor _userAccessor;


        public SimpleUserService
            (
            IPlayerRepository playerRepository,
            HashHepler hashService,
            RegistrationService registrationService,
            JwtProvider jwtServica,
            UserContextAccessor userAccessor
            )
        {
            _playerRepository = playerRepository;
            _hashService = hashService;
            _registrationService = registrationService;
            _jwtService = jwtServica;
            _userAccessor = userAccessor;
        }

        public async Task<BoolTextRespViewModel> Registration(ModelStateDictionary modelState, UserViewModel newPlayerr)
        {
            return await _registrationService.TryCreateNewPlayer(modelState, newPlayerr);
        }

        public async Task<BoolTextRespViewModel> Authentication(string email, string password)
        {
            password = _hashService.StringToSha256(password);
            Player? player = _playerRepository.GetList().Result.FirstOrDefault(p => p.Email.ToLower() == email.ToLower() && p.Password == password);
            if (player is null) throw new Exception( "Неверный логин или пароль");


            var encodedJwt = await _jwtService.BuildToken(player);

            return new BoolTextRespViewModel { Good = true, Text = encodedJwt };
        }

        

        public async Task<Player> GetPlayer(ClaimsPrincipal user)
        {
            ClaimsUserViewModel? ClaimUser = _userAccessor.GetClimsParams(user);
            var player = await _playerRepository.GetOne(ClaimUser.Id);
            if (player is null) throw new Exception($"Игрок не найден id: {ClaimUser.Nik}");
            return player;
        }

        public async Task<BoolTextRespViewModel> PutPlayer(Player updatedPlayer)
        {
            var player = await _playerRepository.Update(updatedPlayer);
            if (player is null) throw new Exception("Не найден пользователь по итогу обновления");
            return new BoolTextRespViewModel { Good = true, Text = $"Готово. {player.Nik} id: {player.Id}" };
        }

        public async Task<BoolTextRespViewModel> TokenCheck(ClaimsPrincipal user)
        {
            ClaimsUserViewModel? ClaimUser = _userAccessor.GetClimsParams(user);
            if (ClaimUser.Id is null) throw new Exception("Айди пуст");
            var pl = await _playerRepository.GetOne(ClaimUser.Id);
            if(pl is null)throw new Exception("Не нашли пользователя по айди " + ClaimUser.Id);
            return new BoolTextRespViewModel { Good = true, Text = pl.Nik };
        }

    }
}

