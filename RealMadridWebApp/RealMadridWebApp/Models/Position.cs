using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RealMadridWebApp.Models
{
    public class Position
    {
        public int Id { get; set; }

        [Display(Name = "Image Path")]
        public string ImagePath { get; set; }

        [Display(Name = "Position")]
        public string PositionName { get; set; }

        [JsonIgnore]
        public List<Player> Players { get; set; }
    }
}
