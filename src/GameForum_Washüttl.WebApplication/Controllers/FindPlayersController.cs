using GameForum_Washüttl.DomainModel.Models;
using GameForum_Washüttl.DomainModel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        
        public async Task<IActionResult> Index(string filter, string currentFilter, string sortedBy)
        {
            IEnumerable<Player> context;
            ViewData["CurrentFilter"] = currentFilter ?? filter;
            
            if (!string.IsNullOrEmpty(filter))
                context = await findPlayersService.GetAllWithSearch(filter);
            else
                context = await findPlayersService.GetAllAsync();

            if (sortedBy != null)
            {
                if (sortedBy == "Player")
                {
                    List<Player> players = context.ToList();
                    players.Sort((o,j) => o.p_name.CompareTo(j.p_name));
                    context = players.AsEnumerable();
                }
            }

            return View(context);
        }

        public IActionResult AddAnswer(string id)
        {
            try
            {
                string[] param = id.Split("#");
                Answer input = new Answer() { a_p_receiver = param[0], a_g_game = param[1] };

                if (input != null)
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
                await findPlayersService.DeleteAnswer(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAnswer(string id, [Bind("a_message,a_p_sender")] Answer input)
        {
            if (input.a_p_sender != null)
            {
                try
                {
                    string[] param = id.Split("#");
                    input.a_p_receiver = param[0];
                    input.a_g_game = param[1];

                    await findPlayersService.AddAnswer(input);
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception)
                {
                    return View(input);
                }
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
            if (id != null)
                await findPlayersService.DeleteRequest(id);

            return RedirectToAction(nameof(Index));
        }
    }
}