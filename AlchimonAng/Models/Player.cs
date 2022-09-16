using System;
using System.ComponentModel.DataAnnotations;
namespace AlchimonAng.Models
{
    public class Player
    {
        public string? Id { get; set; }
        public string? Nik { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? role { get; set; }
        public int? Money { get; set; }
        public Dictionary<int, Alchemon>? Karman { get; set; }





        public override string ToString()
        {
            return $"Id: {Id}\nNik: {Nik}\nPass: {Password}\nRole: {role}\nMoney: {Money}\nKarman.Count {Karman.Count}";
        }
    }

    
}




