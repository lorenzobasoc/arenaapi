using System;
using ArenaApi.Models;

namespace ArenaApi
{
    public class Utilities
    {
         public  static Fighter FromEntityToModel(FighterEntity fe){
             if (fe.Type == "Ork"){
                var typeOrk = typeof(Ork);
                var ork = (Ork)Activator.CreateInstance(typeOrk);
                ork.Strength = fe.Strength;
                ork.Id = fe.Id;
                ork.Pv = fe.Pv;
                ork.Speed = fe.Speed;
                ork.Name = fe.Name;
                ork.Type = fe.Type;
                return ork;
            } else if (fe.Type == "Wizard"){
                var typeWizard = typeof(Wizard);
                var wizard = (Wizard)Activator.CreateInstance(typeWizard);
                wizard.MaximumPower = fe.MaximumPower;
                wizard.Id = fe.Id;
                wizard.Pv = fe.Pv;
                wizard.Speed = fe.Speed;
                wizard.Name = fe.Name;
                wizard.Type = fe.Type;
                return wizard;
            } else if (fe.Type == "Warrior"){
                var typeWarrior = typeof(Warrior);
                var warrior = (Warrior)Activator.CreateInstance(typeWarrior);
                warrior.DamagePercentage = fe.DamagePercentage;
                warrior.Id = fe.Id;
                warrior.Pv = fe.Pv;
                warrior.Speed = fe.Speed;
                warrior.Name = fe.Name;
                warrior.Type = fe.Type;
                return warrior;
            } else {
                throw new ArgumentException("Fighter's type is not valid.");
            }
        }
   
        public static FighterDto FromEntityToDto(FighterEntity fe){
             var instance = new FighterDto {
                Id = fe.Id,
                Pv = fe.Pv,
                Speed = fe.Speed,
                Name = fe.Name,
                Type = fe.Type,
             };
             if (fe.Type == "Ork"){
                instance.Power = fe.Strength;
            } else if (fe.Type == "Wizard"){
                instance.Power = fe.MaximumPower;
            } else if (fe.Type == "Warrior"){
                instance.Power = fe.DamagePercentage;
            } else {
                throw new ArgumentException("Fighter's type is not valid.");
            }
            return instance;
        }

        public static FighterEntity FromDtoToEntity(FighterDto f){
            var instance = new FighterEntity {
                Id = f.Id,
                Pv = f.Pv,
                Speed = f.Speed,
                Name = f.Name,
                Type = f.Type,
            };
            if (f.Type == "Ork"){
                instance.Strength = f.Power;
                instance.MaximumPower = 0;
                instance.DamagePercentage = 0;
            } else if (f.Type == "Wizard"){
                instance.Strength = 0;
                instance.MaximumPower = f.Power;
                instance.DamagePercentage = 0;
            } else if (f.Type == "Warrior"){
                instance.Strength = 0;
                instance.MaximumPower = 0;
                instance.DamagePercentage = f.Power;
            } else {
                throw new ArgumentException("Fighter's type is not valid.");
            }
            return instance;
        }
   
        public static void UpdateEntity(FighterEntity old, FighterEntity newEnt){
                old.Pv = newEnt.Pv;
                old.Speed = newEnt.Speed;
                old.Name = newEnt.Name;
                old.Strength = newEnt.Strength;
                old.DamagePercentage = newEnt.DamagePercentage;
                old.MaximumPower = newEnt.MaximumPower;
                old.Type = newEnt.Type;
        }
    }
}