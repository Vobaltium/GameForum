using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameForum_Washüttl.DomainModel.Exceptions;
using GameForum_Washüttl.DomainModel.Interfaces;
using GameForum_Washüttl.DomainModel.Models;
using Microsoft.EntityFrameworkCore;

namespace GameForum_Washüttl.Application.Services
{
    public class GamesService : IGamesService
    {
        private readonly GameForumDBContext DBContext;

        public GamesService(GameForumDBContext DBContext)
        {
            this.DBContext = DBContext;
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await DBContext.Games.ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetAllAsyncWithSearch(string searchString)
        {
            IEnumerable<Game> games = await GetAllAsync();
            return games.Where(o => o.g_name.ToLower().Contains(searchString.ToLower()) ||
                                    o.g_genre.ToLower().Contains(searchString.ToLower()));
        }

        public async Task AddGame(Game input)
        {
            try
            {
                DBContext.Games.Add(input);
                await DBContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new ServiceException("Die Methode AddGame ist fehlgeschlagen!", e);
            }
            catch (InvalidOperationException e)
            {
                throw new ServiceException("Die Methode AddGame ist fehlgeschlagen!", e);
            }
        }

        public async Task DeleteGame(string id)
        {
            Game input = await DBContext.Games.FindAsync(id);
            
            if(input != null)
            {
                try
                { 
                    DBContext.Games.Remove(input);
                    await DBContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (await DBContext.Games.FindAsync(id) == null)
                        throw new ServiceException("Datensatz konnte nicht gefunden werden!");
                    else
                        throw new ServiceException("Die Methode DeleteGame ist fehlgeschlagen!", e);
                }
                catch (InvalidOperationException e)
                {
                    throw new ServiceException("Die Methode DeleteGame ist fehlgeschlagen!", e);
                }
            }
            else
                throw new ServiceException("Datensatz konnte nicht gefunden werden!");
        }

        public async Task UpdateGame(Game input)
        {
            var toChange = await DBContext.Games.FindAsync(input.g_name);
            
            if (toChange != null)
            {
                try
                {
                    toChange.ReleaseDate = input.ReleaseDate;
                    toChange.g_genre = input.g_genre;
                    toChange.g_imageLink = input.g_imageLink;
                    toChange.g_multiplayer = input.g_multiplayer;

                    await DBContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (await DBContext.Games.FindAsync(input.g_name) == null)
                        throw new ServiceException("Datensatz konnte nicht gefunden werden!");
                    else
                        throw new ServiceException("Die Methode UpdateGame ist fehlgeschlagen!", e);
                }
                catch (InvalidOperationException e)
                {
                    throw new ServiceException("Die Methode UpdateGame ist fehlgeschlagen!", e);
                }
            }
            else
                throw new ServiceException("Datensatz konnte nicht gefunden werden!");
        }
        
        public IQueryable<Game> GetTable(
            Expression<Func<Game, bool>> filterExpression = null, 
            Func<IQueryable<Game>, IOrderedQueryable<Game>> orderBy = null)
        {
            IQueryable<Game> result = DBContext
                .Set<Game>();

            if (filterExpression != null)
            {
                result = result.Where(filterExpression);
            }
            if (orderBy != null)
            {
                result = orderBy(result);
            }

            return result;
        }
    }
}
