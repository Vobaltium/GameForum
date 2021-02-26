using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GameForum_Washüttl.DomainModel.Models
{
    public partial class GameForumDBContext : DbContext
    {
        public GameForumDBContext(DbContextOptions<GameForumDBContext> options) : base(options){}

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<PlayersPlayGames> PlayersPlayGames { get; set; }
            
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.HasKey(e => new { e.a_p_receiver, e.a_p_sender, e.a_g_game });

                entity.HasOne(d => d.receiver_object)
                    .WithMany(p => p.answers_receiver)
                    .HasForeignKey(d => d.a_p_receiver);

                entity.HasOne(d => d.sender_object)
                    .WithMany(p => p.answers_sender)
                    .HasForeignKey(d => d.a_p_sender);

            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.Ignore(e => e.ReleaseDate);
            });


            modelBuilder.Entity<PlayersPlayGames>(entity =>
            {
                entity.HasKey(e => new { e.pg_g_name, e.pg_p_name });

                entity.HasOne(d => d.game_object)
                    .WithMany(p => p.players_play_games)
                    .HasForeignKey(d => d.pg_g_name)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.player_object)
                    .WithMany(p => p.players_play_games)
                    .HasForeignKey(d => d.pg_p_name)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
