using System;
using System.ComponentModel.DataAnnotations;

namespace RealMadridWebApp.Models {

    public class Match {

        public int Id { get; set; }

        [Required]
        public int TeamId { get; set; }

        public Team Team { get; set; }

        [Required]
        public bool isAway { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Negative values are not allowed.")]
        public int? GoalsHome { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Negative values are not allowed.")]
        public int? GoalsAway { get; set; }
    }
}
