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
        public int Capacity { get; set; }

        [Display(Name = "Image Path")]
        public string ImagePath { get; set; }
    }
}
