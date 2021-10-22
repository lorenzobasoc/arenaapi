using System;
using System.Text.Json.Serialization;

namespace ArenaApi.Models
{
     public class Ork : Fighter
    {
        public int Strength { get; set; }

        public override event Action<Fighter> Died;
        public override event Action<Fighter, Fighter, int> Damaged;

        public override int CalculateDamage(){
            return Strength;
        }

        public override void Fight(Fighter attacker){
            var damage = attacker.CalculateDamage(); 
            if (damage < Pv){
                Pv -= damage;
                Damaged?.Invoke(this, attacker, damage);
            } else {
                Pv = 0;
                Damaged?.Invoke(this, attacker, damage);
                Died?.Invoke(this);
            }
        }

    }
}