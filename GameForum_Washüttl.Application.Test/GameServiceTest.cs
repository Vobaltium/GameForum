using System;
using System.Linq;
using GameForum_Washüttl.DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Bogus;
using Bogus.Extensions;

namespace GameForum_Washüttl.Application.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            
        }
        
        private GameForumDBContext GetDbContext()
        {
            // Test-DB erstellen
            DbContextOptionsBuilder<GameForumDBContext> dbOptions = new DbContextOptionsBuilder<GameForumDBContext>()
                .UseSqlite($"Data Source=TestsAdministrator_Test.db");

            GameForumDBContext dbContext = new GameForumDBContext(dbOptions.Options);

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            // Test-DB seeden
            /*dbContext.CatAccountState.Add(new CatAccountState() { CatAccountStateId = new Guid("64ac80ef-fb9a-413c-b6e6-e86e457b5589"), Description = "Aktiv", Key = "A", ShortName = "Aktiv" });
            dbContext.SaveChanges();

            dbContext.CatTestState.Add(new CatTestState() { CatTestStateId = new Guid("37b02a9a-9027-4a96-aba6-6ece61319cc2"), Description = "in Vorbereitung", Key = "P", ShortName = "i.V." });
            dbContext.SaveChanges();

            dbContext.Teacher.Add(new Teacher() { T_ID = "AF", T_Account = "akyildiz", T_CatAccountStateId = new Guid("64ac80ef-fb9a-413c-b6e6-e86e457b5589"), T_Email = "akyildiz@spengergasse.at", T_Firstname = "Fatma", T_Lastname = "Akyildiz" });
            dbContext.Teacher.Add(new Teacher() { T_ID = "AGU", T_Account = "augustin", T_CatAccountStateId = new Guid("64ac80ef-fb9a-413c-b6e6-e86e457b5589"), T_Email = "augustin@spengergasse.at", T_Firstname = "Susanne", T_Lastname = "Augustin" });
            dbContext.Teacher.Add(new Teacher() { T_ID = "AH", T_Account = "auinger", T_CatAccountStateId = new Guid("64ac80ef-fb9a-413c-b6e6-e86e457b5589"), T_Email = "auinger@spengergasse.at", T_Firstname = "Harald", T_Lastname = "Auinger" });
            dbContext.Teacher.Add(new Teacher() { T_ID = "AMA", T_Account = "amcha", T_CatAccountStateId = new Guid("64ac80ef-fb9a-413c-b6e6-e86e457b5589"), T_Email = "amcha@spengergasse.at", T_Firstname = "Alfred", T_Lastname = "Amcha" });
            dbContext.SaveChanges();

            dbContext.Schoolclass.Add(new Schoolclass() { C_ID = "1AHIF", C_Department = "HIF", C_ClassTeacher = "AH" });
            dbContext.Schoolclass.Add(new Schoolclass() { C_ID = "2BHIF", C_Department = "HIF", C_ClassTeacher = "AGU" });
            dbContext.Schoolclass.Add(new Schoolclass() { C_ID = "4AHIF", C_Department = "HIF", C_ClassTeacher = "AMA" });
            dbContext.Schoolclass.Add(new Schoolclass() { C_ID = "1CHIF", C_Department = "HIF", C_ClassTeacher = "AF" });
            dbContext.Schoolclass.Add(new Schoolclass() { C_ID = "1EHIF", C_Department = "HIF", C_ClassTeacher = "AF" });
            dbContext.SaveChanges();

            dbContext.Period.Add(new Period() { P_From = new DateTime(2020, 09, 27, 08, 00, 00), P_To = new DateTime(2020, 09, 27, 08, 50, 00) });
            dbContext.Period.Add(new Period() { P_From = new DateTime(2020, 09, 27, 08, 50, 00), P_To = new DateTime(2020, 09, 27, 09, 40, 00) });
            dbContext.Period.Add(new Period() { P_From = new DateTime(2020, 09, 27, 09, 55, 00), P_To = new DateTime(2020, 09, 27, 10, 45, 00) });
            dbContext.Period.Add(new Period() { P_From = new DateTime(2020, 09, 27, 10, 45, 00), P_To = new DateTime(2020, 09, 27, 11, 35, 00) });
            dbContext.Period.Add(new Period() { P_From = new DateTime(2020, 09, 27, 11, 45, 00), P_To = new DateTime(2020, 09, 27, 12, 35, 00) });
            dbContext.Period.Add(new Period() { P_From = new DateTime(2020, 09, 27, 12, 35, 00), P_To = new DateTime(2020, 09, 27, 13, 25, 00) });
            dbContext.Period.Add(new Period() { P_From = new DateTime(2020, 09, 27, 13, 25, 00), P_To = new DateTime(2020, 09, 27, 14, 15, 00) });
            dbContext.Period.Add(new Period() { P_From = new DateTime(2020, 09, 27, 14, 25, 00), P_To = new DateTime(2020, 09, 27, 15, 15, 00) });
            dbContext.SaveChanges();

            dbContext.Lesson.Add(new Lesson() { L_Class = "2BHIF", L_Day = 3, L_Hour = 3, L_Room = "DE.03", L_Subject = "NW2", L_Teacher = "AGU", L_Untis_ID = 1 });
            dbContext.Lesson.Add(new Lesson() { L_Class = "2BHIF", L_Day = 4, L_Hour = 2, L_Room = "C3.09", L_Subject = "NW2", L_Teacher = "AGU", L_Untis_ID = 1 });
            dbContext.Lesson.Add(new Lesson() { L_Class = "2BHIF", L_Day = 5, L_Hour = 8, L_Room = "D4.03", L_Subject = "NW2", L_Teacher = "AGU", L_Untis_ID = 1 });
            dbContext.Lesson.Add(new Lesson() { L_Class = "4AHIF", L_Day = 3, L_Hour = 2, L_Room = "C3.07", L_Subject = "AM", L_Teacher = "AMA", L_Untis_ID = 1 });
            dbContext.Lesson.Add(new Lesson() { L_Class = "4AHIF", L_Day = 3, L_Hour = 1, L_Room = "C3.07", L_Subject = "AM", L_Teacher = "AMA", L_Untis_ID = 1 });
            dbContext.Lesson.Add(new Lesson() { L_Class = "4AHIF", L_Day = 2, L_Hour = 2, L_Room = "C3.09", L_Subject = "GGPB", L_Teacher = "AMA", L_Untis_ID = 1 });
            dbContext.Lesson.Add(new Lesson() { L_Class = "1CHIF", L_Day = 1, L_Hour = 6, L_Room = "C5.10", L_Subject = "BWM2", L_Teacher = "AF", L_Untis_ID = 1 });
            dbContext.Lesson.Add(new Lesson() { L_Class = "1CHIF", L_Day = 1, L_Hour = 7, L_Room = "C5.10", L_Subject = "BWM2", L_Teacher = "AF", L_Untis_ID = 1 });
            dbContext.Lesson.Add(new Lesson() { L_Class = "1EHIF", L_Day = 3, L_Hour = 5, L_Room = "C2.14", L_Subject = "BWM1", L_Teacher = "AF", L_Untis_ID = 1 });
            dbContext.Lesson.Add(new Lesson() { L_Class = "1EHIF", L_Day = 5, L_Hour = 1, L_Room = "C4.14", L_Subject = "BWM1", L_Teacher = "AF", L_Untis_ID = 1 });
            dbContext.SaveChanges();

            dbContext.Test.Add(new DomainModel.Models.Test() { Guid = new Guid("8bf2a7b9-f4d0-4b0a-aa68-698232f56398"), TE_CatTestStateId = new Guid("37b02a9a-9027-4a96-aba6-6ece61319cc2"), TE_Class = "1AHIF", TE_Date = new DateTime(2021, 3, 27), TE_Subject = "NW2", TE_Teacher = "AGU", TE_Lesson = 3 });
            dbContext.Test.Add(new DomainModel.Models.Test() { Guid = new Guid("2761cc63-3ecb-41e4-b8da-53eba840355d"), TE_CatTestStateId = new Guid("37b02a9a-9027-4a96-aba6-6ece61319cc2"), TE_Class = "1AHIF", TE_Date = new DateTime(2021, 10, 14), TE_Subject = "Dy", TE_Teacher = "AF", TE_Lesson = 1 });
            dbContext.Test.Add(new DomainModel.Models.Test() { Guid = new Guid("6bfb4cbc-c306-4a84-b012-fd4784c177a3"), TE_CatTestStateId = new Guid("37b02a9a-9027-4a96-aba6-6ece61319cc2"), TE_Class = "1AHIF", TE_Date = new DateTime(2019, 9, 9), TE_Subject = "BWM1", TE_Teacher = "AGU", TE_Lesson = 2 });
            dbContext.Test.Add(new DomainModel.Models.Test() { Guid = new Guid("4641d511-d68f-464e-9fb1-f0f5c8fbe7da"), TE_CatTestStateId = new Guid("37b02a9a-9027-4a96-aba6-6ece61319cc2"), TE_Class = "1AHIF", TE_Date = new DateTime(2020, 5, 7), TE_Subject = "BWM2", TE_Teacher = "AF", TE_Lesson = 3 });
            dbContext.Test.Add(new DomainModel.Models.Test() { Guid = new Guid("8c34b771-cf39-49a0-a0b3-433858ab1e30"), TE_CatTestStateId = new Guid("37b02a9a-9027-4a96-aba6-6ece61319cc2"), TE_Class = "1AHIF", TE_Date = new DateTime(2019, 12, 30), TE_Subject = "AMx", TE_Teacher = "AF", TE_Lesson = 3 });
            dbContext.Test.Add(new DomainModel.Models.Test() { Guid = new Guid("9909304f-4909-42ff-a6cf-d779cee32907"), TE_CatTestStateId = new Guid("37b02a9a-9027-4a96-aba6-6ece61319cc2"), TE_Class = "1AHIF", TE_Date = new DateTime(2020, 2, 29), TE_Subject = "TINF_1", TE_Teacher = "AMA", TE_Lesson = 7 });
            dbContext.Test.Add(new DomainModel.Models.Test() { Guid = new Guid("157efbf8-e91b-45d4-991b-a5f3bd1a5f45"), TE_CatTestStateId = new Guid("37b02a9a-9027-4a96-aba6-6ece61319cc2"), TE_Class = "1AHIF", TE_Date = new DateTime(2020, 5, 22), TE_Subject = "TICP4A", TE_Teacher = "AF", TE_Lesson = 6 });
            dbContext.Test.Add(new DomainModel.Models.Test() { Guid = new Guid("aedc2e6d-a9a9-4214-974e-224150199267"), TE_CatTestStateId = new Guid("37b02a9a-9027-4a96-aba6-6ece61319cc2"), TE_Class = "1AHIF", TE_Date = new DateTime(2020, 1, 5), TE_Subject = "TICP4A", TE_Teacher = "AGU", TE_Lesson = 2 });
            dbContext.Test.Add(new DomainModel.Models.Test() { Guid = new Guid("c76aacd4-3062-43ec-b3e4-ba3cadd45bbb"), TE_CatTestStateId = new Guid("37b02a9a-9027-4a96-aba6-6ece61319cc2"), TE_Class = "1AHIF", TE_Date = new DateTime(2020, 4, 2), TE_Subject = "Dx", TE_Teacher = "AF", TE_Lesson = 5 });
            dbContext.Test.Add(new DomainModel.Models.Test() { Guid = new Guid("e7fc29f4-b12c-4225-896d-26bda231d359"), TE_CatTestStateId = new Guid("37b02a9a-9027-4a96-aba6-6ece61319cc2"), TE_Class = "1AHIF", TE_Date = new DateTime(2020, 6, 21), TE_Subject = "POS1z", TE_Teacher = "AF", TE_Lesson = 4 });
            dbContext.SaveChanges();*/

            return dbContext;
        }

        [Fact()]
        public async void Create_Success()
        {
            // Arrange
            /*ExamService examService = new ExamService(GetDbContext());
            DomainModel.Models.Test newEntity = new DomainModel.Models.Test() { Guid = new Guid("272bb2eb-3102-4459-98e8-7d3ecbb14d0c"), TE_CatTestStateId = new Guid("37b02a9a-9027-4a96-aba6-6ece61319cc2"), TE_Class = "1AHIF", TE_Date = new DateTime(2021, 4, 28), TE_Subject = "POS1z", TE_Teacher = "AF", TE_Lesson = 4 };

            // Act
            await examService.CreateAsync(newEntity);

            // Assert
            Assert.Equal(11, examService.GetTable().Count());*/
        }
        
        public void Seed()
        {
            DbContextOptionsBuilder<GameForumDBContext> dbOptions = new DbContextOptionsBuilder<GameForumDBContext>().UseSqlite($"Data Source=TestsAdministrator_Test.db");

            GameForumDBContext dbContext = new GameForumDBContext(dbOptions.Options);

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            Randomizer.Seed = new Random(2112); 

            var games = new Faker<Game>().Rules((f, l) =>
            {
                l.g_name = f.Name.FirstName();
                l.g_imageLink = f.Image.DataUri(90, 60);
                l.g_releaseDate = f.Date.Past().Year;
            })
            .Generate(100)
            // Die Id Werte können kollidieren. Deswegen ein DISTINCT.
            .GroupBy(l => l.g_name).Select(g => g.First())
            .ToList();
            
            dbContext.Games.AddRange(games);
            //_db.GetCollection<Lehrer>(nameof(Lehrer)).InsertMany(lehrer);

            //var abteilungen = new string[] { "HIF", "AIF", "BIF" };
            var players = new Faker<Player>().Rules((f, k) =>
            {
                k.p_name = f.Name.FirstName();
                /*k.Id = f.Random.Int(1, 5) + f.Random.String2(1, "ABCD") + f.Random.ListItem(abteilungen);
                k.KvId = f.Random.ListItem(lehrer).Id;*/
            })
            .Generate(10)
            .GroupBy(k => k.p_name).Select(g => g.First())
            .ToList();
            //_db.GetCollection<Klasse>(nameof(Klasse)).InsertMany(klassen);
            dbContext.Players.AddRange(players);

            /*var schueler = new Faker<Schueler>().Rules((f, s) =>
            {
                s.Vorname = f.Name.FirstName();
                s.Zuname = f.Name.LastName();
                s.KlasseId = f.Random.ListItem(klassen).Id;
                s.Gebdat = f.Date.Between(
                    new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    new DateTime(2003, 12, 31, 0, 0, 0, DateTimeKind.Utc)).Date.OrNull(f, 0.2f);
            })
            .Generate(200);
            _db.GetCollection<Schueler>(nameof(Schueler)).InsertMany(schueler);*/
        }
    }
}