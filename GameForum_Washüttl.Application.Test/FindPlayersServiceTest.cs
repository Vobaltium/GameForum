using System;
using System.Linq;
using System.Threading.Tasks;
using GameForum_Washüttl.DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Bogus;
using GameForum_Washüttl.Application.Services;
using Xunit.Abstractions;

namespace GameForum_Washüttl.Application.Test
{
    public class FindPlayersServiceTest
    {
        [Fact()]
        public async Task CreateRequestTest()
        {
            // Arrange
            GameForumDBContext context = TestSeed.Seed();
            FindPlayersService findPlayersService = new FindPlayersService(context);
            PlayersPlayGames playersPlayGames = new PlayersPlayGames(){ pg_message = "Hier ist eine Message", pg_g_name = context.Games.ToList()[0].g_name, pg_p_name = "Vobaltium"};
            int count = context.PlayersPlayGames.Count();

            // Act
            await findPlayersService.AddRequest(playersPlayGames);

            // Assert
            Assert.Equal(count + 1, context.PlayersPlayGames.Count());
        }
    }
}