using System;
using System.Linq;
using System.Threading.Tasks;
using GameForum_Washüttl.DomainModel.Models;
using Xunit;
using GameForum_Washüttl.Application.Services;

namespace GameForum_Washüttl.Application.Test
{
    public class GameServiceTest
    {
        [Fact()]
        public async Task CreateTest()
        {
            // Arrange
            GamesService gamesService = new GamesService(TestSeed.Seed());
            Game game = new Game(){ g_name = "Lorenzcopter", g_genre = "Crazy Shit", g_multiplayer = true, g_imageLink = "", ReleaseDate = DateTime.Now};
            int count = gamesService.GetAllAsync().Result.Count();

            // Act
            await gamesService.AddGame(game);

            // Assert
            Assert.Equal(count + 1, gamesService.GetAllAsync().Result.Count());
        }
        
        [Fact]
        public void UpdateTest()
        {
            // Arrange
            GamesService gamesService = new GamesService(TestSeed.Seed());
            Game oldGame = gamesService.GetAllAsync().Result.ToList()[0];
            Game newGame = new Game(){g_name = oldGame.g_name, g_genre = "Crazy Shit", g_multiplayer = oldGame.g_multiplayer, g_imageLink = oldGame.g_imageLink, g_releaseDate = oldGame.g_releaseDate, ReleaseDate = oldGame.ReleaseDate};

            // Act
            gamesService.UpdateGame(newGame);

            // Assert
            Assert.Equal("Crazy Shit", gamesService.GetAllAsyncWithSearch(oldGame.g_name).Result.FirstOrDefault().g_genre);
        }
        
        [Fact]
        public void DeleteTest()
        {
            // Arrange
            GamesService gamesService = new GamesService(TestSeed.Seed());
            Game game = gamesService.GetAllAsync().Result.ToList()[0];
            int count = gamesService.GetAllAsync().Result.Count();

            // Act
            gamesService.DeleteGame(game.g_name);

            // Assert
            Assert.Equal(count - 1, gamesService.GetAllAsync().Result.Count());
        }
    }
}