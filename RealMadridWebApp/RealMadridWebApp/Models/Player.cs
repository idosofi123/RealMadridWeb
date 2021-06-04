using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RealMadridWebApp.Models
{

    public enum Foot
    {
        Left,
        Right
    }

    public class Player
    {

        public int PlayerId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Range(0, 99)]
        [Display(Name = "Shirt Number")]
        public int ShirtNumber { get; set; }

        [Display(Name = "Image Path")]
        public string ImagePath { get; set; }

        [Required]
        [Display(Name = "Position Id")]
        public int PositionId { get; set; }

        [Required]
        public Position Position { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        
        [Required]
        [Display(Name = "Prefered Foot")]
        [EnumDataType(typeof(Foot))]
        public Foot PreferedFoot { get; set; }

        [Required]
        public Country BirthCountry { get; set; }

        [Display(Name = "Height(cm)")]
        [Range(0,300)]
        public int Height { get; set; }

        [Display(Name = "Weight(kg)")]
        [Range(0, 400)]
        public int Weight { get; set; }

    }
}
