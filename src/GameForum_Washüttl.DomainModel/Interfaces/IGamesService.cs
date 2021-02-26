using GameForum_Washüttl.DomainModel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameForum_Washüttl.DomainModel.Interfaces
{
    public interface IGamesService
    {
        Task<IEnumerable<Game>> GetAllAsync();
        Task<IEnumerable<Game>> GetAllAsyncWithSearch(string searchString);

        Task AddGame(Game input);
        Task DeleteGame(string id);
        Task UpdateGame(Game input);
    }
}
