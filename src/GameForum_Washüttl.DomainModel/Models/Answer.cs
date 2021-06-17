using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameForum_Washüttl.DomainModel.Models
{
    public partial class Answer
    {
        [StringLength(20)]
        [Key]
        [Required]
        public string a_p_receiver { get; set; }

        [StringLength(20)]
        [Key]
        [Required]
        public string a_p_sender { get; set; }

        [StringLength(40)]
        [Key]
        public string a_g_game { get; set; }

        [StringLength(60)]
        [Required]
        public string a_message { get; set; }

        public virtual Player receiver_object { get; set; }
        public virtual Player sender_object { get; set; }

    }
}
