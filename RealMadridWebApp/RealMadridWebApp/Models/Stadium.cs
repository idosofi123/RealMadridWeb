using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RealMadridWebApp.Models {

    public class Stadium {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Negative values are not allowed.")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Capacity { get; set; }

        [Required]
        [Range(-85, 85, ErrorMessage = "Please enter a value between 85 and -85.")]
        public double Latitude { get; set; }

        [Required]
        [Range(-180, 180, ErrorMessage = "Please enter a value between 180 and -180.")]
        public double Longitude { get; set; }

        [Display(Name = "Image Path")]
        public string ImagePath { get; set; }
    }
}
