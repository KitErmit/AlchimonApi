using System;
using System.ComponentModel.DataAnnotations;
namespace AlchimonAng.Models
{
    public class Player
    {
        public string? Id { get; set; }
        public string? Nik { get; set; }
        [Required(ErrorMessage = "Не указан Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }
        [Required]
        [StringLength(12, MinimumLength = 8, ErrorMessage = "Длина строки должна быть от 8 до 12 символов")]
        [RegularExpression(@"(?=^.{8,12}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$", ErrorMessage = "Некорректные символы пароля")]
        public string Password { get; set; }
        public string? role { get; set; }
        public int? Money { get; set; }
        public Dictionary<int, Alchemon>? Karman { get; set; }






        public override string ToString()
        {
            return $"Id: {Id}\nNik: {Nik}\nPass: {Password}\nRole: {role}\nMoney: {Money}\nKarman.Count {Karman.Count}";
        }
    }

    public static class RoleConsts
    {
        public const string Player = "player";
        public const string God = "god";
    }
}




