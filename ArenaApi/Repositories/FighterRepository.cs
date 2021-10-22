using System.Collections.Generic;
using System.Threading.Tasks;
using ArenaApi.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using ArenaApi.Interfaces;
using System.Linq;

namespace ArenaApi.Repositories
{
    public class FighterRepository : IFighterRepository
    {
        private readonly AppDbContext _context;

        public FighterRepository(AppDbContext ctx) {
            _context = ctx;
        }

        public async Task<List<FighterDto>> Get() {
            var listEntityFighters = await _context.Fighters.ToListAsync();
            var listDtoFighters = new List<FighterDto>();
            foreach (var f in listEntityFighters) {
                var fm = Utilities.FromEntityToDto(f);
                listDtoFighters.Add(fm);
            }
            return listDtoFighters;
        }

        public async Task<FighterDto> Get(int id) {
            var fighter = await _context.Fighters.FindAsync(id);
            if (fighter == null){
                throw new NullReferenceException();
            }
            return Utilities.FromEntityToDto(await _context.Fighters.FindAsync(id));
        }

        public async Task<FighterDto> Create(FighterDto fighter) {
            if (!NameValidator(fighter)){
                var enitityFighter = Utilities.FromDtoToEntity(fighter);
                _context.Fighters.Add(enitityFighter);
                await _context.SaveChangesAsync();
                return fighter;
            } else {
                throw new InvalidOperationException("Il nome inserito è già stato usato.");
            }
        }

        public async Task Delete(int id) {
            var fighterToDelete = await _context.Fighters.FindAsync(id);
            if (fighterToDelete == null){
                throw new NullReferenceException();
            } else {
                _context.Fighters.Remove(fighterToDelete);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<FighterDto> Update(int id, FighterDto fighter) {
            var fighterToUpdate = await _context.Fighters.FindAsync(id);
            if (fighterToUpdate != null){
                var newEntityFighter = Utilities.FromDtoToEntity(fighter);
                Utilities.UpdateEntity(fighterToUpdate, newEntityFighter);
                await _context.SaveChangesAsync();
            } else {
                throw new KeyNotFoundException("Questo ID non esiste.");
            }
            return fighter;
        }

        private bool NameValidator(FighterDto fighter){
            var fighters =  _context.Fighters.ToList();
            return fighters
                    .Select(f => f.Name)
                    .Any(n => n == fighter.Name);
        }
    }
}
