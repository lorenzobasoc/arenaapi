using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArenaApi.Interfaces;

namespace ArenaApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class FightersController : ControllerBase {
        private readonly IFighterRepository _fighterRepo;

        public FightersController(IFighterRepository fighterRepo) {
            _fighterRepo = fighterRepo;
        }

        [HttpGet]
        public async Task<List<FighterDto>> GetFighters() {
            return await _fighterRepo.Get();
        }

        [HttpGet("{id}")]
        public async Task<FighterDto> GetFighter(int id) {
            return await _fighterRepo.Get(id);             
        }

        [HttpPost]
        public async Task<FighterDto> PostFighter(FighterDto fighter) {
            var newFighter =  await _fighterRepo.Create(fighter);
            return newFighter;
        }

        [HttpPut("{id}")]
        public async Task<FighterDto> UpdateFighter(int id, FighterDto fighter) {
            await _fighterRepo.Update(id, fighter);
            return fighter;
        }

        [HttpDelete("{id}")]
        public async Task<FighterDto> DeleteFighter(int id) {
            var fighterToDelete = await _fighterRepo.Get(id);
            await _fighterRepo.Delete(fighterToDelete.Id);
            return fighterToDelete;
        }
    }
}
