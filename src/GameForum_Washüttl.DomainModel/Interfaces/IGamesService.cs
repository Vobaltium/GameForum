using System;
using GameForum_Washüttl.DomainModel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        IQueryable<Game> GetTable(Expression<Func<Game, bool>> filterExpression,
            Func<IQueryable<Game>, IOrderedQueryable<Game>> orderBy = null);
    }
}
