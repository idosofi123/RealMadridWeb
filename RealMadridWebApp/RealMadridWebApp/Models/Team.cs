using System.ComponentModel.DataAnnotations;

namespace RealMadridWebApp.Models {

    public class Team {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int StadiumId { get; set; }

        public Stadium Stadium { get; set; }

        [Display(Name = "Image Path")]
        public string ImagePath { get; set; }
    }
}
