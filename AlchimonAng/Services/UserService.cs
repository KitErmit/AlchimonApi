using System;
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
        Task<BoolTextRespViewModel> Registration(Player newPlayer);
        Task<BoolTextRespViewModel> Authentication(string nik, string password);
        Task<IList<Player>> GetRoster();
        Player GetPlayer(string id);
        Task<Player> PutPlayer(Player player);
    }

    public class SimpleUserService : IUserService
    {
        private readonly IPlayerRepository _playerRepository;


        public SimpleUserService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public async Task<BoolTextRespViewModel> Registration(Player newPlayer)
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
            newPlayer.Password = PasswordToHash(newPlayer.Password);
            string respID = await _playerRepository.Create(newPlayer);

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

            return new BoolTextRespViewModel { Good = true, Text = $"PlayerID: {respID} JWT: " + encodedJwt };

        }

        public async Task<BoolTextRespViewModel> Authentication(string email, string password)
        {
            password = PasswordToHash(password);
            Player? player = _playerRepository.GetList().Result.FirstOrDefault(p => p.Email.ToLower() == email.ToLower() && p.Password == password);
            if (player is null) return new BoolTextRespViewModel { Good = false, Text = "Неверный логин или пароль" };
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
            return new BoolTextRespViewModel { Good = true, Text = encodedJwt };
        }

        public async Task<IList<Player>> GetRoster()
        {
            var list = await _playerRepository.GetList();

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


        private string PasswordToHash(string pass)
        {
            SHA256 sha256 = SHA256Managed.Create();
            UTF8Encoding objUtf8 = new UTF8Encoding();
            var hashValue = sha256.ComputeHash(objUtf8.GetBytes(pass));
            return Convert.ToBase64String(hashValue);
        }

    }
}

