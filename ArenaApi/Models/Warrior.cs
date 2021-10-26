using System;

namespace ArenaApi.Models
{
    public class Warrior : Fighter
    {
        public int DamagePercentage { get; set; }
        private readonly int MIN_DAMAGE = 5;

        public override event Action<Fighter, Arena> Died;
        public override event Action<Fighter, Fighter, int, Arena> Damaged;

        public override int CalculateDamage(){
            var damage = Pv * DamagePercentage / 100;
            if (damage >= MIN_DAMAGE){
                return damage;
            } else {
                return MIN_DAMAGE;
            }
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