using GameForum_Washüttl.DomainModel.Interfaces;
using GameForum_Washüttl.DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameForum_Washüttl.DomainModel.Exceptions;
using System;

namespace GameForum_Washüttl.Application.Services
{
    public class FindPlayersService : IFindPlayersService
    {
        private readonly GameForumDBContext DBContext;

        public FindPlayersService(GameForumDBContext DBContext)
        {
            this.DBContext = DBContext;
        }

        public async Task AddAnswer(Answer input)
        {
            if (!DBContext.Players.Any(o => o.p_name == input.a_p_sender))
                DBContext.Players.Add(new Player() { p_name = input.a_p_sender });
            try
            {
                DBContext.Answers.Add(input);
                await DBContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new ServiceException("Die Methode AddAnswer ist fehlgeschlagen!", e);
            }
            catch (InvalidOperationException e)
            {
                throw new ServiceException("Die Methode AddAnswer ist fehlgeschlagen!", e);
            }
        }

        public async Task AddRequest(PlayersPlayGames input)
        {
            if (!DBContext.Players.Any(o => o.p_name == input.pg_p_name))
                DBContext.Players.Add(new Player() { p_name = input.pg_p_name });
            try
            {
                DBContext.PlayersPlayGames.Add(input);
                await DBContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new ServiceException("Die Methode AddRequest ist fehlgeschlagen!", e);
            }
            catch (InvalidOperationException e)
            {
                throw new ServiceException("Die Methode AddRequest ist fehlgeschlagen!", e);
            }
        }

        public async Task DeleteRequest(string id)
        {
            string[] param = id.Split("#");
            var input = await DBContext.PlayersPlayGames.FirstOrDefaultAsync(m => m.pg_p_name == param[0] && m.pg_g_name == param[1]);

            if (input != null)
            {
                try
                {
                    DBContext.PlayersPlayGames.Remove(input);
                    DBContext.Answers.RemoveRange(DBContext.Answers.Where(o => o.a_p_receiver == param[0] && o.a_g_game == param[1]));
                    await DBContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!DBContext.PlayersPlayGames.Any(m => m.pg_p_name == param[0] && m.pg_g_name == param[1]))
                        throw new ServiceException("Datensatz konnte nicht gefunden werden!");
                    else
                        throw new ServiceException("Die Methode DeleteRequest ist fehlgeschlagen!", e);
                }
                catch (InvalidOperationException e)
                {
                    throw new ServiceException("Die Methode DeleteRequest ist fehlgeschlagen!", e);
                }
            }
            else
                throw new ServiceException("Datensatz konnte nicht gefunden werden!");
        }

        public async Task DeleteAnswer(string id)
        {
            string[] param = id.Split("#");
            var input = await DBContext.Answers.FindAsync(param[1], param[0], param[2]);

            if (input != null)
            {
                try
                {
                    DBContext.Answers.Remove(input);
                    await DBContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (await DBContext.Answers.FindAsync(param[1], param[0], param[2]) == null)
                        throw new ServiceException("Datensatz konnte nicht gefunden werden!");
                    else
                        throw new ServiceException("Die Methode DeleteAnswer ist fehlgeschlagen!", e);
                }
                catch (InvalidOperationException e)
                {
                    throw new ServiceException("Die Methode DeleteAnswer ist fehlgeschlagen!", e);
                }
            }
            else
                throw new ServiceException("Datensatz konnte nicht gefunden werden!");
        }

        public async Task UpdateAnswer(Answer input)
        {
            var toChange = await DBContext.Answers.FirstOrDefaultAsync(m => m.a_g_game == input.a_g_game && m.a_p_sender == input.a_p_sender && m.a_p_receiver == input.a_p_receiver);

            if (toChange != null)
            {
                try
                {
                    toChange.a_message = input.a_message;
                    await DBContext.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException e)
                {
                    if (!DBContext.Answers.Any(m => m.a_g_game == input.a_g_game && m.a_p_sender == input.a_p_sender && m.a_p_receiver == input.a_p_receiver))
                        throw new ServiceException("Datensatz konnte nicht gefunden werden!");
                    else
                        throw new ServiceException("Die Methode UpdateAnswer ist fehlgeschlagen!", e);
                }
                catch(InvalidOperationException e)
                {
                    throw new ServiceException("Die Methode UpdateAnswer ist fehlgeschlagen!", e);
                }
            }
            else
                throw new ServiceException("Datensatz konnte nicht gefunden werden!");
        }

        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            return await DBContext.Players.Where(o => DBContext.PlayersPlayGames.Where(l => l.pg_p_name == o.p_name).Count() > 0)
                                            .Include(o => o.answers_receiver)
                                            .Include(o => o.answers_sender)
                                            .Include(o => o.players_play_games)
                                            .ToListAsync();
        }

        public async Task<IEnumerable<Player>> GetAllWithSearch(string searchString)
        {
            IEnumerable<Player> players = await GetAllAsync();
            return players.Where(o => o.p_name.ToLower().Contains(searchString.ToLower()) || 
                                      o.players_play_games.Where(o => o.pg_g_name.ToLower().Contains(searchString.ToLower())).Count() > 0);
        }

        public async Task UpdateRequest(PlayersPlayGames input)
        {
            var toChange = await DBContext.PlayersPlayGames.FirstOrDefaultAsync(m => m.pg_p_name == input.pg_p_name && m.pg_g_name == input.pg_g_name);

            if (toChange != null)
            {
                try
                {
                    toChange.pg_message = input.pg_message;
                    await DBContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!DBContext.PlayersPlayGames.Any(m => m.pg_p_name == input.pg_p_name && m.pg_g_name == input.pg_g_name))
                        throw new ServiceException("Datensatz konnte nicht gefunden werden!");
                    else
                        throw new ServiceException("Die Methode UpdateRequest ist fehlgeschlagen!", e);
                }
                catch(InvalidOperationException e)
                {
                    throw new ServiceException("Die Methode UpdateRequest ist fehlgeschlagen!", e);
                }
            }
            else
                throw new ServiceException("Datensatz konnte nicht gefunden werden!");
        }
    }
}
