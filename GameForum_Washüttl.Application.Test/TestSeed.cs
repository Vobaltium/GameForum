using System;
using System.Linq;
using GameForum_Washüttl.DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using Bogus;

namespace GameForum_Washüttl.Application.Test
{
    public static class TestSeed
    {
        public static GameForumDBContext Seed()
        {
            DbContextOptionsBuilder<GameForumDBContext> dbOptions = new DbContextOptionsBuilder<GameForumDBContext>().UseSqlite($"Data Source=TestsAdministrator_Test.sqlite");

            GameForumDBContext dbContext = new GameForumDBContext(dbOptions.Options);

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            Randomizer.Seed = new Random(2112); 

            var games = new Faker<Game>().Rules((f, l) =>
            {
                l.g_name = f.Name.FirstName();
                l.g_imageLink = f.Image.DataUri(90, 60);
                l.g_genre = f.Lorem.Word();
                l.g_releaseDate = f.Date.Past().Year;
            })
            .Generate(100)
            .GroupBy(l => l.g_name).Select(g => g.First()).ToList();
            
            dbContext.Games.AddRange(games);

            var players = new Faker<Player>().Rules((f, k) =>
            {
                k.p_name = f.Name.FirstName();
            })
            .Generate(10)
            .GroupBy(k => k.p_name).Select(g => g.First())
            .ToList();
            
            dbContext.Players.AddRange(players);
            
            var playersPlayGames = new Faker<PlayersPlayGames>().Rules((f, l) =>
                {
                    l.pg_p_name = f.Random.ListItem(players).p_name;
                    l.pg_g_name = f.Random.ListItem(games).g_name;
                    l.pg_message = f.Lorem.Sentence();
                })
                .Generate(50)
                .GroupBy(l => new {l.pg_g_name, l.pg_p_name}).Select(g => g.First()).ToList();

            dbContext.PlayersPlayGames.AddRange(playersPlayGames);
            
            var answers = new Faker<Answer>().Rules((f, l) =>
                {
                    PlayersPlayGames toAnswer = f.Random.ListItem(playersPlayGames);
                    l.a_message = f.Lorem.Sentence();
                    l.a_g_game = toAnswer.pg_g_name;
                    l.a_p_sender = toAnswer.pg_p_name;
                    l.a_p_receiver = f.Random.ListItem(players).p_name;
                })
                .Generate(50)
                .GroupBy(l => new {l.a_g_game, l.a_p_sender, l.a_p_receiver}).Select(g => g.First()).ToList();
            
            dbContext.Answers.AddRange(answers);
            dbContext.SaveChanges();
            
            return dbContext;
        }    
    }
}