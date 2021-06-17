using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RealMadridWebApp.Models {

    public class Competition {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0, float.MaxValue, ErrorMessage = "Negative values are not allowed.")]
        [Display(Name = "Ticket Price")]
        public float TicketPrice { get; set; }

        public List<Match> Matches;

        [Display(Name = "Image Path")]
        public string ImagePath { get; set; }
    }
}
