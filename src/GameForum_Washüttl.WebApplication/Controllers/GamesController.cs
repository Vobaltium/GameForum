using GameForum_Washüttl.DomainModel.Interfaces;
using GameForum_Washüttl.DomainModel.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameForum_Washüttl.Application.Services;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> Index(string filter, string currentFilter, string sortedBy, int pageIndex = 1)
        {
            ViewData["CurrentFilter"] = currentFilter ?? filter;
            ViewData["CurrentSort"] = sortedBy;
            
            ViewData["NameSortParam"] = sortedBy == "Name" ? "NameDesc" : "Name";
            ViewData["ReleaseDateSortParam"] = sortedBy == "ReleaseDate" ? "ReleaseDateDesc" : "ReleaseDate";
            ViewData["GenreSortParam"] = sortedBy == "Genre" ? "GenreDesc" : "Genre";
            
            Expression<Func<Game, bool>> filterPredicate = null;
            if (!string.IsNullOrEmpty(filter))
            {
                filterPredicate = t => t.g_name.ToLower().Contains(filter.ToLower())
                                       || t.g_genre.ToLower().Contains(filter.ToLower());
            }

            Func<IQueryable<Game>, IOrderedQueryable<Game>> sortOrderExpression = null;
            if (!string.IsNullOrEmpty(sortedBy))
            {
                sortOrderExpression = sortedBy switch
                {
                    "Name" => l => l.OrderBy(s => s.g_name),
                    "Genre" => l => l.OrderBy(s => s.g_genre),
                    "ReleaseDate" => l => l.OrderBy(s => s.g_releaseDate),
                    "NameDesc" => l => l.OrderByDescending(s => s.g_name),
                    "GenreDesc" => l => l.OrderByDescending(s => s.g_genre),
                    _ => l => l.OrderByDescending(s => s.g_releaseDate)
                };
            }
            
            IQueryable<Game> result = gamesService.GetTable(filterPredicate, sortOrderExpression);
            result = result.Include(t => t.players_play_games);
            
            PaginatedList<Game> model = await PaginatedList<Game>.CreateAsync(result, pageIndex, 5);
            
            return View(model);
        }

        public async Task<IActionResult> Create([Bind("g_name,g_genre,g_multiplayer,ReleaseDate,g_imageLink")] Game input)
        {
            if (ModelState.IsValid && await DBContext.Games.FindAsync(input.g_name) == null && DateTime.Compare(input.ReleaseDate, DateTime.Now) <= 0)
            {
                await gamesService.AddGame(input);
                return RedirectToAction(nameof(Index));
            }
            return View(input);
        }

        public async Task<IActionResult> DeleteGame(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var erg = await gamesService.GetAllAsyncWithSearch(id);
                return View(erg.FirstOrDefault());
            }

            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessGameDeletion(string id)
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
                if (DateTime.Compare(input.ReleaseDate, DateTime.Now) > 0)
                {
                    input.g_name = id;
                    return View(input);
                }

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
