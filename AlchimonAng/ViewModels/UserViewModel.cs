using System;
using System.ComponentModel.DataAnnotations;

namespace AlchimonAng.ViewModels
{
    public class UserViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Passconf { get; set; }
    }
}

