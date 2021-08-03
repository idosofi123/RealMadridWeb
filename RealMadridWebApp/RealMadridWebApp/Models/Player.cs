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
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name must contain only letters")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name must contain only letters")]
        public string LastName { get; set; }

        [Required]
        [Range(0, 99)]
        [Display(Name = "Shirt Number")]
        public int ShirtNumber { get; set; }

        [Display(Name = "Image Path")]
        public string ImagePath { get; set; }

        [Required]
        [Display(Name = "Position")]
        public int PositionId { get; set; }

        public Position Position { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [PlayerBirthDateValidation(ErrorMessage = "Official player must be at least 16 years old")]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [Required]
        [Display(Name = "Prefered Foot")]
        [EnumDataType(typeof(Foot))]
        public Foot PreferedFoot { get; set; }

        public Country BirthCountry { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int BirthCountryId { get; set; }

        [Display(Name = "Height(cm)")]
        [Range(0, 300)]
        public double Height { get; set; }

        [Display(Name = "Weight(kg)")]
        [Range(0, 400)]
        public double Weight { get; set; }

    }
}
