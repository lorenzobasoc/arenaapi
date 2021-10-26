using System;

namespace ArenaApi.Models
{
    public class Wizard : Fighter
    {
        public int MaximumPower { get; set;}

        public override event Action<Fighter, Arena> Died;
        public override event Action<Fighter, Fighter, int, Arena> Damaged;

        public override int CalculateDamage(){
            return (int)(MaximumPower * new Random().NextDouble());        
        }

         public override void Fight(Fighter attacker, Arena arena){
            var damage = attacker.CalculateDamage(); 
            if (damage < Pv){
                Pv -= damage;
                Damaged?.Invoke(this, attacker, damage, arena);
            } else {
                Pv = 0;
                IsDead = true;
                Damaged?.Invoke(this, attacker, damage, arena);
                Died?.Invoke(this, arena);
            }
        }
    }
}