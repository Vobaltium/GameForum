using GameForum_Washüttl.DomainModel.Interfaces;
using GameForum_Washüttl.DomainModel.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameForum_Washüttl.Application.Services;
using Microsoft.EntityFrameworkCore;

namespace GameForum_Washüttl.WebApplication.Controllers
{
    public class GamesController : Controller
    {
        private readonly GameForumDBContext DBContext;
        private readonly IGamesService gamesService;
        private readonly IReadOnlyService<Game> readOnlyGameService;

        public GamesController(GameForumDBContext DBContext, IGamesService gamesService, IReadOnlyService<Game> readOnlyGameService)
        {
            this.DBContext = DBContext;
            this.gamesService = gamesService;
            this.readOnlyGameService = readOnlyGameService;
        }

        public async Task<IActionResult> Index(string filter, string currentFilter, string sortedBy, int pageIndex = 1)
        {
            IEnumerable<Game> context;
            ViewData["CurrentFilter"] = currentFilter ?? filter;
            
            if (!string.IsNullOrEmpty(filter))
                context = await gamesService.GetAllAsyncWithSearch(filter);
            else
                context = await gamesService.GetAllAsync();
            
            List<Game> games = context.ToList();
            if (sortedBy != null)
            {
                if (sortedBy == "Name")
                    games.Sort((o,j) => o.g_name.CompareTo(j.g_name));
                else if (sortedBy == "Genre")
                    games.Sort((o,j) => o.g_genre.CompareTo(j.g_genre));
                else if (sortedBy == "ReleaseDate")
                    games.Sort((o,j) => o.g_releaseDate.CompareTo(j.g_releaseDate));
            }

            IQueryable<Game> model2 = readOnlyGameService.GetTable(null);
            
            PagenatedList<Game> model = await PagenatedList<Game>.CreateAsync(model2, pageIndex, 10);
            
            return View(model);
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
