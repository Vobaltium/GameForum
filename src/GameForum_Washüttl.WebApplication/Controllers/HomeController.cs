using GameForum_Washüttl.DomainModel.Models;
using GameForum_Washüttl.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;

namespace GameForum_Washüttl.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly GameForumDBContext DBContext;

        public HomeController(ILogger<HomeController> logger, GameForumDBContext DBContext)
        {
            this.logger = logger;
            this.DBContext = DBContext;
            DBContext.Database.EnsureCreated();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
