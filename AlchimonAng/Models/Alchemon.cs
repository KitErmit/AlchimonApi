using System;
namespace AlchimonAng.Models
{
    public class Alchemon : ICloneable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AlName { get; set; }
        public string Emoji { get; set; }
        //public Spacial spacial { get; set; }
        public int Hp { get; set; }
        public int Dmg { get; set; }
        public int Agi { get; set; }

        public int Tear { get; set; }
        public int Maxpoint { get; set; }
        public string Noise { get; set; }



        public Alchemon(string alName, string Emoji, int hp, int dmg, int agi, string noise)
        {
            AlName = alName;
            this.Emoji = Emoji;
            Hp = hp;
            Dmg = dmg;
            Agi = agi;
            Tear = 1;
            Name = "None";
            Noise = noise;
            Maxpoint = 70;
        }


        public object Clone()
        {
            return MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{AlName} {Emoji}";
        }
    }
}

