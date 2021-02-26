using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameForum_Washüttl.DomainModel.Models
{
    public partial class Player
    {
        public Player()
        {
            answers_receiver = new HashSet<Answer>();
            players_play_games = new HashSet<PlayersPlayGames>();
            answers_sender = new HashSet<Answer>();
        }
        [Key]
        [Required]
        [StringLength(20)]
        public string p_name { get; set; }

        public virtual ICollection<Answer> answers_receiver { get; set; }
        public virtual ICollection<Answer> answers_sender { get; set; }
        public virtual ICollection<PlayersPlayGames> players_play_games { get; set; }
    }
}
