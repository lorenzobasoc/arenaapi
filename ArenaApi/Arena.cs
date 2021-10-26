using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArenaApi.DataAccess;
using ArenaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ArenaApi
{
    public class Arena
    {
        public List<Fighter> Fighters { get; private set; }
        public List<ArenaAction> Actions { get; private set; }
        public  bool running = false;
        private event Action<Fighter, Arena> Won;
        private static readonly AppDbContext _db = GenerateDb();

        public Arena() { }

        public async Task Start(){
            running = true;
            Actions = new List<ArenaAction>();
            Fighters = new List<Fighter>();
            var entityFighters  = await GetFightersFromDb();
             foreach (var f in entityFighters){
                Fighters.Add(FighterMapper.FromEntityToModel(f));
            }
            Fighters = Fighters.OrderBy(o => o.Speed).ToList(); 
            Won += HandleWon;
            AttachMethodsToFightersEvents(this); 
            var currentAttack = Fighters.Count - 1;
            while (running) {
                await Task.Delay(1000);
                var randomPosition = GetNextRandomFighter(currentAttack);
                var attacker = Fighters[currentAttack];
                var attacked = Fighters[randomPosition];
                attacked.Fight(attacker, this);
                if (attacked.IsDead){
                    Fighters.Remove(attacked);
                }
                if (currentAttack == 0) {
                    currentAttack = Fighters.Count - 1;
                } else {
                    currentAttack--;
                }
                if (Fighters.Count == 1) {
                    Won?.Invoke(Fighters[0], this);
                    running = false;
                }
            }
        }

        public bool Status(){
            return running;
        }

        private static async Task<List<FighterEntity>> GetFightersFromDb(){
            return await _db.Fighters.ToListAsync();
        }

        private int GetNextRandomFighter(int currentAttack){
            int randomPosition;
            while (true){
                randomPosition = new Random().Next(0, Fighters.Count); 
                if (randomPosition != currentAttack) {
                    break;
                }
            }
            return randomPosition;
        }
        
        public static AppDbContext GenerateDb() {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql("Host=localhost;Database=arena;Username=;Password=")
            .Options;
        return new AppDbContext(options);
        }

        private static void AttachMethodsToFightersEvents(Arena a){
            foreach (var f in a.Fighters){
                f.Damaged += HandleDamage;
                f.Died += HandleDie;
            }
        }
    
        private static void HandleDie(Fighter f, Arena arena){
            var message = $"{f.Name} è morto!";
            var time = DateTime.Now;
            var newAction = new ArenaAction(time, message); 
            arena.Actions.Add(newAction);
         }

        private static void HandleDamage(Fighter attacked, Fighter attacker, int damage, Arena arena){ 
            var message = $"{attacker.Name} infligge {damage} punti di danno a {attacked.Name}. A {attacked.Name} rimangono {attacked.Pv} pv. ";
            var time = DateTime.Now;
            var newAction = new ArenaAction(time, message);
            arena.Actions.Add(newAction);
        }

        private static void HandleWon(Fighter f, Arena arena){ 
            var message = $"***LA LOTTA è TERMINATA***, Il vincitore dell'arena è {f.Name}";
            var time = DateTime.Now;
            var newAction = new ArenaAction(time, message); 
            arena.Actions.Add(newAction);
        }
    }
}
