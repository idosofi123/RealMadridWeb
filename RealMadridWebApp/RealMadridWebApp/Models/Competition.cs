
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealMadridWebApp.Models {

    public class Competition {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "Negative values are not allowed.")]
        [Display(Name = "Ticket Price")]
        public float TicketPrice { get; set; }

        public byte[] Image { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
