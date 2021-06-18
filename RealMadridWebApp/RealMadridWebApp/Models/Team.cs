using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RealMadridWebApp.Models {

    public class Team {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Is Home Team?")]
        public bool IsHome { get; set; }

        [Required]
        [Display(Name = "Stadium")]
        public int StadiumId { get; set; }

        public Stadium Stadium { get; set; }

        [JsonIgnore]
        public List<Match> Matches { get; set; }

        [Display(Name = "Image Path")]
        public string ImagePath { get; set; }
    }
}
