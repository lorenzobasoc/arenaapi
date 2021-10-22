using System;
using System.Text.Json.Serialization;

namespace ArenaApi.Models
{
    public abstract class Fighter 
    {
        public int Id { get; set; }
        public int Pv { get; set; }
        public int Speed { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public abstract void Fight(Fighter attacker);
        public abstract int CalculateDamage();

        public abstract event Action<Fighter> Died;
        public abstract event Action<Fighter, Fighter, int> Damaged;

    }
}
