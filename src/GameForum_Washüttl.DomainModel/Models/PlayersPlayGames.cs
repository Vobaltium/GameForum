using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameForum_Washüttl.DomainModel.Models
{
    public partial class PlayersPlayGames
    {
        [Key]
        [Required]
        [StringLength(40)]
        public string pg_g_name { get; set; }

        [Key]
        [Required]
        [StringLength(20)]
        public string pg_p_name { get; set; }

        [Required]
        [StringLength(30)]
        public string pg_message { get; set; }

        public virtual Game game_object { get; set; }
        public virtual Player player_object { get; set; }
    }
}
