﻿using System.ComponentModel.DataAnnotations;

namespace CSSHarris.Models.ChatModels
{
    public class Room
    {
        [Key]
        public string ID { get; set; }

        public string Owner { get; set; }

        [MaxLength(32)]
        [StringLength(32)]
        public string Title { get; set; }
        public Chatlog Chatlog { get; set; } = new();
    }
}