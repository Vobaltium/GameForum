using GameForum_Washüttl.DomainModel.Models;
using GameForum_Washüttl.DomainModel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameForum_Washüttl.Application.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameForum_Washüttl.WebApplication.Controllers
{
    public class FindPlayersController : Controller
    {
        private readonly GameForumDBContext DBContext;
        private readonly IFindPlayersService findPlayersService;
        
        public FindPlayersController(GameForumDBContext DBContext, IFindPlayersService findPlayersService)
        {
            this.DBContext = DBContext;
            this.findPlayersService = findPlayersService;
        }
        
        public async Task<IActionResult> Index(string filter, string currentFilter, string sortedBy, int pageIndex = 1)
        {
            IEnumerable<Player> context;
            ViewData["CurrentFilter"] = currentFilter ?? filter;
            ViewData["CurrentSort"] = sortedBy;
            
            ViewData["PlayerNameSortParam"] = sortedBy == "Name" ? "NameDesc" : "Name";

            Expression<Func<Player, bool>> filterPredicate = null;
            if (!string.IsNullOrEmpty(filter))
            {
                filterPredicate = t => t.p_name.ToLower().Contains(filter.ToLower())
                                       || t.players_play_games.Any(o => o.pg_message.ToLower().Contains(filter.ToLower()))
                                       || t.players_play_games.Any(o => o.pg_g_name.ToLower().Contains(filter.ToLower()))
                                       || t.answers_receiver.Any(o => o.a_p_sender.ToLower().Contains(filter.ToLower()) || o.a_message.ToLower().Contains(filter.ToLower()));
            }

            Func<IQueryable<Player>, IOrderedQueryable<Player>> sortOrderExpression = null;
            if (!string.IsNullOrEmpty(sortedBy))
            {
                sortOrderExpression = sortedBy switch
                {
                    "Name" => l => l.OrderBy(s => s.p_name),
                    _ => l => l.OrderByDescending(s => s.p_name)
                };
            }
            
            IQueryable<Player> result = findPlayersService.GetTable(filterPredicate, sortOrderExpression);
            result = result
                .Include(t => t.answers_receiver)
                .Include(t => t.answers_sender)
                .Include(t => t.players_play_games)
                .Where(o => o.players_play_games.Count() > 0);
            
            PaginatedList<Player> model = await PaginatedList<Player>.CreateAsync(result, pageIndex, 5);
            
            return View(model);
        }

        public IActionResult AddAnswer(string id)
        {
            try
            {
                string[] param = id.Split("#");
                Answer input = new Answer() { a_p_receiver = param[0], a_g_game = param[1] };

                if (!string.IsNullOrEmpty(input.a_p_receiver) && !string.IsNullOrEmpty(input.a_g_game))
                    return View(input);
            }
            catch(Exception)
            {
                return BadRequest();
            }
            return NotFound();
        }

        public async Task<IActionResult> EditAnswer(string id)
        {
            try
            {
                string[] param = id.Split("#");
                var input = await DBContext.Answers.FindAsync(param[1], param[0], param[2]);

                if (input != null)
                    return View(input);
            }
            catch(Exception)
            {
                return BadRequest();
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAnswer(string id, [Bind("a_message")] Answer input)
        {
            try
            {
                string[] param = id.Split("#");
                input.a_p_sender = param[0];
                input.a_p_receiver = param[1];
                input.a_g_game = param[2];

                if (input != null)
                {
                    await findPlayersService.UpdateAnswer(input);
                }
            }
            catch(Exception)
            {
                return View(input);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteAnswer(string id)
        {
            if (id != null)
            {
                string[] param = id.Split("#");
                var input = await DBContext.Answers.FindAsync(param[1], param[0], param[2]);
                return View(input);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ProcessAnswerDeletion(string id)
        {
            if (id != null)
                await findPlayersService.DeleteAnswer(id);
            
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAnswer(string id, [Bind("a_message,a_p_sender")] Answer input)
        {
            try
            {
                string[] param = id.Split("#");
                input.a_p_receiver = param[0];
                input.a_g_game = param[1];
                
                if (!string.IsNullOrEmpty(input.a_p_sender) && input.a_p_sender != input.a_p_receiver)
                {
                    await findPlayersService.AddAnswer(input);
                    return RedirectToAction(nameof(Index));
                }
                return View(input);
            }
            catch(Exception)
            {
                return View(input);
            }
            return View(input);
        }

        public async Task<IActionResult> AddRequest([Bind("pg_message,pg_g_name,pg_p_name")] PlayersPlayGames input)
        {
            if(ModelState.IsValid)
            {
                await findPlayersService.AddRequest(input);
                return RedirectToAction(nameof(Index));
            }
            ViewData["PgGName"] = new SelectList(DBContext.Games, "g_name", "g_name", input.pg_g_name);
            return View(input);
        }

        public async Task<IActionResult> EditRequest(string id)
        {
            try
            {
                string[] param = id.Split("#");
                var input = await DBContext.PlayersPlayGames.FindAsync(param[1], param[0]);

                if (input != null)
                    return View(input);
            }
            catch(Exception)
            {
                return BadRequest();
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRequest(string id, [Bind("pg_message")] PlayersPlayGames input)
        {
            if (!string.IsNullOrEmpty(input.pg_message))
            {
                try
                {
                    string[] param = id.Split("#");
                    input.pg_p_name = param[0];
                    input.pg_g_name = param[1];

                    await findPlayersService.UpdateRequest(input);
                }
                catch (Exception)
                {
                    return BadRequest();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(input);
        }
        
        public async Task<IActionResult> DeleteRequest(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string[] param = id.Split("#");
                var input = await DBContext.PlayersPlayGames.FirstOrDefaultAsync(m => m.pg_p_name == param[0] && m.pg_g_name == param[1]);
                return View(input);
            }

            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> ProcessRequestDeletion(string id)
        {
            if (id != null)
                await findPlayersService.DeleteRequest(id);

            return RedirectToAction(nameof(Index));
        }
        
    }
}