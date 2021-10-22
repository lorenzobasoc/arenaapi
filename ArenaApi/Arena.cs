using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArenaApi.DataAccess;
using ArenaApi.Models;
using Microsoft.EntityFrameworkCore;
using ArenaApi.Interfaces;
/*
    To do:
    - Finire il CRUD
    - Arena controller 
    - Check sugli input quando inserisci un nuovo fighter
    - Middleware che piglia le eccezioni
    - JWT
    - Logging
*/
namespace ArenaApi
{
    public class Arena
    {
        public List<Fighter> Fighters { get; private set; }
        public static List<Action> Actions { get; private set; }
        public  bool endGame = false;
        private event Action<Fighter> Won;
        private static readonly AppDbContext _db = GenerateDb();

        public Arena() { }

        public async Task Start(){
            endGame = true;
            Actions = new List<Action>();
            Fighters = new List<Fighter>();
            var entityFighters  = await GetFightersFromDb();
             foreach (var f in entityFighters){
                Fighters.Add(Utilities.FromEntityToModel(f));
            }
            Fighters = Fighters.OrderBy(o => o.Speed).ToList(); 
            Won += HandleWon;
            AttachMethodsToFightersEvents(this); 
            var currentAttack = Fighters.Count - 1;
            while (endGame) {
                Thread.Sleep(1000);
                Again:
                var randomPosition = new Random().Next(0, Fighters.Count); 
                if (randomPosition == currentAttack) {
                    goto Again;
                }
                var attacker = Fighters[currentAttack];
                var attacked = Fighters[randomPosition];
                attacked.Fight(attacker);
                if (attacked.Pv <= 0){
                    Fighters.Remove(attacked);
                }
                if (currentAttack == 0) {
                    currentAttack = Fighters.Count - 1;
                } else {
                    currentAttack--;
                }
                if (Fighters.Count == 1) {
                    Won?.Invoke(Fighters[0]);
                    endGame = false;
                }
            }
        }

        public string Status(){
            if (endGame){
                return "L'arena è in esecuzione";
            } else {
                return "L'arena non è in esecuzione";
            }
        }

        private static async Task<List<FighterEntity>> GetFightersFromDb(){
            return await _db.Fighters.ToListAsync();
        }
        
        public static AppDbContext GenerateDb() {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql("Host=localhost;Database=arena;Username=lorenzobasoc;Password=ciao")
            .Options;
        return new AppDbContext(options);
        }

        private static void AttachMethodsToFightersEvents(Arena a){
            foreach (var f in a.Fighters){
                f.Damaged += HandleDamage;
                f.Died += HandleDie;
            }
        }
    
        private static void HandleDie(Fighter f){
            var message = $"{f.Name} è morto!";
            var time = DateTime.Now;
            var newAction = new Action(time, message); 
            Actions.Add(newAction);
         }

        private static void HandleDamage(Fighter attacked, Fighter attacker, int damage){ 
            var message = $"{attacker.Name} infligge {damage} punti di danno a {attacked.Name}. A {attacked.Name} rimangono {attacked.Pv} pv. ";
            var time = DateTime.Now;
            var newAction = new Action(time, message);
            Actions.Add(newAction);
        }

        private static void HandleWon(Fighter f){ 
            var message = $"***LA LOTTA è TERMINATA***, Il vincitore dell'arena è {f.Name}";
            var time = DateTime.Now;
            var newAction = new Action(time, message); 
            Actions.Add(newAction);
        }
    }
}
