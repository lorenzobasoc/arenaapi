using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArenaApi.Interfaces
{
    public interface IFighterRepository 
    {
        Task<List<FighterDto>> Get();

        Task<FighterDto> Get(int id);

        Task<FighterDto> Create(FighterDto fighter);

        Task<FighterDto> Update(int id, FighterDto fighter);

        Task Delete(int id);
    }
}
