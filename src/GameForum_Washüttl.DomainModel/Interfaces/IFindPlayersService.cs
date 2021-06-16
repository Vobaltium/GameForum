using System;
using GameForum_Washüttl.DomainModel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GameForum_Washüttl.DomainModel.Interfaces
{
    public interface IFindPlayersService
    {
        Task<IEnumerable<Player>> GetAllAsync();
        Task<IEnumerable<Player>> GetAllWithSearch(string searchString);

        Task UpdateRequest(PlayersPlayGames entity);
        Task AddRequest(PlayersPlayGames input);
        Task DeleteRequest(string id);

        Task AddAnswer(Answer input);
        Task UpdateAnswer(Answer input);
        Task DeleteAnswer(string id);
        
        IQueryable<Player> GetTable(Expression<Func<Player, bool>> filterExpression,
            Func<IQueryable<Player>, IOrderedQueryable<Player>> orderBy = null);
    }
}
