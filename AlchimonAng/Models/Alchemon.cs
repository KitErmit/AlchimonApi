using System;
namespace AlchimonAng.Models
{
    public class Alchemon : ICloneable
    {
        public string Id { get; set; }  //guid id
        public string Name { get; set; } //name by player
        public string AlName { get; set; } //biological species
        public string Emoji { get; set; } // Emoji species
        
        public int Hp { get; set; }
        public int Dmg { get; set; }
        public int Agi { get; set; }

        public int Tear { get; set; }
        public string Noise { get; set; } //sound made by alchimon

        public int Lvl { get; set; }
        public int Xp { get; set; } //amount of xp the alchimon has
        public int NxtLvlXp { get; set; } //max amount of xp you need to lvl up

        public string PlayerId { get; set; }
        public Player Player { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

    }
}

