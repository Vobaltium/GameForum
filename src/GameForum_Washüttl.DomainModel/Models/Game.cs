using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameForum_Washüttl.DomainModel.Models
{
    public partial class Game
    {
        public Game()
        {
            players_play_games = new HashSet<PlayersPlayGames>();
        }
        [Key]
        [StringLength(40)]
        [Required]
        public string g_name { get; set; }
        public long g_releaseDate { get; set; }
        [StringLength(20)]
        [Required]
        public string g_genre { get; set; }
        [Required]
        public bool g_multiplayer { get; set; }
        [StringLength(100)]
        public string g_imageLink { get; set; }

        private DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public DateTime ReleaseDate
        {
            get
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(g_releaseDate).ToLocalTime();
            }
            set
            {
                g_releaseDate = (long)value.ToLocalTime().Subtract(start).TotalMilliseconds;
            }
        }

        public virtual ICollection<PlayersPlayGames> players_play_games { get; set; }
    }
}
