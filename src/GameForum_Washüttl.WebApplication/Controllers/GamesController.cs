using GameForum_Washüttl.DomainModel.Interfaces;
using GameForum_Washüttl.DomainModel.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameForum_Washüttl.WebApplication.Controllers
{
    public class GamesController : Controller
    {
        private readonly GameForumDBContext DBContext;
        private readonly IGamesService gamesService;

        public GamesController(GameForumDBContext DBContext, IGamesService gamesService)
        {
            this.DBContext = DBContext;
            this.gamesService = gamesService;
        }

        public async Task<IActionResult> Index(string id)
        {
            IEnumerable<Game> context;
            if (!string.IsNullOrEmpty(id))
                context = await gamesService.GetAllAsyncWithSearch(id);
            else
                context = await gamesService.GetAllAsync();

            return View(context);
        }

        public async Task<IActionResult> Create([Bind("g_name,g_genre,g_multiplayer,ReleaseDate,g_imageLink")] Game input)
        {
            if (ModelState.IsValid && await DBContext.Games.FindAsync(input.g_name) == null)
            {
                await gamesService.AddGame(input);
                return RedirectToAction(nameof(Index));
            }
            return View(input);
        }

        public async Task<IActionResult> DeleteGame(string id)
        {
            if(id != null)
                await gamesService.DeleteGame(id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditGame(string id)
        {
            if (id != null)
            {
                Game input = await DBContext.Games.FindAsync(id);

                if (input != null)
                    return View(input);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGame(string id, [Bind("g_genre,g_multiplayer,ReleaseDate,g_imageLink")] Game input)
        {
            if (id != null)
            {
                try
                {
                    input.g_name = id;
                    await gamesService.UpdateGame(input);
                }
                catch(Exception)
                {
                    return View(input);
                }
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
