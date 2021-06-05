﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RealMadridWebApp.Models {

    public class Match {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Team")]
        public int TeamId { get; set; }

        public Team Team { get; set; }

        [Required]
        [Display(Name = "Is Away Match?")]
        public bool isAwayMatch { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Negative values are not allowed.")]
        [Display(Name = "Home Goals")]
        public int? HomeGoals { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Negative values are not allowed.")]
        [Display(Name = "Away Goals")]
        public int? AwayGoals { get; set; }

        public List<User> Users { get; set; }
    }
}
