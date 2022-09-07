﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AlchimonAng.Models;
using AlchimonAng.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace AlchimonAng.Services
{
    public interface IUserService
    {
        Task<AuthenticationRespViewModel> Registration(Player newPlayer);
        Task<AuthenticationRespViewModel> Authentication(string nik, string password);
        string GetRoster();
        Player GetPlayer(string id);
        Task PutPlayer(Player player);
    }

    public class SimpleUserService : IUserService
    {
        private readonly IPlayerRepository _playerRepository;


        public SimpleUserService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public async Task<AuthenticationRespViewModel> Registration(Player newPlayer)
        {
            var roster = _playerRepository.GetList().Result;
            var exist = roster.FirstOrDefault(p => p.Email == newPlayer.Email);
            if (exist is not null) throw new Exception("Пользователь с таким Email уже зарегистрирован");
            newPlayer.Id = Guid.NewGuid().ToString();
            newPlayer.Nik = "Defoult";
            if (newPlayer.Email == "ki.termit@gmail.com" || newPlayer.Email == "Ki.termit@gmail.com")
                newPlayer.role = RoleConsts.God;
            else
                newPlayer.role = RoleConsts.Player;
            newPlayer.Money = 100;
            newPlayer.Karman = new Dictionary<int, Alchemon>();
            newPlayer.Password = HashEbota(newPlayer.Password);
            _playerRepository.Create(newPlayer);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, newPlayer.Nik),
                new Claim(ClaimTypes.Role, newPlayer.role),
                new Claim(ClaimTypes.Country, newPlayer.Id)
            };
            var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(15)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new AuthenticationRespViewModel { Good = true, Text = encodedJwt };

        }

        public async Task<AuthenticationRespViewModel> Authentication(string email, string password)
        {
            password = HashEbota(password);
            Player? player = _playerRepository.GetList().Result.FirstOrDefault(p => p.Email.ToLower() == email.ToLower() && p.Password == password);
            if (player is null) return new AuthenticationRespViewModel { Good = false, Text = "Неверный логин или пароль" };
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, player.Nik),
                new Claim(ClaimTypes.Role, player.role),
                new Claim(ClaimTypes.Country, player.Id)
            };
            var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(15)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            Console.WriteLine(encodedJwt);
            return new AuthenticationRespViewModel { Good = true, Text = encodedJwt };
        }

        public string GetRoster()
        {
            return string.Join
                (
                "\n\n",
                _playerRepository.GetList().Result.Select(kv => kv.ToString()).ToArray()
                );
        }

        public Player GetPlayer(string id)
        {
            var player = _playerRepository.GetOne(id).Result;
            if (player is null) throw new Exception($"Игрок не найден id: {id}");
            return player;
        }

        public async Task PutPlayer(Player player)
        {
            await _playerRepository.Update(player);
        }


        private string HashEbota(string pass)
        {
            SHA256 sha256 = SHA256Managed.Create();
            UTF8Encoding objUtf8 = new UTF8Encoding();
            var hashValue = sha256.ComputeHash(objUtf8.GetBytes(pass));
            return Convert.ToBase64String(hashValue);
        }

    }
}

