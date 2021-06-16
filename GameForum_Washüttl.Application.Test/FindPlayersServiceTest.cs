using System;
using System.Linq;
using System.Reflection.Metadata;
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

        [Fact()]
        public async Task CreateAnswerTest()
        {
            // Arrange
            GameForumDBContext context = TestSeed.Seed();
            FindPlayersService findPlayersService = new FindPlayersService(context);
            Answer answer = new Answer(){ a_message = "Hier ist eine Message", a_g_game = context.Games.ToList()[0].g_name, a_p_sender = "Vobaltium", a_p_receiver = context.PlayersPlayGames.ToList()[0].pg_p_name};
            int count = context.Answers.Count();

            // Act
            await findPlayersService.AddAnswer(answer);

            // Assert
            Assert.Equal(count + 1, context.Answers.Count());
        }
        
        [Fact()]
        public async Task DeleteRequestTest()
        {
            // Arrange
            GameForumDBContext context = TestSeed.Seed();
            FindPlayersService findPlayersService = new FindPlayersService(context);
            PlayersPlayGames playersPlayGames = context.PlayersPlayGames.ToList()[0];
            int count = context.PlayersPlayGames.Count();

            // Act
            await findPlayersService.DeleteRequest(playersPlayGames.pg_p_name+"#"+playersPlayGames.pg_g_name);

            // Assert
            Assert.Equal(count - 1, context.PlayersPlayGames.Count());
        }
        
        [Fact()]
        public async Task DeleteAnswerTest()
        {
            // Arrange
            GameForumDBContext context = TestSeed.Seed();
            FindPlayersService findPlayersService = new FindPlayersService(context);
            Answer playersPlayGames = context.Answers.ToList()[0];
            int count = context.Answers.Count();

            // Act
            await findPlayersService.DeleteAnswer(playersPlayGames.a_p_sender+"#"+playersPlayGames.a_p_receiver+"#"+playersPlayGames.a_g_game);

            // Assert
            Assert.Equal(count - 1, context.Answers.Count());
        }
        
        [Fact()]
        public async Task UpdateRequestTest()
        {
            // Arrange
            GameForumDBContext context = TestSeed.Seed();
            FindPlayersService findPlayersService = new FindPlayersService(context);
            PlayersPlayGames playersPlayGames = context.PlayersPlayGames.ToList()[0];
            PlayersPlayGames toUpdate = new PlayersPlayGames() { pg_g_name = playersPlayGames.pg_g_name, pg_message = "Test Update", pg_p_name = playersPlayGames.pg_p_name};
            int count = context.PlayersPlayGames.Count();

            // Act
            await findPlayersService.UpdateRequest(toUpdate);

            // Assert
            Assert.Equal("Test Update", context.PlayersPlayGames.ToList()[0].pg_message);
        }
        
        [Fact()]
        public async Task UpdateAnswerTest()
        {
            // Arrange
            GameForumDBContext context = TestSeed.Seed();
            FindPlayersService findPlayersService = new FindPlayersService(context);
            Answer answer = context.Answers.ToList()[0];
            Answer toUpdate = new Answer() { a_g_game = answer.a_g_game, a_message = "Test Update", a_p_receiver = answer.a_p_receiver, a_p_sender = answer.a_p_sender};
            int count = context.PlayersPlayGames.Count();

            // Act
            await findPlayersService.UpdateAnswer(toUpdate);

            // Assert
            Assert.Equal("Test Update", context.Answers.ToList()[0].a_message);
        }
        
        [Fact()]
        public void GetAllTest()
        {
            // Arrange
            GameForumDBContext context = TestSeed.Seed();
            FindPlayersService findPlayersService = new FindPlayersService(context);

            // Act
            int count = context.Players.Count();
            int countGetAll = findPlayersService.GetAllAsync().Result.Count();
            
            // Assert
            Assert.Equal(count, countGetAll);
        }
        
        [Fact()]
        public void GetAllWithSearchTest()
        {
            // Arrange
            GameForumDBContext context = TestSeed.Seed();
            FindPlayersService findPlayersService = new FindPlayersService(context);
            Player player = context.Players.ToList()[0];

            // Act
            int count = findPlayersService.GetAllWithSearch(player.p_name).Result.Count();
            
            // Assert
            Assert.Equal(1, count);
        }
    }
}