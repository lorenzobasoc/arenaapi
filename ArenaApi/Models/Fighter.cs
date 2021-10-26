using System;

namespace ArenaApi.Models
{
    public abstract class Fighter 
    {
        public int Id { get; set; }
        public int Pv { get; set; }
        public int Speed { get; set; }
        public string Name { get; set; }
        public bool IsDead { get; set; }
        public string Type { get; set; }

        public abstract void Fight(Fighter attacker, Arena arena);
        public abstract int CalculateDamage();

        public abstract event Action<Fighter, Arena> Died;
        public abstract event Action<Fighter, Fighter, int, Arena> Damaged;
    }
}
