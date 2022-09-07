using System;
using System.ComponentModel.DataAnnotations;

namespace AlchimonAng.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Не указан Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }
        [Required]
        [StringLength(12, MinimumLength = 8, ErrorMessage = "Длина строки должна быть от 8 до 12 символов")]
        [RegularExpression(@"(?=^.{8,12}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$", ErrorMessage = "Некорректные символы пароля")]
        public string Password { get; set; }
        public string Passconf { get; set; }
    }
}

